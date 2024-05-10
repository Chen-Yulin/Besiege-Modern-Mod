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
    class Converter:Unit
    {
        List<string> TypeString = new List<string> {
              "Bool",
              "Float",
              "Vector2",
              "Vector3",
              "Quaternion",
        };

        public MMenu InputType;
        public MMenu OutputType;

        public bool mapperChanged = false;

        public void MapperChangeHandler()
        {
            mapperChanged = false;
            switch (InputType.Value)
            {
                case 0:
                    switch (OutputType.Value)
                    {
                        case 0:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 1:
                            InputNum = 1;
                            OutputNum = 1;
                            break;
                        case 2:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 3:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 4:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        default:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (OutputType.Value)
                    {
                        case 0:
                            InputNum = 1;
                            OutputNum = 1;
                            break;
                        case 1:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 2:
                            InputNum = 2;
                            OutputNum = 1;
                            break;
                        case 3:
                            InputNum = 3;
                            OutputNum = 1;
                            break;
                        case 4:
                            InputNum = 4;
                            OutputNum = 1;
                            break;
                        default:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                    }
                    break;
                case 2:
                    switch (OutputType.Value)
                    {
                        case 0:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 1:
                            InputNum = 1;
                            OutputNum = 2;
                            break;
                        case 2:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 3:
                            InputNum = 1;
                            OutputNum = 1;
                            break;
                        case 4:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        default:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                    }
                    break;
                case 3:
                    switch (OutputType.Value)
                    {
                        case 0:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 1:
                            InputNum = 1;
                            OutputNum = 3;
                            break;
                        case 2:
                            InputNum = 1;
                            OutputNum = 1;
                            break;
                        case 3:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 4:
                            InputNum = 1;
                            OutputNum = 1;
                            break;
                        default:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                    }
                    break;
                case 4:
                    switch (OutputType.Value)
                    {
                        case 0:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 1:
                            InputNum = 1;
                            OutputNum = 4;
                            break;
                        case 2:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        case 3:
                            InputNum = 1;
                            OutputNum = 1;
                            break;
                        case 4:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                        default:
                            InputNum = 0;
                            OutputNum = 0;
                            break;
                    }
                    break;
                default:
                    break;
            }
            UpdateInputPort();
            UpdateOutputPort();
        }
        public void UpdatePortType()
        {
            foreach (var port in Outputs)
            {
                port.Type = Data.DataType.Any;
            }
            var type = Data.DataType.Null;
            switch (InputType.Value)
            {
                case 0:
                    type = Data.DataType.Bool;
                    break;
                case 1:
                    type = Data.DataType.Float;
                    break;
                case 2:
                    type = Data.DataType.Vector2;
                    break;
                case 3:
                    type = Data.DataType.Vector3;
                    break;
                case 4:
                    type = Data.DataType.Quaternion;
                    break;
                default : type = Data.DataType.Null;break;
            }
            foreach (var port in Inputs)
            {
                port.Type = type;
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
        public void UpdateOutputPort()
        {
            // first clear
            int j = 0;
            foreach (var port in Outputs)
            {
                Destroy(port.gameObject);
                j++;
            }
            Outputs.Clear();
            // then init again
            InitOutputPorts();
        }
        public override void SafeAwake()
        {
            Tool.SetOccluder(transform, new Vector3(0.7f, 0.7f, 1));
            InputType = AddMenu("Input", 0, TypeString);
            OutputType = AddMenu("Output", 0, TypeString);

            InputType.ValueChanged += (int value) =>
            {
                mapperChanged = true;
            };
            OutputType.ValueChanged += (int value) =>
            {
                mapperChanged = true;
            };

        }
        public override void OnBlockPlaced()
        {
            name = "Converter Unit";
        }

        public override void BuildingUpdate()
        {
            if (mapperChanged)
            {
                MapperChangeHandler();
            }
        }

        public override void OnUnitSimulateStart()
        {
            name = "Converter Unit";
            UpdatePortType();
        }
        public override void UpdateUnit(Port Caller)
        {
            if (!CheckInputs())
            {
                Outputs[0].MyData = new Data();
                return;
            }
            switch (InputType.Value)
            {
                case 0:
                    switch (OutputType.Value)
                    {
                        case 1:
                            Outputs[0].MyData = new Data(Caller.MyData.Bool ? 1 : 0);
                            break;
                        default:
                            break;
                    }
                    break;
                case 1:
                    switch (OutputType.Value)
                    {
                        case 0:
                            Outputs[0].MyData = new Data(Caller.MyData.Flt == 0 ? false : true);
                            break;
                        case 2:
                            Outputs[0].MyData = new Data(new Vector2(Inputs[0].MyData.Flt, Inputs[1].MyData.Flt));
                            break;
                        case 3:
                            Outputs[0].MyData = new Data(new Vector3(Inputs[0].MyData.Flt, Inputs[1].MyData.Flt, Inputs[2].MyData.Flt));
                            break;
                        case 4:
                            Outputs[0].MyData = new Data(new Quaternion(Inputs[0].MyData.Flt, Inputs[1].MyData.Flt, Inputs[2].MyData.Flt, Inputs[2].MyData.Flt));
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    switch (OutputType.Value)
                    {
                        case 1:
                            Outputs[0].MyData = new Data(Caller.MyData.Vec2.x);
                            Outputs[1].MyData = new Data(Caller.MyData.Vec2.y);
                            break;
                        case 3:
                            Outputs[0].MyData = new Data(new Vector3(Caller.MyData.Vec2.x, Caller.MyData.Vec2.y, 0));
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    switch (OutputType.Value)
                    {
                        case 1:
                            Outputs[0].MyData = new Data(Caller.MyData.Vec3.x);
                            Outputs[1].MyData = new Data(Caller.MyData.Vec3.y);
                            Outputs[2].MyData = new Data(Caller.MyData.Vec3.z);
                            break;
                        case 2:
                            Outputs[0].MyData = new Data(new Vector2(Caller.MyData.Vec2.x, Caller.MyData.Vec2.y));
                            break;
                        case 4:
                            Outputs[0].MyData = new Data(Quaternion.Euler(Inputs[0].MyData.Vec3));
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (OutputType.Value)
                    {
                        case 1:
                            Outputs[0].MyData = new Data(Caller.MyData.Quat.w);
                            Outputs[1].MyData = new Data(Caller.MyData.Quat.x);
                            Outputs[2].MyData = new Data(Caller.MyData.Quat.y);
                            Outputs[3].MyData = new Data(Caller.MyData.Quat.z);
                            break;
                        case 3:
                            Outputs[0].MyData = new Data(Caller.MyData.Quat.eulerAngles);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
