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
    class Unpacker : Unit
    {
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
        }
        public override void OnBlockPlaced()
        {
            name = "Unpacker Unit";
            InputNum = 1;
            OutputNum = 4;
            ControlNum = 0;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Unpacker Unit";
        }
        public override void UpdateUnit(Port Caller)
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
