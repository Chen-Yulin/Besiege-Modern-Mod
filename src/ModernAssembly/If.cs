using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class If : Unit
    {
        List<string> DataTypeString = new List<string> {
            "Bool",
            "Float"
        };
        List<string> OptTypeString = new List<string> {
            " == ",
            " !=", 
            " > ",
            " < ",
            " >= ",
            " <= ",
        };
        public MMenu InputType;
        public MMenu OptType;

        public bool CheckInput()
        {
            return Inputs[0].MyData.Type != Data.DataType.Null && Inputs[1].MyData.Type != Data.DataType.Null;
        }

        public override void SafeAwake()
        {
            InputType = AddMenu("Input Type 1", 0, DataTypeString);
            OptType = AddMenu("Opt Type", 0, OptTypeString);
        }
        public override void OnBlockPlaced()
        {
            name = "If Unit";
            InputNum = 2;
            OutputNum = 1;
            ControlNum = 0;
            InitInputPorts();
            InitOutputPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "If Unit";
            Outputs[0].Type = Data.DataType.Bool;
            Inputs[0].Type = (Data.DataType)Enum.Parse(typeof(Data.DataType), InputType.Selection);
            Inputs[1].Type = Inputs[0].Type;
        }

        public override void UpdateUnit()
        {
            if (CheckInput())
            {
                bool res = false;

                float i1, i2;

                if (Inputs[0].Type == Data.DataType.Bool)
                {
                    i1 = Inputs[0].MyData.Bool ? 1 : 0;
                    i2 = Inputs[1].MyData.Bool ? 1 : 0;
                }
                else
                {
                    i1 = Inputs[0].MyData.Flt;
                    i2 = Inputs[1].MyData.Flt;
                }

                switch (OptType.Value)
                {
                    case 0:
                        res = i1 == i2;
                        break;
                    case 1:
                        res = i1 != i2;
                        break;
                    case 2:
                        res = i1 > i2;
                        break;
                    case 3:
                        res = i1 < i2;
                        break;
                    case 4:
                        res = i1 >= i2;
                        break;
                    case 5:
                        res = i1 <= i2;
                        break;
                    default:
                        break;
                }
                Debug.Log("input: "+i1 + ", " + i2+"; output: "+res);
                Outputs[0].MyData = new Data(res);
            }
            else
            {
                Outputs[0].MyData = new Data();
            }
        }
    }
}
