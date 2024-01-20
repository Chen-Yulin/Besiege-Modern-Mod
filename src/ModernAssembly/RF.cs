using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class RF : Unit
    {
        public MText ReceiveChannel;
        public MText SendChannel;

        private bool useSend;
        private bool useReceive;

        public override void SafeAwake()
        {
            ReceiveChannel = AddText("Receive Channel", "Receive", "");
            SendChannel = AddText("Send Channel", "Send", "");
        }

        public override void OnBlockPlaced()
        {
            name = "RF Unit";
            InputNum = 1;
            ControlNum = 0;
            OutputNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "RF Unit";
            Inputs[0].Type = Data.DataType.Any;
            Outputs[0].Type = Data.DataType.Any;

            useSend = (SendChannel.Value != "");
            useReceive = (ReceiveChannel.Value != "");

            if (useReceive)
            {
                WirelessManager.Instance.RegisterPort(ReceiveChannel.Value, Outputs[0]);
            }
        }
        public override void OnSimulateStop()
        {
            WirelessManager.Instance.UnregisterChannel(ReceiveChannel.Value);
        }

        public override void UpdateUnit()
        {
            if (useSend)
            {
                WirelessManager.Instance.PassData(SendChannel.Value, Inputs[0].MyData);
            }
        }
    }
}
