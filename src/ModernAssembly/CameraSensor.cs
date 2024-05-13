using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class CameraSensor:Sensor
    {
        public MSlider FOVSlider;

        public Camera cam;

        public MSlider Width;
        public MSlider Height;

        public RenderTexture rt;

        public Texture2D tex;

        private float fov;
        public float Fov
        {
            get { return fov; }
            set 
            { 
                fov = value;
                if (cam)
                {
                    cam.fieldOfView = value;
                }
            }
        }
        private void RenderTextureToTexture2D(RenderTexture texture, Texture2D dst)
        {
            Graphics.CopyTexture(texture, dst);
        }
        public override string GetName()
        {
            return "Camera Sensor";
        }
        public override bool needPara()
        {
            return true;
        }
        public override void SensorSafeAwake()
        {
            Width = AddSlider("Horizontal resolution", "Width", 1920, 320, 2560);
            Height = AddSlider("Vertical resolution", "Height", 1080, 180, 1440);
            FOVSlider = AddSlider("Default FOV", "FOV", 60, 10, 120);
        }


        public override void SensorSimulateStart()
        {
            GameObject camObject = new GameObject("Camera");
            camObject.transform.SetParent(transform);
            camObject.transform.localPosition = new Vector3(0, 0, 0.3f);
            camObject.transform.localRotation = Quaternion.identity;
            cam = camObject.AddComponent<Camera>();
            rt = new RenderTexture((int)Width.Value, (int)Height.Value, 16, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            cam.farClipPlane = 8000f;
            Fov = FOVSlider.Value;
            if (onboard)
            {
                Inputs[0].Type = Data.DataType.Float;
            }
            tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        }
        public override Data SensorGenerate()
        {
            RenderTextureToTexture2D(rt, tex);
            return new Data(tex);
        }

        public override void SensorUpdatePara()
        {
            if (Inputs[0].MyData.Type != Data.DataType.Null)
            {
                Fov = Inputs[0].MyData.Flt;
            }
        }
        public override void WirelessSensorUpdatePara(Data data)
        {
            if (data.Type != Data.DataType.Null)
            {
                Fov = data.Flt;
            }
        }
    }
}
