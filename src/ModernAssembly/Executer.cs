using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Modern
{
    public class Executer : Unit
    {
        public MToggle OnBoard;
        public MText Channel;

        protected bool _onBoardChanged;

        public bool onboard;

        public void UpdateInput()
        {
            if (OnBoard.isDefaultValue)
            {
                InputNum = 0;
                int j = 0;
                foreach (var port in Inputs)
                {
                    Destroy(port.gameObject);
                    j++;
                }
                Inputs.Clear();
            }
            else
            {
                foreach (var port in Inputs)
                {
                    Destroy(port.gameObject);
                }
                Inputs.Clear();
                InputNum = 1;
                InitInputPorts();
            }
        }
        public void UpdateMapper()
        {
            Channel.DisplayInMapper = OnBoard.isDefaultValue;
        }

        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            OnBoard = AddToggle("On Board", "OnBoard", "On Board", false);
            Channel = AddText("Receive Channel", "Channel", "");
            OnBoard.Toggled += (bool value) =>
            {
                _onBoardChanged = true;
            };

            // left for customization
            ExecuterSafeAwake();
        }

        public override void OnBlockPlaced()
        {
            if (GetName()!=null)
            {
                name = GetName();
            }
            ControlNum = 0;
            OutputNum = 0;
        }

        public override void BuildingUpdate()
        {
            if (_onBoardChanged)
            {
                _onBoardChanged = false;
                UpdateInput();
                UpdateMapper();
            }

            // left for customization
            ExecuterBuildUpdate();
        }

        public override void OnSimulateStart()
        {
            name = GetName();
            UpdateInput();

            onboard = !OnBoard.isDefaultValue;
            if (!onboard)
            {
                ExecuterSimulateStart();
            }
        }

        public override void SimulateFixedUpdateHost()
        {
            if (onboard)
            {
                if (MotherBoard)
                {
                    if (!connectionInited)
                    {
                        connectionInited = true;
                        PortsFindConnection();
                    }
                    ExecuterSimulateFixedUpdate();
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

                        // left for customization
                        ExecuterSimulateStart();
                    }
                }
            }
            else
            {
                ExecuterSimulateFixedUpdate();
            }
        }
        public override void SimulateUpdateHost()
        {
            if (onboard)
            {
                if (MotherBoard)
                {
                    ExecuterSimulateUpdate();
                }
            }
            else
            {
                ExecuterSimulateUpdate();
            }
        }
        public override void OnSimulateStop()
        {
            WirelessManager.Instance.UnregisterChannel(Channel.Value);
            ExecuterSimulateStop();
        }

        public override void UpdateUnit(Port Caller)
        {
            ExecuterUpdateOuput(Caller);
        }

        public virtual string GetName()
        {
            return "Executer";
        }
        public virtual void ExecuterBuildUpdate()
        {
            return;
        }
        public virtual void ExecuterSimulateFixedUpdate()
        {
            return;
        }
        public virtual void ExecuterSimulateUpdate()
        {

        }
        public virtual void ExecuterSimulateStart()
        {

        }
        public virtual void ExecuterSimulateStop()
        {

        }
        public virtual void ExecuterSafeAwake()
        {
            return;
        }
        public virtual void ExecuterUpdateOuput(Port Caller)
        {
            return;
        }
    }
}
