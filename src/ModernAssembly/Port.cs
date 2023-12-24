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

        private Stack<Port> _connectedPorts = new Stack<Port>(); // only for output
        private Data _data = new Data();

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
                    foreach (var port in _connectedPorts)
                    {
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

        public void InitPort(Unit unit, bool io, Data.DataType type, int index, int totalPort) 
        {
            Vis = gameObject;
            IO = io;
            Type = type;
            parentUnit = unit;
            InitPortVis(index, totalPort);
            name = (IO ? "Output" : "Input") + " Port " + index.ToString();
            Debug.Log("Create Port " + (io ? "Output" : "Input") + " " + index.ToString() + " of " + totalPort.ToString() + " of " + unit.name + " with type " + type.ToString() + ".");
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

        public void AddConnection(Port port)
        {
            if (!_connectedPorts.Contains<Port>(port))
            {
                _connectedPorts.Push(port);
            }
        }

    }
}
