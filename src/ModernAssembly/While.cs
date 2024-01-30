using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class While : Unit
    {
        public bool inloop = false;
        public bool DataTrue(Data data)
        {
            return data.Type == Data.DataType.Bool && data.Bool;
        }
        public bool DataFalse(Data data)
        {
            return data.Type == Data.DataType.Bool && !data.Bool;
        }
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
        }
        public override void OnBlockPlaced()
        {
            name = "While Unit";
            InputNum = 2;
            OutputNum = 2;
            ControlNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void OnUnitSimulateStart()
        {
            name = "While Unit";
            Controls[0].Type = Data.DataType.Bool;
            Inputs[1].usePulse = true;
            Controls[0].usePulse = true;
            Outputs[1].usePulse = true;
        }

        public override void UpdateUnit()
        {
            if ((Controls[0].MyData.Type == Data.DataType.Null && Inputs[0].MyData.Type != Data.DataType.Null) && !inloop) // start state
            {
                Debug.Log("Enter starting state");
                inloop = true;
                Outputs[1].MyData = Inputs[0].MyData; // will write data to control 0 and input 1
            }
            else if (inloop) // loop state
            {
                Debug.Log("Entering loop");
                if (Controls[0].pulsed && Inputs[1].pulsed) // when both control 0 and input 1 are updated
                {
                    Controls[0].pulsed = false;
                    Inputs[1].pulsed = false;
                    Debug.Log("Looping");
                    if (DataTrue(Controls[0].MyData))
                    {
                        Debug.Log("Loop true");
                        Debug.Log("Updating output 1");
                        Outputs[1].MyData = Inputs[1].MyData;
                        Debug.Log("Finish updating output 1");
                    }
                    else
                    {
                        if (Controls[0].Type == Data.DataType.Null)
                        {
                            Debug.Log("Loop null");
                        }
                        else
                        {
                            Debug.Log("Loop false");
                        }
                        inloop = false;
                        Outputs[0].MyData = Outputs[1].MyData;
                        Outputs[1].MyData = new Data();
                        Debug.Log("exit loop");
                    }
                }
                else
                {
                    Debug.Log("But not all port updated, waiting");
                }
            }
        }
        /*
        public override void UnitSimulateFixedUpdateHost()
        {
            if ((Controls[0].MyData.Type  == Data.DataType.Null && Inputs[0].MyData.Type != Data.DataType.Null) ||
                DataTrue(Controls[0].MyData))
            {
                if (Inputs[1].MyData.Type == Data.DataType.Null)
                {
                    Outputs[1].MyData = Inputs[0].MyData;
                }
                else
                {
                    Outputs[1].MyData = Inputs[1].MyData;
                }
            }
            else if (DataFalse(Controls[0].MyData))
            {
                Outputs[0].MyData = new Data(Outputs[1].MyData);
                Outputs[1].MyData = new Data();
            }
        }*/
    }
}
