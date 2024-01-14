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
        List<string> SingleDataTypeString = new List<string> {
            "Bool",
            "Float",
            "Vector2",
            "Vector3",
            "Quaternion",
        };

        List<string> TwoDataTypeString = new List<string>
        {
            "Bool",
            "Float",
            "Vector2",
            "Vector3",
            "Quaternion",
            "Vector2 & Float",
            "Vector3 & Float",
        };

        List<List<string>>[] OptTypeString = new List<List<string>>[]
        {
            new List<List<string>> // single data opt
            {
                new List<string>() // single bool opt
                {
                    "not",
                    "to float"
                },
                new List<string>() // single float opt
                {
                    "sign",
                    "abs",
                    "sin",
                    "cos",
                    "tan",
                    "asin",
                    "acos",
                    "atan",
                },
                new List<string>() // single Vector2 opt
                {
                    "normalize",
                    "magnitude",
                },
                new List<string>() // single Vector3 opt
                {
                    "normalize",
                    "magnitude",
                    "to quaternion",
                },
                new List<string>() // single quaternion
                {
                    "to vector3",
                }
            },
            new List<List<string>>() // two data opt
            {
                new List<string>() // two bool opt
                {
                    "and",
                    "or",
                    "xor",
                    "xand",
                    "nand",
                    "nor",
                },
                new List<string>() // two float opt
                {
                    "a + b",
                    "a - b",
                    "a x b",
                    "a / b",
                    "a % b",
                    "a ^ b",
                    "log_b (a)",
                },
                new List<string>() // two vecto2 opt
                {
                    "A + B",
                    "A - B",
                    "A x B",
                    "A · B",
                },
                new List<string>() // two vector 3 opt
                {
                    "A + B",
                    "A - B",
                    "A x B",
                    "A · B",
                },
                new List<string>() // two quaternion opt
                {
                    "A · B",
                    "A / B",
                },
                new List<string>() // vector2 & float opt
                {
                    "A · b",
                    "A / b",
                },
                new List<string>() // vector3 & float opt
                {
                    "A · b",
                    "A / b",
                },
            }
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

        public MMenu InputNumMenu;
        public MMenu OneTypeMenu;
        public MMenu TwoTypeMenu;
        public MMenu[] OneDataOptMenu = new MMenu[5];
        public MMenu[] TwoDataOptMenu = new MMenu[7];

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
        public void UpdateOptTypeMapper(int inputNum)
        {
            if (inputNum == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i != OneTypeMenu.Value)
                    {
                        OneDataOptMenu[i].DisplayInMapper = false;
                    }
                    else
                    {
                        OneDataOptMenu[i].DisplayInMapper = true;
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    TwoDataOptMenu[i].DisplayInMapper = false;
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    if (i != TwoTypeMenu.Value)
                    {
                        TwoDataOptMenu[i].DisplayInMapper = false;
                    }
                    else
                    {
                        TwoDataOptMenu[i].DisplayInMapper = true;
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    OneDataOptMenu[i].DisplayInMapper = false;
                }
            }
        }
        public void UpdateInputMapper()
        {
            if (InputNum == 1)
            {
                OneTypeMenu.DisplayInMapper = true;
                TwoTypeMenu.DisplayInMapper = false;
                UpdateOptTypeMapper(1);
            }
            else
            {
                OneTypeMenu.DisplayInMapper = false;
                TwoTypeMenu.DisplayInMapper = true;
                UpdateOptTypeMapper(2);
            }
        }
        public void UpdateInputType()
        {
            if (InputNum == 1)
            {
                Inputs[0].Type = (Data.DataType)Enum.Parse(typeof(Data.DataType), OneTypeMenu.Selection);
            }
            else
            {
                switch (OneTypeMenu.Value)
                {
                    case 0:
                        Inputs[0].Type = Data.DataType.Bool;
                        Inputs[1].Type = Data.DataType.Bool;
                        break;
                    case 1:
                        Inputs[0].Type = Data.DataType.Float;
                        Inputs[1].Type = Data.DataType.Float;
                        break;
                    case 2:
                        Inputs[0].Type = Data.DataType.Vector2;
                        Inputs[1].Type = Data.DataType.Vector2;
                        break;
                    case 3:
                        Inputs[0].Type = Data.DataType.Vector3;
                        Inputs[1].Type = Data.DataType.Vector3;
                        break;
                    case 4:
                        Inputs[0].Type = Data.DataType.Quaternion;
                        Inputs[1].Type = Data.DataType.Quaternion;
                        break;
                    case 5:
                        Inputs[0].Type = Data.DataType.Float;
                        Inputs[1].Type = Data.DataType.Vector2;
                        break;
                    case 6:
                        Inputs[0].Type = Data.DataType.Float;
                        Inputs[1].Type = Data.DataType.Vector3;
                        break;
                    case 7:
                        Inputs[0].Type = Data.DataType.Float;
                        Inputs[1].Type = Data.DataType.Quaternion;
                        break;
                }
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
            OneTypeMenu = AddMenu("One Type", 0, SingleDataTypeString);
            TwoTypeMenu = AddMenu("Two Type", 0, TwoDataTypeString);
            for (int i = 0; i < 5; i++)
            {
                string type = SingleDataTypeString[i];
                OneDataOptMenu[i] = AddMenu("One " + type + " Opt", 0, OptTypeString[0][i]);
            }
            for (int i = 0; i < 7; i++)
            {
                string type = TwoDataTypeString[i];
                TwoDataOptMenu[i] = AddMenu("Two " + type + " Opt", 0, OptTypeString[0][i]);
            }

            InputNumMenu.ValueChanged += (int value) =>
            {
                InputNumChanged = true;
            };
            OneTypeMenu.ValueChanged += (int value) =>
            {
                UpdateOptTypeMapper(1);
            };
            TwoTypeMenu.ValueChanged += (int value) =>
            {
                UpdateOptTypeMapper(2);
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

        public override void UpdateUnit()
        {
            if (InputNum == 1)
            {
                
            }
        }
    }
}
