using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Switch : Sensor
    {
        public MKey SwitchKey;
        public MToggle DefaultOn;
        public MToggle AutoReturn;

        public bool useEmulate = false;

        public Transform Vis;

        private bool on = false;

        public bool On{
            get
            {
                return on;
            }
            set
            {
                if (on != value)
                {
                    on = value;
                    if (!Vis)
                    {
                        Vis = transform.Find("Vis");
                    }
                    if (Vis)
                    {
                        Vector3 scale = Vis.localScale;
                        scale.z = Mathf.Abs(scale.z) * (On?-1:1);
                        Vis.localScale = scale;
                        Indicator.SetActive(on);
                    }
                }
            }
        }

        public GameObject Indicator;

        public void InitIndicator()
        {
            Indicator = new GameObject("Indicator");
            Indicator.transform.parent = this.transform;
            Indicator.transform.localPosition = new Vector3(0, 0, 0.19f);
            Indicator.transform.localRotation = Quaternion.identity;
            Indicator.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Indicator.AddComponent<MeshFilter>().sharedMesh = ModResource.GetMesh("Sphere Mesh").Mesh;
            MeshRenderer mr = Indicator.AddComponent<MeshRenderer>();
            mr.material = new Material(Shader.Find("Particles/Alpha Blended"));
            Color displayColor = Color.green;
            displayColor.a = 0.5f;
            mr.material.SetColor("_TintColor", displayColor);
            Indicator.SetActive(false);
        }

        public override string GetName()
        {
            return "Switch";
        }

        public override void SensorSafeAwake()
        {
            SwitchKey = AddKey("Switch Key", "Switch Key", KeyCode.T);
            DefaultOn = AddToggle("Default On", "Default On", false);
            AutoReturn = AddToggle("Auto Reset", "Auto Reset", false);
        }

        public override void SensorSimulateStart()
        {
            InitIndicator();
            On = !DefaultOn.isDefaultValue;
        }

        public override Data SensorGenerate()
        {
            return new Data(On);
        }

        public override void SensorSimulateFixedUpdate()
        {
            if (AutoReturn.isDefaultValue)
            {
                if (SwitchKey.EmulationPressed())
                {
                    On = !On;
                }
            }
            else
            {
                if (SwitchKey.EmulationHeld())
                {
                    On = AutoReturn.isDefaultValue;
                    useEmulate = true;
                }
                else if (useEmulate)
                {
                    On = !AutoReturn.isDefaultValue;
                }
            }
            

        }
        public override void SensorSimulateUpdate()
        {
            if (AutoReturn.isDefaultValue)
            {
                if (SwitchKey.IsPressed)
                {
                    On = !On;
                }
            }
            else
            {
                if (SwitchKey.IsHeld)
                {
                    On = AutoReturn.isDefaultValue;
                    useEmulate = false;
                }
                else if (!useEmulate)
                {
                    On = !AutoReturn.isDefaultValue;
                }
            }
        }

    }
}
