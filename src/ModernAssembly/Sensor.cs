using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    class Sensor : Unit
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

        public override void SafeAwake()
        {
            OnBoard = AddToggle("On Board", "OnBoard", "On Board", false);
            OnBoard.Toggled += (bool value) =>
            {
                _onBoardChanged = true;
            };
            SensorSafeAwake();
        }

        public override void BuildingUpdate()
        {
            if (_onBoardChanged)
            {
                _onBoardChanged = false;
                UpdateOutput();
            }
            SensorBuildUpdate();
        }

        public override void OnSimulateStart()
        {
            UpdateOutput();
            SensorSimulateStart();
            onboard = !OnBoard.isDefaultValue;
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
                    UnitSimulateFixedUpdateHost();
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
            else
            {

            }
            
        }

        public virtual void SensorBuildUpdate()
        {
            return;
        }
        public virtual void SensorSimulateStart()
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
