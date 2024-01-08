using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Switch : Unit
    {
        

        public override void SafeAwake()
        {
        }
        public override void OnBlockPlaced()
        {
            name = "Switch Unit";
            InputNum = 1;
            OutputNum = 1;
            ControlNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "Switch Unit";
        }
    }
}
