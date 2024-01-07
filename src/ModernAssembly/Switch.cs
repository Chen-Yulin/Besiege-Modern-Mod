using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Switch : Unit
    {
        public new int InputNum = 2;
        public new int OutputNum = 1;

        public override void InitInputPorts()
        {
            for (int i = 0; i < InputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, false, Data.DataType.Any, i, InputNum);
                Inputs.Add(port);
            }
        }

        public override void InitOutputPorts()
        {
            for (int i = 0; i < OutputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, true, Data.DataType.Any, i, OutputNum);
                Outputs.Add(port);
            }
        }

        public override void SafeAwake()
        {
        }
        public override void OnBlockPlaced()
        {
            name = "Switch Unit";
            InitInputPorts();
            InitOutputPorts();
        }

        public override void OnSimulateStart()
        {
            name = "Switch Unit";
        }
    }
}
