using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Modern
{
    public class Memory : Reg
    {
        public Dictionary<int, Data> memory = new Dictionary<int, Data>();
        
        public override void OnBlockPlaced()
        {
            name = "Memory Unit";
            InputNum = 3;
            OutputNum = 1;
            ControlNum = 2;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }
        public override void OnUnitSimulateStart()
        {
            name = "Memory Unit";
            Controls[0].Type = Data.DataType.Bool;
            Controls[1].Type = Data.DataType.Bool;
            Inputs[0].Type = Data.DataType.Any;
            Inputs[1].Type = Data.DataType.Float;
            Inputs[2].Type = Data.DataType.Float;
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
            bool valuechanged = false;
            if (triggered)
            {
                if (DataTrue(Controls[1].MyData))
                {
                    valuechanged = true;
                    int wrt_addr = (Inputs[2].MyData.Type == Data.DataType.Null) ? 0 : (int)Mathf.Round(Inputs[2].MyData.Flt);
                    Data tmp = new Data();
                    tmp.TempTexID = (uint)(Outputs[0].TempTexID * 1000 + wrt_addr);
                    tmp.DeepCopy(Inputs[0].MyData);
                    if (memory.ContainsKey(wrt_addr))
                    {
                        memory[wrt_addr] = tmp;
                    }
                    else
                    {
                        memory.Add(wrt_addr, tmp);
                    }

                }
            }

            if ((Caller.Index == 1 && !Caller.AsControl) || valuechanged)//read_addr
            {
                int read_addr = (Inputs[1].MyData.Type == Data.DataType.Null) ? 0 : (int)Mathf.Round(Inputs[1].MyData.Flt);
                Outputs[0].MyData = memory.ContainsKey(read_addr) ? memory[read_addr] : new Data();
            }
        }

    }
}
