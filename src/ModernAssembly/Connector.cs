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

        public void Awake()
        {

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
                            return;
                        }
                    }
                }
                RefPort = null;
            }
            else
            {
                RefPort = null;
            }
        }
        
    }
}
