using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modding.Modules;
using Modding;
using Modding.Blocks;
using UnityEngine;
using UnityEngine.Networking;
using Modding.Blocks;

namespace Modern
{
    public class Port : MonoBehaviour
    {
        public bool IO = false; // false = input, true = output
        public Data.DataType Type = Data.DataType.Null;
        public int Index = 0;

        private bool _asControl = false;
        public bool AsControl
        {
            set
            {
                if (!IO)
                {
                    _asControl = value;
                }
                else
                {
                    _asControl = false;
                }
            }
            get
            {
                return _asControl;
            }

        }

        public List<Port> _distPorts = new List<Port>(); // only for output
        private Port _srcPort = null; // only for input
        private Data _data = new Data();

        public Port SrcPort
        {
            get { return _srcPort; }
            set
            {
                if (IO)
                {
                    return;
                }
                if (value == _srcPort)
                {
                    return;
                }
                _srcPort = value;

                if (!SrcLine)
                {
                    InitSrcLine();
                }
                if (SrcLine)
                {
                    if (_srcPort == null)
                    {
                        SrcLine.enabled = false;
                    }
                    else
                    {
                        SrcLine.enabled = true;
                    }
                }

            }
        }

        private bool _highlight = false;
        public bool Highlight
        {
            get
            {
                return _highlight;
            }
            set
            {
                if (value == _highlight)
                {
                    return;
                }
                _highlight = value;
                if (_highlight)
                {
                    Vis.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"));
                    Vis.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else
                {
                    Vis.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
                    Vis.GetComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Port Texture").Texture;
                }
            }
        }

        public LineRenderer SrcLine;

        public Data MyData
        {
            get
            {
                return _data;
            }
            set
            {
                //Debug.Log(Index + (IO?" output":" input") + " port of " + parentUnit.name);
                if (_data.Equal(value))
                {
                    //Debug.Log("same data");
                    return;
                }

                if (Type != Data.DataType.Any && value.Type != Type)
                {
                    //Debug.Log("different type: " + Type + " " + value.Type);
                    if (_data.Type != Data.DataType.Null)
                    {
                        _data.Type = Data.DataType.Null;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    _data = value;
                }
                if (IO) // output
                {
                    //Debug.Log("as output to " + _distPorts.Count);
                    foreach (var port in _distPorts)
                    {
                        //Debug.Log("update dist port " + port);
                        if (!port.IO) // the connected input ports
                        {
                            port.MyData = value;
                        }
                    }
                }
                else
                {
                    parentUnit.UpdateUnit();
                }
            }
        }

        public Unit parentUnit;

        public GameObject Vis;

        public void InitPort(Unit unit, bool io, Data.DataType type, int index, int totalPort, bool asControl = false) 
        {
            Vis = gameObject;
            IO = io;
            AsControl = asControl;
            Type = type;
            parentUnit = unit;
            Index = index;
            InitPortVis(index, totalPort);
            //InitPortTrigger();
            name = (IO ? "Output" : (AsControl? "Control" : "Input")) + " Port " + index.ToString();
        }

        public float GetOffset(int index, int totalPort)
        {
            if (totalPort == 1)
            {
                return 0;
            }
            else
            {
                return 0.1f - 0.2f/(totalPort-1) * index;
            }
        }

        public void InitPortVis(int index, int totalPort)
        {
            Vis.transform.SetParent(parentUnit.transform);
            if (AsControl)
            {
                Vis.transform.localPosition = new Vector3(0.3f, GetOffset(index, totalPort), 0.05f);
                Vis.transform.localRotation = Quaternion.Euler(0, 90, 90);
            }
            else
            {
                Vis.transform.localPosition = new Vector3(GetOffset(index, totalPort), (IO ? 0.3f : -0.3f), 0.05f);
                Vis.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            
            Vis.transform.localScale = new Vector3(1f, 1f, (IO ? -1 : 1));
            MeshFilter MF = Vis.AddComponent<MeshFilter>();
            MF.sharedMesh = ModResource.GetMesh("Port Mesh").Mesh;
            MeshRenderer MR = Vis.AddComponent<MeshRenderer>();
            MR.material.mainTexture = ModResource.GetTexture("Port Texture").Texture;
        }
        public void InitPortTrigger()
        {
            SphereCollider SC = Vis.AddComponent<SphereCollider>();
            SC.isTrigger = true;
            SC.radius = 0.04f;
            SC.center = Vector3.zero;
        }

        public void SettlePorts(Board board)
        {
            Vector2 portCoord = Tool.GetBoardCoordinate(transform.position, board.transform);
            transform.position = board.transform.TransformPoint((Vector3)portCoord * 0.058f - (0.058f * 31f + 0.029f) * Vector3.one);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.05f);
        }

        public void FindConnectedPorts(Board board) // only for output
        {
            Vector2 portCoord = Tool.GetBoardCoordinate(transform.position, board.transform);
            transform.position = board.transform.TransformPoint((Vector3)portCoord * 0.058f - (0.058f * 31f + 0.029f) *Vector3.one);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.05f);

            List<Port> connected = board.FindConnectedPort(portCoord);
            //Debug.Log(connected.Count);
            foreach (var port in connected)
            {
                if (port != this)
                {
                    //Debug.Log("Add " + port.parentUnit.name + " " + port.Index);
                    AddDistConnection(port);
                }
            }
        }

        public void AddDistConnection(Port port) // port: the dist port (I), my: (O)
        {
            // only for output port
            if (IO && !port.IO)
            {
                if (!_distPorts.Contains<Port>(port))
                {
                    Port inputPort = port;
                    // my port is output, add dist port
                    _distPorts.Add(port);
                    // remove other reference to this dist port
                    if (inputPort.SrcPort)
                    {
                        inputPort.SrcPort._distPorts.Remove(port);
                    }
                    // modify the src port of the dist port
                    inputPort.SrcPort = this;

                }
            }
        }

        public void InitSrcLine()
        {
            try
            {
                SrcLine = gameObject.GetComponent<LineRenderer>();

                if (!SrcLine)
                {
                    SrcLine = gameObject.AddComponent<LineRenderer>();
                    SrcLine.enabled = false;
                    SrcLine.material.shader = Shader.Find("Unlit/Color");
                    SrcLine.material.color = Color.yellow;
                    SrcLine.SetWidth(0.02f, 0.02f);
                    SrcLine.SetVertexCount(4);
                }
            }
            catch { }
        }

        public void Awake()
        {
            
        }

        public void Start()
        {
            InitSrcLine();
        }

        public void LateUpdate()
        {
            if (SrcPort && SrcPort._distPorts.Contains(this) && SrcLine)
            {
                SrcLine.enabled = true;
                SrcLine.SetPosition(3, transform.position);
                SrcLine.SetPosition(2, transform.position + transform.up * transform.lossyScale.y * 0.2f);
                SrcLine.SetPosition(1, SrcPort.transform.position + transform.up * transform.lossyScale.y * 0.2f);
                SrcLine.SetPosition(0, SrcPort.transform.position);
            }
            else
            {
                SrcLine.enabled = false;
            }
        }

    }
}
