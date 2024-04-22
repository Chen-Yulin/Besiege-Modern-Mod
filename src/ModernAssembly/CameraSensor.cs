using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class CameraSensor:Sensor
    {
        public MSlider FOV;
        public Beam beam;

        public MSlider Power;

        public float power = 1;

        public void AdjustBeam(float dist)
        {
            beam.transform.localScale = new Vector3(1 / transform.lossyScale.x / Mathf.Sqrt(dist), 1 / transform.lossyScale.y / Mathf.Sqrt(dist), dist / transform.lossyScale.z) * power;
        }
        public override string GetName()
        {
            return "Radar Sensor";
        }
        public override bool needPara()
        {
            return true;
        }
        public override void SensorSafeAwake()
        {
            Power = AddSlider("Power", "Power", 10, 1, 1000);
            Power.ValueChanged += (float value) =>
            {
                power = value;
            };
        }


        public override void SensorSimulateStart()
        {
            GameObject BeamObject = new GameObject("Beam");
            BeamObject.transform.parent = transform;
            BeamObject.transform.localPosition = Vector3.zero;
            BeamObject.transform.localRotation = Quaternion.identity;
            BeamObject.transform.localScale = new Vector3(power / transform.lossyScale.x, power / transform.lossyScale.y, power / transform.lossyScale.z);
            beam = BeamObject.AddComponent<Beam>();


            if (onboard)
            {
                Inputs[0].Type = Data.DataType.Float;
            }
        }
        public override Data SensorGenerate()
        {
            List<BlockBehaviour> targetList = beam.scannedColliders.Keys.ToList();
            List<BlockBehaviour> sampleList = targetList.Where((x, i) => i % 5 == 0).ToList();
            if (sampleList.Count > 0)
            {
                try
                {
                    M_Package pkg = new M_Package(new Data(true), new Data(sampleList[0].transform.position), new Data(sampleList[0].Rigidbody.velocity), new Data());
                    return new Data(pkg);
                }
                catch
                {
                    M_Package pkg = new M_Package(new Data(false), new Data(), new Data(), new Data());
                    return new Data(pkg);
                }

            }
            else
            {
                M_Package pkg = new M_Package(new Data(false), new Data(), new Data(), new Data());
                return new Data(pkg);
            }

        }

        public override void SensorUpdatePara()
        {
            float dist = power;
            if (Inputs[0].MyData.Type != Data.DataType.Null)
            {
                dist = Inputs[0].MyData.Flt;
            }

            AdjustBeam(dist);
        }
        public override void WirelessSensorUpdatePara(Data data)
        {
            float dist = power;
            if (data.Type != Data.DataType.Null)
            {
                dist = data.Flt;
            }

            AdjustBeam(dist);
        }
    }
}
