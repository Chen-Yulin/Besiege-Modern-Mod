﻿using System;
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

        public MSlider[] InputChannel = new MSlider[2];
        public MSlider OutputChannel;

        public void UpdateCalType()
        {
        }

        public void UpdateInputPort()
        {
            foreach (var port in Inputs)
            {
                Destroy(port.gameObject);
            }
            Inputs.Clear();
            for (int i = 0; i < InputNum; i++)
            {
                GameObject vis = new GameObject();
                Port port = vis.AddComponent<Port>();
                port.InitPort(this, false, Data.DataType.Any, i, InputNum);
                Inputs.Add(port);
            }
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
                Debug.Log("Update Input Port " + i.ToString() + " to " + Inputs[i].Type.ToString());
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
                //Debug.Log("InputNumMenu.ValueChanged " + value.ToString());
                InputNum = value + 1;
                UpdateInputPort();
                UpdateInputMapper();
                UpdateInputType();
            };
            InputType[0].ValueChanged += (int value) =>
            {
                UpdateInputType();
            };
            InputType[1].ValueChanged += (int value) =>
            {
                UpdateInputType();
            };

            InputChannel[0] = AddSlider("Input Channel 1", "Input Channel 1", 0, 0, float.MaxValue);
            InputChannel[1] = AddSlider("Input Channel 2", "Input Channel 2", 0, 0, float.MaxValue);
            OutputChannel = AddSlider("Output Channel", "Output Channel", 0, 0, float.MaxValue);

        }
        public void Start()
        {
            name = "ALU";
            InitOutputPorts();
        }

    }
}
