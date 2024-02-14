using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class While : Unit
    {
        public bool inloop = false;
        public bool DataTrue(Data data)
        {
            return data.Type == Data.DataType.Bool && data.Bool;
        }
        public bool DataFalse(Data data)
        {
            return data.Type == Data.DataType.Bool && !data.Bool;
        }
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
        }
        public override void OnBlockPlaced()
        {
            name = "While Unit";
            InputNum = 2;
            OutputNum = 3; // 0 for result, 1 for loop condition, 2 for loop body
            ControlNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "While Unit";
            Controls[0].Type = Data.DataType.Bool;
        }

        public override void UpdateUnit(Port Caller)
        {
            if (Caller.Index == 0 && !Caller.AsControl)
            {
                Debug.Log("Initial input");
                Outputs[1].MyData = Inputs[0].MyData; // judge the condition
                Debug.Log("initial condition" + Controls[0].MyData.Bool);
                Outputs[2].MyData = Inputs[0].MyData; // generate value of this loop
            }
            else if (Caller.Index == 1 && !Caller.AsControl)
            {
                Debug.Log("loop value changed, condition" + Controls[0].MyData.Bool);
                if (DataTrue(Controls[0].MyData))
                {
                    Outputs[1].MyData = Inputs[1].MyData; // judge the condition
                    Outputs[2].MyData = Inputs[1].MyData; // generate value of this loop
                }
                else
                {
                    Outputs[0].MyData = Inputs[1].MyData;
                }
            }
        }

    }
}
