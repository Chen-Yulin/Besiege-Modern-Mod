using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Switch : Sensor
    {
        public MKey SwitchKey;
        public MToggle DefaultOn;
        public MToggle AutoReturn;

        public bool useEmulate = false;

        public Transform Vis;

        private bool on = false;

        public bool On{
            get
            {
                return on;
            }
            set
            {
                if (on != value)
                {
                    on = value;
                    if (!Vis)
                    {
                        Vis = transform.Find("Vis");
                    }
                    if (Vis)
                    {
                        Vector3 scale = Vis.localScale;
                        scale.z = Mathf.Abs(scale.z) * (On?-1:1);
                        Vis.localScale = scale;
                    }
                }
            }
        }

        public override string GetName()
        {
            return "Switch";
        }

        public override void SensorSafeAwake()
        {
            SwitchKey = AddKey("Switch Key", "Switch Key", KeyCode.T);
            DefaultOn = AddToggle("Default On", "Default On", false);
            AutoReturn = AddToggle("Auto Reset", "Auto Reset", false);
        }

        public override void SensorSimulateStart()
        {
            On = !DefaultOn.isDefaultValue;
            Outputs[0].MyData = new Data(On);
        }

        public override Data SensorGenerate()
        {
            return new Data(On);
        }

        public override void SensorSimulateFixedUpdate()
        {
            if (AutoReturn.isDefaultValue)
            {
                if (SwitchKey.EmulationPressed())
                {
                    On = !On;
                }
            }
            else
            {
                if (SwitchKey.EmulationHeld())
                {
                    On = true;
                    useEmulate = true;
                }
                else if (useEmulate)
                {
                    On = false;
                }
            }
            

        }
        public override void SensorSimulateUpdate()
        {
            if (AutoReturn.isDefaultValue)
            {
                if (SwitchKey.IsPressed)
                {
                    On = !On;
                }
            }
            else
            {
                if (SwitchKey.IsHeld)
                {
                    On = true;
                    useEmulate = false;
                }
                else if (!useEmulate)
                {
                    On = false;
                }
            }
        }

    }
}
