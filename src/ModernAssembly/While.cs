using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class While : Unit
    {
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
        }
        public override void OnBlockPlaced()
        {
            name = "While Unit";
            InputNum = 2;
            OutputNum = 2;
            ControlNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "While Unit";
        }

        public override void UnitSimulateFixedUpdateHost()
        {
            if ((Controls[0].MyData.Type  == Data.DataType.Null && Inputs[0].MyData.Type != Data.DataType.Null) ||
                DataTrue(Controls[0].MyData))
            {
                if (Inputs[1].MyData.Type == Data.DataType.Null)
                {
                    Outputs[1].MyData = Inputs[0].MyData;
                }
                else
                {
                    Outputs[1].MyData = Inputs[1].MyData;
                }
            }
            else if (DataFalse(Controls[0].MyData))
            {
                Outputs[0].MyData = new Data(Outputs[1].MyData);
                Outputs[1].MyData = new Data();
            }
        }
    }
}
