using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class For : Unit
    {
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
        }
        public override void OnBlockPlaced()
        {
            name = "For Unit";
            InputNum = 2;
            OutputNum = 2;
            ControlNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "For Unit";
        }
    }
}
