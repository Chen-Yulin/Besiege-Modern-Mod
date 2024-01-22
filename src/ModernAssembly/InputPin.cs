using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class InputPin : Unit
    {
        public Transform Vis;

        public override void OnSimulateStart()
        {
            name = "Input Pin";
            InputNum = 0;
            ControlNum = 0;
            OutputNum = 1;
            Vis = transform.Find("Vis");
            Outputs.Add(Vis.gameObject.AddComponent<Port>());
            Outputs[0].InitPort(this, true, Data.DataType.Any, 0, 1, false, false);
        }
        public override void OnUnitSimulateStart()
        {
            name = "Input Pin";
        }
    }
}
