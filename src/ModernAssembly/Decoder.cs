using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modding.Modules;
using Modding;
using Modding.Blocks;
using UnityEngine;
using UnityEngine.Networking;
using Modding.Blocks;

namespace Modern
{
    class Decoder : Unit
    {
        public override void SafeAwake()
        {
        }
        public override void OnBlockPlaced()
        {
            name = "Decoder Unit";
            InputNum = 1;
            OutputNum = 4;
            ControlNum = 0;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Decoder Unit";
        }
        public override void UpdateUnit()
        {
            if (CheckInputs())
            {
                for (int i = 0; i < 4; i++)
                {
                    Outputs[i].MyData = new Data(Inputs[0].MyData.Package.DataArr[i]);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Outputs[i].MyData = new Data();
                }
            }
            
        }
    }
}
