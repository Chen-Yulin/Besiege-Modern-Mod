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
        }

        public override void SensorSimulateStart()
        {
            On = !DefaultOn.isDefaultValue;
        }

        public override Data SensorGenerate()
        {
            return new Data(On);
        }

        public override void SensorSimulateFixedUpdate()
        {
            if (SwitchKey.EmulationPressed())
            {
                On = !On;
            }

        }
        public override void SensorSimulateUpdate()
        {
            if (SwitchKey.IsPressed)
            {
                On = !On;
            }
        }

    }
}
