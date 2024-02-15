using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    public class Reg : Unit
    {
        public bool DataTrue(Data data)
        {
            return data.Type == Data.DataType.Bool && data.Bool;
        }
        public override void OnBlockPlaced()
        {
            name = "Reg Unit";
            InputNum = 1;
            OutputNum = 1;
            ControlNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Reg Unit";
            Controls[0].Type = Data.DataType.Bool;
        }

        public override void UpdateUnit(Port Caller)
        {
            if (Caller.Index == 0 && Caller.AsControl && DataTrue(Controls[0].MyData)) // poseEdge of control 0
            {
                Outputs[0].MyData = Inputs[0].MyData;
            }
        }
    }
}
