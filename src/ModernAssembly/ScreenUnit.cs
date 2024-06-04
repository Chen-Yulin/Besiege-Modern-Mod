using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    class ScreenUnit : Executer
    {
        public MSlider Ratio;

        public GameObject screen;
        public MeshFilter ScreenMF;
        public MeshRenderer ScreenMR;

        public void initScreen()
        {
            if (!transform.FindChild("Screen"))
            {
                screen = new GameObject("Screen");
                screen.transform.SetParent(transform);
                screen.transform.localPosition = new Vector3(0, 0, 0.4f);
                screen.transform.localRotation = Quaternion.Euler(90, 0, 0);
                screen.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                ScreenMF = screen.AddComponent<MeshFilter>();
                ScreenMR = screen.AddComponent<MeshRenderer>();
                ScreenMF.mesh = ModResource.GetMesh("Plane Mesh").Mesh;
                //ScreenMR.material.shader = Shader.Find("Particles/Alpha Blended");
                //ScreenMR.material.shader = AssetManager.Instance.Shader.GrayShader;
                //ScreenMR.sortingOrder = 50;
            }

        }

        public override string GetName()
        {
            return "Screen";
        }
        public override void ExecuterSafeAwake()
        {
            Ratio = AddSlider("Ratio", "ratio", 1.6f, 0.4f, 2f);
        }
        public override void ExecuterSimulateStart()
        {
            if (onboard)
            {
                Inputs[0].Type = Data.DataType.Image;
            }
            initScreen();
        }
        public override void ExecuterUpdateOuput(Port Caller)
        {
            Data data = Caller.MyData;
            ExecuterWirelessReceiveData(data);
        }

        public override void ExecuterWirelessReceiveData(Data data)
        {
            if (data.Type == Data.DataType.Image)
            {
                ScreenMR.material.mainTexture = data.Img;
            }
        }
    }
}
