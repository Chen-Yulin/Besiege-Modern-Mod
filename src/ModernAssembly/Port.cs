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

        private List<Port> _distPorts = new List<Port>(); // only for output
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
                if (value == _data)
                {
                    return;
                }
                _data = value;
                if (IO) // output
                {
                    foreach (var port in _distPorts)
                    {
                        if (port.IO) // the connected input ports
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
            InitPortTrigger();
            name = (IO ? "Output" : "Input") + " Port " + index.ToString();
            //Debug.Log("Create Port " + (io ? "Output" : "Input") + " " + index.ToString() + " of " + totalPort.ToString() + " of " + unit.name + " with type " + type.ToString() + ".");
        }

        public float GetOffset(int index, int totalPort)
        {
            if (totalPort == 1)
            {
                return 0;
            }
            else
            {
                return -0.1f + 0.2f/(totalPort-1) * index;
            }
        }

        public void InitPortVis(int index, int totalPort)
        {
            Vis.transform.SetParent(parentUnit.transform);
            if (AsControl)
            {
                Vis.transform.localPosition = new Vector3(0.3f, GetOffset(index, totalPort), 0.05f);
                Vis.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
            SC.radius = 0.07f;
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
                    inputPort.SrcPort._distPorts.Remove(port);
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
                    SrcLine.material.color = Color.gray;
                    SrcLine.SetWidth(0.05f, 0.05f);
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
            if (SrcPort && SrcLine)
            {
                SrcLine.SetPosition(1, transform.position);
                SrcLine.SetPosition(0, SrcPort.transform.position);
            }
        }

    }
}
