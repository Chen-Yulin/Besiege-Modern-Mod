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

        public bool CheckInputs()
        {
            if (Inputs[0].MyData.Type == )
            {
                
            }
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
            bool res = false;
            switch (OptType.Value)
            {
                case 0:
                    res = Inputs[0].MyData.Bool
                    break;
                default:
                    break;
            }
        }
    }
}
