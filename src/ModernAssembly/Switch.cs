using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Switch : Unit
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

        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            SwitchKey = AddKey("Switch Key", "Switch Key", KeyCode.T);
            DefaultOn = AddToggle("Default On", "Default On", false);
        }
        public override void OnBlockPlaced()
        {
            name = "Switch Unit";
            InputNum = 0;
            ControlNum = 0;
            OutputNum = 1;
            InitOutputPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Switch Unit";
            Outputs[0].Type = Data.DataType.Bool;
            On = !DefaultOn.isDefaultValue;
            Outputs[0].MyData = new Data(On);
        }

        public override void UnitSimulateUpdateHost()
        {
            if (SwitchKey.IsPressed)
            {
                On = !On;
                Outputs[0].MyData = new Data(On);
            }
            
        }

    }
}
