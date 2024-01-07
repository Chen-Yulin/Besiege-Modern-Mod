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
    public class Unit : BlockScript
    {
        public int InputNum = 1;
        public int OutputNum = 1;
        public int ControlNum = 0;

        public List<Port> Inputs = new List<Port>();
        public List<Port> Controls = new List<Port>();
        public List<Port> Outputs = new List<Port>();

        public void InitControlPorts()
        {
            for (int i = 0; i < ControlNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, false, Data.DataType.Any, i, ControlNum, true);
                Controls.Add(port);
            }
        }
        public void InitInputPorts()
        {
            for (int i = 0; i < InputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, false, Data.DataType.Any, i, InputNum);
                Inputs.Add(port);
            }
        }   
        public void InitOutputPorts()
        {
            for (int i = 0; i < OutputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, true, Data.DataType.Any, i, OutputNum);
                Outputs.Add(port);
            }
        }

        public virtual void UpdateUnit()
        {
            return;
        }
    }
}
