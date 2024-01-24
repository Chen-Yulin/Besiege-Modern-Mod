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
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.06f, 0.06f, 1));
        }
        public void Start()
        {
            name = "Input Pin";
            transform.Find("Adding Point").GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.05f, 0.01f);
        }
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
