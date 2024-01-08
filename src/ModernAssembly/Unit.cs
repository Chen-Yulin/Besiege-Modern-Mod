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
using System.Xml.Serialization;

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

        public Board MotherBoard;

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

        public void FindMotherBoard()
        {
            foreach (var joint in GetComponent<BlockBehaviour>().iJointTo)
            {
                try
                {
                    MotherBoard = joint.gameObject.GetComponent<Board>();
                    Debug.Log("Mother board found");
                    break;
                }
                catch { }
            }
        }

        public void PortsFindConnection()
        {
            foreach (var port in Inputs)
            {
                port.FindConnectedPorts(MotherBoard);
            }
            foreach (var port in Outputs)
            {
                port.FindConnectedPorts(MotherBoard);
            }
            foreach (var port in Controls)
            {
                port.FindConnectedPorts(MotherBoard);
            }
        }

        public override void OnSimulateStart()
        {
            FindMotherBoard();
            PortsFindConnection();
            OnUnitSimulateStart();
        }

        public virtual void OnUnitSimulateStart()
        {
            return;
        }


        public virtual void UpdateUnit()
        {
            return;
        }
    }
}
