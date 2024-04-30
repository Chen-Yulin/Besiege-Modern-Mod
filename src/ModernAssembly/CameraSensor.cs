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
        private Texture2D RenderTextureToTexture2D(RenderTexture texture)
        {
            RenderTexture RT = RenderTexture.active;
            RenderTexture.active = texture;
            Texture2D texture2D = new Texture2D(texture.width, texture.height);
            texture2D.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);
            return texture2D;
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
            cam = gameObject.AddComponent<Camera>();
            rt = new RenderTexture((int)Width.Value, (int)Height.Value, 16, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            Fov = FOVSlider.Value;
            if (onboard)
            {
                Inputs[0].Type = Data.DataType.Float;
            }
        }
        public override Data SensorGenerate()
        {
            return new Data(RenderTextureToTexture2D(rt));

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
