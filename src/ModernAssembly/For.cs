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
        public new int InputNum = 3;
        public new int OutputNum = 2;

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
            name = "For Unit";
            InitInputPorts();
            InitOutputPorts();
        }

        public override void OnSimulateStart()
        {
            name = "For Unit";
        }
    }
}
