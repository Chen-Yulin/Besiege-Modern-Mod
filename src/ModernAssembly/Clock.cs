using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Clock : Unit
    {
        public MSlider Interval;

        private int cnt = 0;

        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            Interval = AddSlider("Interval", "Interval", 0.1f, 0.02f, 1);
        }
        public override void OnBlockPlaced()
        {
            name = "Clock Unit";
            InputNum = 0;
            ControlNum = 0;
            OutputNum = 1;
            InitOutputPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Clock Unit";
            Outputs[0].Type = Data.DataType.Bool;
        }

        public override void UnitSimulateFixedUpdateHost()
        {
            if (cnt < Mathf.RoundToInt(Interval.Value * 100) - 1)
            {
                if (cnt < Interval.Value * 50 - 1)
                {
                    Outputs[0].MyData = new Data(true);
                }
                else
                {
                    Outputs[0].MyData = new Data(false);
                }
                cnt++;
            }
            else
            {
                cnt = 0;
                Outputs[0].MyData = new Data(true);
            }
        }



    }
}
