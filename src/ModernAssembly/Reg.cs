using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    public class Reg : Unit
    {
        public List<String> modeStr = new List<String>{"Pos_Edge", "Neg_Edge", "Pos/Neg_Edge"};
        public MMenu TriggerMode;
        public bool DataTrue(Data data)
        {
            return data.Type == Data.DataType.Bool && data.Bool;
        }

        public override void SafeAwake()
        {
            TriggerMode = AddMenu("Mode",0, modeStr);
        }

        public override void OnBlockPlaced()
        {
            name = "Reg Unit";
            InputNum = 1;
            OutputNum = 1;
            ControlNum = 2;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Reg Unit";
            Controls[0].Type = Data.DataType.Bool;
            Controls[1].Type = Data.DataType.Bool;
        }

        public override void UpdateUnit(Port Caller)
        {
            bool triggered = false;
            switch (TriggerMode.Value)
            {
                case 0: // Pos_Edge
                    if (Caller.Index == 0 && Caller.AsControl && DataTrue(Controls[0].MyData))
                    {
                        triggered = true;
                    }
                    break;
                case 1: // Neg_Edge
                    if (Caller.Index == 0 && Caller.AsControl && !DataTrue(Controls[0].MyData))
                    {
                        triggered = true;
                    }
                    break;
                case 2: // Pos/Neg_Edge
                    if (Caller.Index == 0 && Caller.AsControl)
                    {
                        triggered = true;
                    }
                    break;
                default:
                    break;
            }
            if (triggered)
            {
                if (DataTrue(Controls[1].MyData))
                {
                    Outputs[0].MyData = Inputs[0].MyData;
                }
            }
        }
    }
}
