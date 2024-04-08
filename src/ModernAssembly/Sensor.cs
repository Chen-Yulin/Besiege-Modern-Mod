using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Modern
{
    public class Sensor : Unit
    {
        public MToggle OnBoard;
        public MText OuputChannel;
        public MText InputChannel;

        private bool _onBoardChanged;

        public bool onboard;

        public virtual bool needPara() { return false; }

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
            if (needPara())
            {
                InputChannel.DisplayInMapper = OnBoard.isDefaultValue;
            }
            OuputChannel.DisplayInMapper = OnBoard.isDefaultValue;
        }

        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            OnBoard = AddToggle("On Board", "OnBoard", "On Board", false);
            if (needPara())
            {
                InputChannel = AddText("Parameter Channel", "P Channel", "");
            }
            OuputChannel = AddText("Send Channel", "Channel", "");
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
                if (needPara())
                {
                    UpdateInput();
                }
                UpdateOutput();
                UpdateMapper();
            }

            // left for customization
            SensorBuildUpdate();
        }

        public override void OnSimulateStart()
        {
            name = GetName();
            if (needPara())
            {
                UpdateInput();
            }
            UpdateOutput();
            
            onboard = !OnBoard.isDefaultValue;
            if (!onboard)
            {
                WirelessManager.Instance.RegisterUnit(InputChannel.Value, this);
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
                if (OuputChannel.Value != "")
                {
                    WirelessManager.Instance.PassData(OuputChannel.Value, SensorGenerate());
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
            SensorSimulateStop();
            if (!onboard)
            {
                WirelessManager.Instance.UnregisterChannel(InputChannel.Value);
            }
        }
        public override void UpdateUnit(Port Caller)
        {
            SensorUpdatePara();
        }
        public override void WirelessReceiveData(Data data)
        {
            WirelessSensorUpdatePara(data);
        }

        public virtual void SensorUpdatePara()
        {
            return;
        }
        public virtual void WirelessSensorUpdatePara(Data data)
        {
            return;
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
