using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class KeyEmulator:Executer
    {
        public MKey key;

        public bool held = false;

        public BlockBehaviour bb;

        public override bool EmulatesAnyKeys { get { return true; } }
        public override string GetName()
        {
            return "Key Emulator";
        }

        public override void ExecuterSafeAwake()
        {
            key = AddEmulatorKey("EMULATE", "Target Key", KeyCode.C);
        }

        public override void ExecuterSimulateStart()
        {
            if (onboard)
            {
                Inputs[0].Type = Data.DataType.Bool;
            }
            bb = GetComponent<BlockBehaviour>();
        }

        public override void ExecuterUpdateOuput(Port Caller)
        {
            if (Caller.MyData.Type == Data.DataType.Bool)
            {
                if (Caller.MyData.Bool)
                {
                    if (!held)
                    {
                        EmulateKeys(new MKey[0], key, true);
                        held = true;
                    }
                }
                else
                {
                    if (held)
                    {
                        EmulateKeys(new MKey[0], key, false);
                        held = false;
                    }
                }
            }
            else
            {
                if (held)
                {
                    EmulateKeys(new MKey[0], key, false);
                    held = false;
                }
            }
        }

        public override void ExecuterWirelessReceiveData(Data data)
        {
            Debug.Log("data received");
            if (data.Type == Data.DataType.Bool)
            {
                if (data.Bool)
                {
                    if (!held)
                    {
                        EmulateKeys(new MKey[0], key, true);
                        held = true;
                    }
                }
                else
                {
                    if (held)
                    {
                        EmulateKeys(new MKey[0], key, false);
                        held = false;
                    }
                }
            }
            else
            {
                if (held)
                {
                    EmulateKeys(new MKey[0], key, false);
                    held = false;
                }
            }
        }

        public override void ExecuterSimulateStop()
        {
            if (held)
            {
                EmulateKeys(new MKey[0], key, false);
                held = false;
            }
        }
    }
}
