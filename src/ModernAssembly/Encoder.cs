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
    class Encoder : Unit
    {
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
        }
        public override void OnBlockPlaced()
        {
            name = "Encoder Unit";
            InputNum = 4;
            OutputNum = 1;
            ControlNum = 0;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Encoder Unit";
        }

        public override void UpdateUnit(Port Caller)
        {
            //Debug.Log("update encoder");
            Outputs[0].MyData = new Data(new M_Package(Inputs[0].MyData, Inputs[1].MyData, Inputs[2].MyData, Inputs[3].MyData));
        }
    }
}
