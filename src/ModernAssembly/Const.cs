using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Const : Unit
    {
        List<string> DataTypeString = new List<string> {
              "Bool",
              "Float",
              "Vector2",
              "Vector3",
              "Quaternion",
        };
        public MMenu TypeMenu;
        public MMenu BoolSelection;
        public MSlider FloatSelection;
        public MSlider[] Vector2Selection = new MSlider[2];
        public MSlider[] Vector3Selection = new MSlider[3];
        public MSlider[] QuaternionSelection = new MSlider[4];

        public bool TypeChanged = false;

        private int initCount = 0;

        public void TypeChangeHandler()
        {
            switch (TypeMenu.Value)
            {
                case 0:
                    BoolSelection.DisplayInMapper = true;
                    FloatSelection.DisplayInMapper = false;
                    Vector2Selection[0].DisplayInMapper = false;
                    Vector2Selection[1].DisplayInMapper = false;
                    Vector3Selection[0].DisplayInMapper = false;
                    Vector3Selection[1].DisplayInMapper = false;
                    Vector3Selection[2].DisplayInMapper = false;
                    QuaternionSelection[0].DisplayInMapper = false;
                    QuaternionSelection[1].DisplayInMapper = false;
                    QuaternionSelection[2].DisplayInMapper = false;
                    QuaternionSelection[3].DisplayInMapper = false;
                    break;
                case 1:
                    BoolSelection.DisplayInMapper = false;
                    FloatSelection.DisplayInMapper = true;
                    Vector2Selection[0].DisplayInMapper = false;
                    Vector2Selection[1].DisplayInMapper = false;
                    Vector3Selection[0].DisplayInMapper = false;
                    Vector3Selection[1].DisplayInMapper = false;
                    Vector3Selection[2].DisplayInMapper = false;
                    QuaternionSelection[0].DisplayInMapper = false;
                    QuaternionSelection[1].DisplayInMapper = false;
                    QuaternionSelection[2].DisplayInMapper = false;
                    QuaternionSelection[3].DisplayInMapper = false;
                    break;
                case 2:
                    BoolSelection.DisplayInMapper = false;
                    FloatSelection.DisplayInMapper = false;
                    Vector2Selection[0].DisplayInMapper = true;
                    Vector2Selection[1].DisplayInMapper = true;
                    Vector3Selection[0].DisplayInMapper = false;
                    Vector3Selection[1].DisplayInMapper = false;
                    Vector3Selection[2].DisplayInMapper = false;
                    QuaternionSelection[0].DisplayInMapper = false;
                    QuaternionSelection[1].DisplayInMapper = false;
                    QuaternionSelection[2].DisplayInMapper = false;
                    QuaternionSelection[3].DisplayInMapper = false;
                    break;
                case 3:
                    BoolSelection.DisplayInMapper = false;
                    FloatSelection.DisplayInMapper = false;
                    Vector2Selection[0].DisplayInMapper = false;
                    Vector2Selection[1].DisplayInMapper = false;
                    Vector3Selection[0].DisplayInMapper = true;
                    Vector3Selection[1].DisplayInMapper = true;
                    Vector3Selection[2].DisplayInMapper = true;
                    QuaternionSelection[0].DisplayInMapper = false;
                    QuaternionSelection[1].DisplayInMapper = false;
                    QuaternionSelection[2].DisplayInMapper = false;
                    QuaternionSelection[3].DisplayInMapper = false;
                    break;
                case 4:
                    BoolSelection.DisplayInMapper = false;
                    FloatSelection.DisplayInMapper = false;
                    Vector2Selection[0].DisplayInMapper = false;
                    Vector2Selection[1].DisplayInMapper = false;
                    Vector3Selection[0].DisplayInMapper = false;
                    Vector3Selection[1].DisplayInMapper = false;
                    Vector3Selection[2].DisplayInMapper = false;
                    QuaternionSelection[0].DisplayInMapper = true;
                    QuaternionSelection[1].DisplayInMapper = true;
                    QuaternionSelection[2].DisplayInMapper = true;
                    QuaternionSelection[3].DisplayInMapper = true;
                    break;
                default:
                    break;
            }
        }

        public override void SafeAwake()
        {
            TypeMenu = AddMenu("Type", 0, DataTypeString);
            TypeMenu.ValueChanged += (int value) =>
            {
                TypeChanged = true;
            };
            BoolSelection = AddMenu("Bool", 0, new List<string> { "False", "True" });
            FloatSelection = AddSlider("Float","Float", 0, float.MinValue, float.MaxValue);
            Vector2Selection[0] = AddSlider("Vector2 X", "Vector2_X", 0, float.MinValue, float.MaxValue);
            Vector2Selection[1] = AddSlider("Vector2 Y", "Vector2_Y", 0, float.MinValue, float.MaxValue);
            Vector3Selection[0] = AddSlider("Vector3 X", "Vector3_X", 0, float.MinValue, float.MaxValue);
            Vector3Selection[1] = AddSlider("Vector3 Y", "Vector3_Y", 0, float.MinValue, float.MaxValue);
            Vector3Selection[2] = AddSlider("Vector3 Z", "Vector3_Z", 0, float.MinValue, float.MaxValue);
            QuaternionSelection[0] = AddSlider("Quaternion X", "Quaternion_X", 0, float.MinValue, float.MaxValue);
            QuaternionSelection[1] = AddSlider("Quaternion Y", "Quaternion_Y", 0, float.MinValue, float.MaxValue);
            QuaternionSelection[2] = AddSlider("Quaternion Z", "Quaternion_Z", 0, float.MinValue, float.MaxValue);
            QuaternionSelection[3] = AddSlider("Quaternion W", "Quaternion_W", 0, float.MinValue, float.MaxValue);
        }
        public override void OnBlockPlaced()
        {
            name = "Const Unit";
            InputNum = 0;
            ControlNum = 0;
            OutputNum = 1;
            InitInputPorts();
            InitOutputPorts();
            InitControlPorts();
        }

        public override void BuildingUpdate()
        {
            if (TypeChanged)
            {
                TypeChangeHandler();
            }
        }

        public override void OnUnitSimulateStart()
        {
            name = "Const Unit";
            Outputs[0].Type = (Data.DataType)Enum.Parse(typeof(Data.DataType), TypeMenu.Selection);
        }

        public override void UnitSimulateFixedUpdateHost()
        {
            if (initCount == 1)
            {
                initCount++;
                switch (TypeMenu.Value)
                {
                    case 0:
                        Outputs[0].MyData = new Data(BoolSelection.Value == 1);
                        break;
                    case 1:
                        Outputs[0].MyData = new Data(FloatSelection.Value);
                        break;
                    case 2:
                        Outputs[0].MyData = new Data(new Vector2(Vector2Selection[0].Value, Vector2Selection[1].Value));
                        break;
                    case 3:
                        Outputs[0].MyData = new Data(new Vector3(Vector3Selection[0].Value, Vector3Selection[1].Value, Vector3Selection[2].Value));
                        break;
                    case 4:
                        Outputs[0].MyData = new Data(new Quaternion(QuaternionSelection[0].Value, QuaternionSelection[1].Value, QuaternionSelection[2].Value, QuaternionSelection[3].Value));
                        break;
                    default:
                        break;
                }
            }
            else if (initCount == 0) 
            {
                initCount++;
            }
        }
    }
}
