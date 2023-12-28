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

namespace Modern
{
    public class ALU : Unit
    {
        List<string> DataTypeString = new List<string> {
            "Bool",
            "Float",
            "Vector2",
            "Vector3",
            "Quaternion",
        };

        List<string> FloatTypeString =new List<string>
        {
            "a + b",
            "a - b",
            "a x b",
            "a / b",
            "a % b",
            "a ^ b",
            "log_b (a)",
            "sin (a)",
            "cos (a)",
            "tan (a)",
            "asin (a)",
            "acos (a)",
            "atan (a)",
        };

        List<string> BoolTypeString = new List<string>
        {
            "AND",
            "OR",
            "XOR",
            "XAND",
            "NAND",
            "NOR",
        };

        List<string> VectorTypeString = new List<string>
        {
            "A + B",
            "A - B",
            "A x B",
            "A · B",
            "Abs(A)",
        };

        List<string> VectorFloatTypeString = new List<string>
        {
            "A x b",
            "A / b",
        };

        List<string> QuaternionTypeString = new List<string>
        {
            "A x B",
            "A / B",
        };

        public MMenu[] InputType = new MMenu[2];
        public MMenu TypeMenu;
        public MMenu InputNumMenu;

        public new MSlider[] InputChannel = new MSlider[2];
        public new MSlider[] InputSrcChannel = new MSlider[2]; // -1 for no connection
        public new MSlider[] OutputChannel = new MSlider[1];

        public bool InputNumChanged = false;
        public bool InputTypeChanged = false;

        public void UpdateCalType()
        {
        }

        public void InputNumChangeHandler()
        {
            InputNumChanged = false;
            //Debug.Log("InputNumMenu.ValueChanged " + value.ToString());
            InputNum = InputNumMenu.Value + 1;
            UpdateInputPort();
            UpdateInputMapper();
            UpdateInputType();
        }

        public void InputTypeChangeHandler()
        {
            InputTypeChanged = false;
            //Debug.Log("InputTypeMenu.ValueChanged " + value.ToString());
            UpdateInputType();
        }
        public void UpdateInputMapper()
        {
            InputType[1].DisplayInMapper = !(InputNum == 1);
        }
        public void UpdateInputType()
        {
            for (int i = 0; i < InputNum; i++)
            {
                Inputs[i].Type = (Data.DataType)Enum.Parse(typeof(Data.DataType), InputType[i].Selection);
            }
        }

        public override int GetPortKey(bool IO, int index)
        {
            if (IO)
            {
                return (int)OutputChannel[index].Value;
            }
            else
            {
                return (int)InputChannel[index].Value;
            }
        }

        public override void SaveInputSrcKey(int index, int key)
        {
            InputSrcChannel[index].SetValue(key);
            InputSrcChannel[index].ApplyValue();
        }

        public override void SaveInputKey(int index, int key)
        {
            InputChannel[index].SetValue(key);
            InputChannel[index].ApplyValue();
        }

        public override void SaveOutputKey(int index, int key)
        {
            OutputChannel[index].SetValue(key);
            OutputChannel[index].ApplyValue();
        }

        public void UpdateInputPort()
        {
            int j = 0;
            foreach (var port in Inputs)
            {
                Destroy(port.gameObject);
                j++;
            }
            Inputs.Clear();
            for (int i = 0; i < InputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, false, Data.DataType.Any, i, InputNum);
                Inputs.Add(port);
                int inputKey = WireManager.Instance.AddPort(port, (int)InputChannel[i].Value);
                SaveInputKey(i, inputKey);
            }
        }

        public override void InitOutputPorts()
        {
            for (int i = 0; i < OutputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, true, Data.DataType.Any, i, OutputNum);
                Outputs.Add(port);
                int outputKey = WireManager.Instance.AddPort(port, (int)OutputChannel[i].Value);
                SaveOutputKey(i, outputKey);
            }
        }

        public override void SafeAwake()
        {
            InputNumMenu = AddMenu("Input", 0, new List<string> { "1 input", "2 inputs" });
            InputType[0] = AddMenu("Input Type 1", 0, DataTypeString);
            InputType[1] = AddMenu("Input Type 2", 0, DataTypeString);
            TypeMenu = AddMenu("Type", 0, FloatTypeString);
            InputNumMenu.ValueChanged += (int value) =>
            {
                InputNumChanged = true;
            };
            InputType[0].ValueChanged += (int value) =>
            {
                InputTypeChanged = true;
            };
            InputType[1].ValueChanged += (int value) =>
            {
                InputTypeChanged = true;
            };

            InputChannel[0] = AddSlider("Input Channel 1", "Input Channel 1", 0, 0, float.MaxValue);
            InputChannel[1] = AddSlider("Input Channel 2", "Input Channel 2", 0, 0, float.MaxValue);
            InputSrcChannel[0] = AddSlider("Input Src Channel 1", "Input Src Channel 1", -1, -1, float.MaxValue);
            InputSrcChannel[1] = AddSlider("Input Src Channel 2", "Input Src Channel 2", -1, -1, float.MaxValue);
            OutputChannel[0] = AddSlider("Output Channel", "Output Channel", 0, 0, float.MaxValue);
        }
        public override void OnBlockPlaced()
        {
            name = "ALU";
            //UpdateInputPort();
            InitOutputPorts();
        }

        public override void OnSimulateStart()
        {
            name = "ALU";
        }

        public override void BuildingUpdate()
        {
            if (InputNumChanged)
            {
                InputNumChangeHandler();
            }
            if (InputTypeChanged)
            {
                InputTypeChangeHandler();
            }
        }

    }
}
