using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class InputPin : Unit
    {
        public Transform Vis;

        public OutputPin SrcPin;

        public LineRenderer SrcLine;

        public Driver driver;

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
                    SrcLine.material.color = Color.green;
                    SrcLine.SetWidth(0.02f, 0.02f);
                    SrcLine.SetVertexCount(4);
                }
            }
            catch { }
        }

        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.06f, 0.06f, 1));
        }
        public void Start()
        {
            name = "Input Pin";
            transform.Find("Adding Point").GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.05f, 0.01f);
        }
        public override void OnSimulateStart()
        {
            name = "Input Pin";
            InputNum = 0;
            ControlNum = 0;
            OutputNum = 1;
            Vis = transform.Find("Vis");
            Outputs.Add(Vis.gameObject.AddComponent<Port>());
            Outputs[0].InitPort(this, true, Data.DataType.Any, 0, 1, false, false);
            InitSrcLine();
        }
        public override void OnUnitSimulateStart()
        {
            name = "Input Pin";
        }

        public override void UnboardUnitSimulateFixedUpdateHost()
        {
            if (!driver)
            {
                foreach (var joint in GetComponent<BlockBehaviour>().iJointTo)
                {
                    if (joint.connectedBody)
                    {
                        driver = joint.connectedBody.gameObject.GetComponent<Driver>();
                        if (driver)
                        {
                            Debug.Log("Driver found");
                        }
                        
                    }
                }
            }
            if (driver)
            {
                driver.DriverGetData(Outputs[0].MyData);
            }
        }

        public override void SimulateLateUpdateAlways()
        {
            if (SrcPin && SrcPin.DstPins.Contains(this) && SrcLine)
            {
                SrcLine.enabled = true;
                SrcLine.SetPosition(3, transform.position);
                SrcLine.SetPosition(2, transform.position + transform.forward * transform.lossyScale.z * 0.2f);
                SrcLine.SetPosition(1, SrcPin.transform.position + SrcPin.transform.forward * SrcPin.transform.lossyScale.z * 0.2f);
                SrcLine.SetPosition(0, SrcPin.transform.position);
            }
            else
            {
                SrcLine.enabled = false;
            }
        }
    }
}
