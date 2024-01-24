using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    internal class OutputPin : Unit
    {
        public Transform Vis;
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.06f, 0.06f, 1));
        }
        public void Start()
        {
            name = "Output Pin";
            transform.Find("Adding Point").GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.05f, 0.01f);
        }
        public override void OnSimulateStart()
        {
            name = "Output Pin";
            InputNum = 1;
            ControlNum = 0;
            OutputNum = 0;
            Vis = transform.Find("Vis");
            Inputs.Add(Vis.gameObject.AddComponent<Port>());
            Inputs[0].InitPort(this, false, Data.DataType.Any, 0, 1, false, false);
        }
        public override void OnUnitSimulateStart()
        {
            name = "Output Pin";
        }
        public override void UpdateUnit()
        {
            base.UpdateUnit();
        }
    }
}
