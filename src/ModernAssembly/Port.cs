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

        private List<int> _distPortsKey = new List<int>(); // only for output
        private int _srcPortKey = -1; // only for input
        private Data _data = new Data();

        public int SrcPort
        {
            get { return _srcPortKey; }
            set
            {
                if (IO)
                {
                    return;
                }
                if (value == _srcPortKey)
                {
                    return;
                }
                _srcPortKey = value;

                if (!SrcLine)
                {
                    InitSrcLine();
                }
                if (SrcLine)
                {
                    if (WireManager.Instance.GetPort(_srcPortKey) == null)
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

        public int MapperKey
        {
            get
            {
                return parentUnit.GetPortKey(IO, Index);
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
                    foreach (var port in _distPortsKey)
                    {
                        if (!WireManager.Instance.GetPort(port).IO) // the connected input ports
                        {
                            WireManager.Instance.GetPort(port).MyData = value;
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

        public void InitPort(Unit unit, bool io, Data.DataType type, int index, int totalPort) 
        {
            Vis = gameObject;
            IO = io;
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
            Vis.transform.localPosition = new Vector3(GetOffset(index,totalPort), (IO ? 0.3f : -0.3f), 0.05f);
            Vis.transform.localRotation = Quaternion.Euler(90,0,0);
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

        public void AddDistConnection(int portKey) // portKey: the mapper key of the dist port
        {
            // only for output port
            if (IO)
            {
                if (!_distPortsKey.Contains<int>(portKey))
                {
                    Port inputPort = WireManager.Instance.GetPort(portKey);
                    // my port is output, add dist port
                    _distPortsKey.Add(portKey);
                    // remove other reference to this dist port
                    WireManager.Instance.GetPort(inputPort.SrcPort)._distPortsKey.Remove(portKey);
                    // modify the src port of the dist port
                    inputPort.SrcPort = MapperKey;
                    // save the src port to the unit of this dist port
                    inputPort.parentUnit.SaveInputSrcKey(inputPort.Index, MapperKey);
                }
            }
        }

        public void UpdateDistKey(int oldKey, int newKey) 
        {
            // only for output
            if (IO)
            {
                if (_distPortsKey.Contains<int>(oldKey))
                {
                    _distPortsKey.Remove(oldKey);
                    _distPortsKey.Add(newKey);
                }
            }
        }

        public void UpdateSrcKey(int oldKey, int newKey)
        {
            // only for input
            if (!IO)
            {
                if (SrcPort == oldKey)
                {
                    SrcPort = newKey;
                    parentUnit.SaveInputSrcKey(Index, newKey);
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
            if (WireManager.Instance.GetPort(SrcPort) && SrcLine)
            {
                
                SrcLine.SetPosition(1, transform.position);
                SrcLine.SetPosition(0, WireManager.Instance.GetPort(SrcPort).transform.position);
            }
        }

    }
}
