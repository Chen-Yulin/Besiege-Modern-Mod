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
    public class Connector : SingleInstance<Connector>
    {
        public override string Name { get; } = "Wire Connector";

        public bool Enabled = false;

        private Port _port = null;
        public Port RefPort
        {
            get
            {
                return _port;
            }
            set
            {
                if (value == _port)
                {
                    return;
                }
                if (_port)
                {
                    _port.Highlight = false;
                }
                _port = value;
                if (_port)
                {
                    _port.Highlight = true;
                }
            }
        }
        public Port PrePort;

        public LineRenderer Line;

        RaycastHit[] RaycastAllSorted(Ray ray, float dist)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, dist);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        public void CreateConnection(Port p1, Port p2)
        {
            if (p1 && p2 && (p1.IO != p2.IO))
            {
                if (p1.IO) // p1 output p2 input
                {
                    p1.AddDistConnection(p2.MapperKey);
                }
                else // p2 output p1 input
                {
                    p2.AddDistConnection(p1.MapperKey);
                }
                Debug.Log("Create connection between " + p1.MapperKey + " and " + p2.MapperKey);
            }
        }

        public void Awake()
        {
            Line = gameObject.AddComponent<LineRenderer>();
            Line.enabled = false;
            Line.SetWidth(0.05f, 0.05f);
        }
        public void Start()
        {
        }

        public void Update()
        {
            if (Enabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = RaycastAllSorted(ray, 10f);
                bool getPort = false;
                if (hits.Length > 0)
                {
                    foreach (var hit in hits)
                    {
                        if (!hit.collider.isTrigger)
                        {
                            break;
                        }
                        Port port = hit.collider.gameObject.GetComponent<Port>();
                        if (port)
                        {
                            RefPort = port;
                            getPort = true;
                            break;
                        }
                    }
                }
                if (!getPort)
                {
                    RefPort = null;
                }


                if (Input.GetMouseButtonDown(0))
                {
                    if (RefPort)
                    {
                        PrePort = RefPort;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    CreateConnection(PrePort, RefPort);
                    PrePort = null;
                }
                if (Input.GetMouseButton(0))
                {
                    if (PrePort)
                    {
                        Line.enabled = true;
                        Line.SetPosition(0, PrePort.transform.position);
                        if (RefPort)
                        {
                            Line.SetPosition(1, RefPort.transform.position);
                        }
                        else
                        {
                            Vector3 normal = PrePort.transform.position - Camera.main.transform.position;
                            Vector3 screenPoint = Input.mousePosition;
                            screenPoint.z = normal.magnitude;
                            Line.SetPosition(1, Camera.main.ScreenToWorldPoint(screenPoint));
                        }
                    }
                    else
                    {
                        Line.enabled = false;
                    }
                }
                else
                {
                    Line.enabled = false;
                }
            }
            else
            {
                PrePort = null;
                RefPort = null;
            }

            

        }
        
    }
}
