using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Mux : Unit
    {
        public List<String> TypeStr = new List<String> { "Bool", "Float" };
        public List<String> InputsStr = new List<String> { "2", "3", "4"};
        public MMenu ControlType;
        public MMenu InputCnt;

        public bool InputNumChanged = false;

        public void InputNumChangeHandler()
        {
            InputNumChanged = false;
            //Debug.Log("InputNumMenu.ValueChanged " + value.ToString());
            if (ControlType.Value == 0)
            {
                InputNum = 2;
            }
            else
            {
                InputNum = InputCnt.Value + 2;
            }
            UpdateInputPort();
        }

        public void OnControlTypeChanged(int controlType)
        {
            if (controlType == 0)
            {
                InputNum = 2;
                InputNumChanged = true;
            }
            InputCnt.DisplayInMapper = (controlType == 1);
        }
        public void UpdateInputPort()
        {
            // first clear
            int j = 0;
            foreach (var port in Inputs)
            {
                Destroy(port.gameObject);
                j++;
            }
            Inputs.Clear();
            // then init again
            InitInputPorts();
        }
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            ControlType = AddMenu("Input Type 1", 0, TypeStr);
            InputCnt = AddMenu("Opt Type", 0, InputsStr);

            ControlType.ValueChanged += (int value) =>
            {
                OnControlTypeChanged(value);
            };
            InputCnt.ValueChanged += (int value) =>
            {
                InputNumChanged = true;
            };
        }
        public override void OnBlockPlaced()
        {
            name = "Mux Unit";
            OutputNum = 1;
            ControlNum = 1;
            InitOutputPorts();
            InitControlPorts();
        }

        public override void BuildingUpdate()
        {
            if (InputNumChanged)
            {
                InputNumChangeHandler();
            }
        }

        public override void OnUnitSimulateStart()
        {
            name = "Mux Unit";
            Outputs[0].Type = Data.DataType.Any;
            Controls[0].Type = ControlType.Value == 0 ? Data.DataType.Bool : Data.DataType.Float;
        }

        public override void UpdateUnit(Port Caller)
        {
            if (Controls[0].MyData.Type == Data.DataType.Null)
            {
                Outputs[0].MyData = new Data();
            }
            else
            {
                if (ControlType.Value == 0) // bool signal
                {
                    if (Controls[0].MyData.Bool) // true
                    {
                        Outputs[0].MyData = Inputs[1].MyData;
                    }
                    else // false
                    {
                        Outputs[0].MyData = Inputs[0].MyData;
                    }
                }
                else // float signal
                {
                    int channel = Mathf.RoundToInt(Mathf.Clamp(Controls[0].MyData.Flt, 0, InputNum - 1));
                    Outputs[0].MyData = Inputs[channel].MyData;
                }
            }
        }


    }
}
