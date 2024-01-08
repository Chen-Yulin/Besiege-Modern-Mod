using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class If : Unit
    {

        public override void SafeAwake()
        {
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
        }
    }
}
