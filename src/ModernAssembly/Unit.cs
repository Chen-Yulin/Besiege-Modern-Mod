﻿using System;
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

        protected int frameCnt = 0;
        protected bool connectionInited = false;

        public void InitControlPorts()
        {
            Controls.Clear();
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
            Inputs.Clear();
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
            Outputs.Clear();
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
            try
            {
                foreach (var joint in GetComponent<BlockBehaviour>().iJointTo)
                {
                    try
                    {
                        MotherBoard = joint.connectedBody.gameObject.GetComponent<Board>();
                        if (MotherBoard)
                        {
                            //Debug.Log("Mother board found");
                            break;
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        public void AddAllPortsToBoard()
        {
            foreach (var port in Inputs)
            {
                Vector2 portCoord = Tool.GetBoardCoordinate(port.transform.position, MotherBoard.transform);
                if (MotherBoard.AttachedPorts.ContainsKey(portCoord))
                {
                    MotherBoard.AttachedPorts[portCoord].Add(port);
                }
                else
                {
                    MotherBoard.AttachedPorts.Add(portCoord, new List<Port>() { port });
                }
                //Debug.Log("Add port to board at " + portCoord.ToString());
            }
            foreach (var port in Outputs)
            {
                Vector2 portCoord = Tool.GetBoardCoordinate(port.transform.position, MotherBoard.transform);
                if (MotherBoard.AttachedPorts.ContainsKey(portCoord))
                {
                    MotherBoard.AttachedPorts[portCoord].Add(port);
                }
                else
                {
                    MotherBoard.AttachedPorts.Add(portCoord, new List<Port>() { port });
                }
                //Debug.Log("Add port to board at " + portCoord.ToString());
            }
            foreach (var port in Controls)
            {
                Vector2 portCoord = Tool.GetBoardCoordinate(port.transform.position, MotherBoard.transform);
                if (MotherBoard.AttachedPorts.ContainsKey(portCoord))
                {
                    MotherBoard.AttachedPorts[portCoord].Add(port);
                }
                else
                {
                    MotherBoard.AttachedPorts.Add(portCoord, new List<Port>() { port });
                }
                //Debug.Log("Add port to board at " + portCoord.ToString());
            }
        }

        public void PortsFindConnection()
        {
            foreach (var port in Outputs)
            {
                port.FindConnectedPorts(MotherBoard);
            }
            foreach (var port in Inputs)
            {
                port.SettlePorts(MotherBoard);
            }
            foreach (var port in Controls)
            {
                port.SettlePorts(MotherBoard);
            }
        }

        public void ClearPorts()
        {
            foreach (var port in Inputs)
            {
                Destroy(port.gameObject);
            }
            foreach (var port in Outputs)
            {
                Destroy(port.gameObject);
            }
            foreach (var port in Controls)
            {
                Destroy(port.gameObject);
            }
        }

        public bool CheckInputs()
        {
            foreach (var port in Inputs)
            {
                if (port.MyData.Type == Data.DataType.Null)
                {
                    return false;
                }
            }
            foreach (var port in Controls)
            {
                if (port.MyData.Type == Data.DataType.Null)
                {
                    return false;
                }
            }
            return true;
        }
        public void PassOutput()
        {
            foreach (var port in Outputs)
            {
                port.MyData = port.MyData;
            }
        }

        public override void OnSimulateStart()
        {
            ClearPorts();
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void SimulateUpdateHost()
        {
            if (connectionInited)
            {
                UnitSimulateUpdateHost();
            }
        }
        public override void SimulateFixedUpdateHost()
        {
            if (MotherBoard)
            {
                if (!connectionInited)
                {
                    connectionInited = true;
                    PortsFindConnection();
                }
                UnitSimulateFixedUpdateHost();// one frame ahead of sensors
            }
            else if (frameCnt > 2)
            {
                UnboardUnitSimulateFixedUpdateHost();
            }
            if (frameCnt < 2)
            {
                frameCnt++;
            }
            else if (frameCnt == 2)
            {
                frameCnt++;
                FindMotherBoard();
                if (MotherBoard)
                {
                    AddAllPortsToBoard();
                    OnUnitSimulateStart();
                }
            }
        }

        

        public virtual void OnUnitSimulateStart()
        {
            return;
        }
        public virtual void UnitSimulateFixedUpdateHost()
        {
            return;
        }
        public virtual void UnboardUnitSimulateFixedUpdateHost()
        {
            return;
        }
        public virtual void UnitSimulateUpdateHost()
        {
            return;
        }
        public virtual void UpdateUnit(Port Caller)
        {
            return;
        }
        public virtual void WirelessReceiveData(Data data)
        {
            return;
        }
        public string DebugString()
        {
            string res = "";
            res += name + "\n";
            if (InputNum != 0)
            {
                res += "\n=== INPUT (" + InputNum.ToString() + ") ===\n";
                foreach (var input in Inputs)
                {
                    res += "∟ " + input.Index.ToString() + ": \n" + input.MyData.ToString(0) + "\n";
                }
            }
            if (ControlNum != 0)
            {
                res += "\n=== CONTROL (" + ControlNum.ToString() + ") ===\n";
                foreach (var control in Controls)
                {
                    res += "∟ " + control.Index.ToString() + ": \n" + control.MyData.ToString(0) + "\n";
                }
            }
            if (OutputNum != 0)
            {
                res += "\n=== OUTPUT (" + OutputNum.ToString() + ") ===\n";
                foreach (var output in Outputs)
                {
                    res += "∟ " + output.Index.ToString() + ": \n" + output.MyData.ToString(0) + "\n";
                }
            }
            return res;
        }
    }
}
