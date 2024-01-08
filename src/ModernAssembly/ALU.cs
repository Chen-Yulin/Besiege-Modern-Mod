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
        }
        public override void OnBlockPlaced()
        {
            name = "ALU";
            OutputNum = 1;
            InitOutputPorts();
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
        public override void OnUnitSimulateStart()
        {
            name = "ALU";
        }
    }
}
