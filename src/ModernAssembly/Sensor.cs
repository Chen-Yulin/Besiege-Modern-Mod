using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Sensor : Unit
    {
        public MToggle OnBoard;
        public MText Channel;

        private bool _onBoardChanged;

        public bool onboard;

        public void UpdateOutput()
        {
            if (OnBoard.isDefaultValue)
            {
                OutputNum = 0;
                int j = 0;
                foreach (var port in Outputs)
                {
                    Destroy(port.gameObject);
                    j++;
                }
                Outputs.Clear();
            }
            else
            {
                foreach (var port in Outputs)
                {
                    Destroy(port.gameObject);
                }
                Outputs.Clear();
                OutputNum = 1;
                InitOutputPorts();
            }
        }
        public void UpdateMapper()
        {
            if (OnBoard.isDefaultValue)
            {
                Channel.DisplayInMapper = true;
            }
            else
            {
                Channel.DisplayInMapper = false;
            }
        }

        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            OnBoard = AddToggle("On Board", "OnBoard", "On Board", false);
            Channel = AddText("Channel", "Channel", "");
            OnBoard.Toggled += (bool value) =>
            {
                _onBoardChanged = true;
            };

            // left for customization
            SensorSafeAwake();
        }

        public override void OnBlockPlaced()
        {
            name = GetName();
            InputNum = 0;
            ControlNum = 0;
        }

        public override void BuildingUpdate()
        {
            if (_onBoardChanged)
            {
                _onBoardChanged = false;
                UpdateOutput();
                UpdateMapper();
            }

            // left for customization
            SensorBuildUpdate();
        }

        public override void OnSimulateStart()
        {
            name = GetName();
            UpdateOutput();
            
            onboard = !OnBoard.isDefaultValue;
            if (!onboard)
            {
                SensorSimulateStart();
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
                    SensorSimulateFixedUpdate();
                    Data res = SensorGenerate();
                    Outputs[0].MyData = res;
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
                        SensorSimulateStart();
                    }
                }
            }
            else
            {
                SensorSimulateFixedUpdate();
                if (Channel.Value != "")
                {
                    WirelessManager.Instance.PassData(Channel.Value, SensorGenerate());
                }
            }
        }
        public override void SimulateUpdateHost()
        {
            if (onboard)
            {
                if (MotherBoard)
                {
                    SensorSimulateUpdate();
                }
            }
            else
            {
                SensorSimulateUpdate();
            }
        }
        public override void OnSimulateStop()
        {
            WirelessManager.Instance.UnregisterChannel(Channel.Value);
            SensorSimulateStop();
        }
        public virtual string GetName()
        {
            return "Sensor";
        }
        public virtual void SensorBuildUpdate()
        {
            return;
        }
        public virtual void SensorSimulateFixedUpdate()
        {
            return;
        }
        public virtual void SensorSimulateUpdate()
        {
            
        }
        public virtual void SensorSimulateStart()
        {

        }
        public virtual void SensorSimulateStop()
        {

        }
        public virtual void SensorSafeAwake()
        {
            return;
        }
        public virtual Data SensorGenerate()
        {
            Data data = new Data();
            return data;
        }
    }
}
