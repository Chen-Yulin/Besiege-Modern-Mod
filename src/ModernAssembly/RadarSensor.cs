﻿using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class ScanCollisonHit : MonoBehaviour
    {
        public MPTeam myTeam = MPTeam.None;
        public bool IFF = false;
        public Beam beam;

        void Start()
        {
        }
        void Update()
        {
        }

        void OnTriggerEnter(Collider col)
        {
            try
            {
                
                
                if (col.isTrigger)
                {
                    return;
                }
                BlockBehaviour hitedBlock = col.attachedRigidbody.gameObject.GetComponent<BlockBehaviour>();
                
                if (!hitedBlock.isSimulating)
                {
                    return;
                }
                
                if (IFF)
                {
                    MPTeam hitedTeam;
                    hitedTeam = hitedBlock.Team;
                    //Debug.Log(hitedTeam.ToString()+" "+myTeam.ToString());
                    if (myTeam == hitedTeam)
                    {
                        return;
                    }
                }
                if (beam.scannedColliders.ContainsKey(hitedBlock))
                {
                    beam.scannedColliders[hitedBlock]++;
                }
                else
                {
                    beam.scannedColliders.Add(hitedBlock, 1);
                    //Debug.Log(hitedBlock.name + "Enter");
                }
            }
            catch
            {
                return;
            }

        }

        void OnTriggerExit(Collider col)
        {
            try
            {

                if (col.isTrigger)
                {
                    return;
                }
                BlockBehaviour hitedBlock = col.attachedRigidbody.gameObject.GetComponent<BlockBehaviour>();
                if (beam.scannedColliders.ContainsKey(hitedBlock))
                {
                    beam.scannedColliders[hitedBlock]--;
                    if (beam.scannedColliders[hitedBlock] == 0)
                    {
                        beam.scannedColliders.Remove(hitedBlock);
                        //Debug.Log(hitedBlock.name + "Exit");
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }

    public class Beam : MonoBehaviour
    {
        public List<BoxCollider> BeamColliders;

        public Dictionary<BlockBehaviour, int> scannedColliders = new Dictionary<BlockBehaviour, int>();

        public void InitBeam(RadarSensor radar, bool vis = true)
        {
            BeamColliders = new List<BoxCollider>();
            float posDist = 3;
            float sideLen = 1;
            for (int i = 0; i < 6; i++)
            {
                GameObject beam = new GameObject("Beam Vis " + i.ToString());
                beam.transform.parent = transform;
                beam.transform.localPosition = new Vector3(0, 0, posDist);
                beam.transform.localRotation = Quaternion.identity;
                beam.transform.localScale = sideLen * Vector3.one;
                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                collider.size = sideLen * Vector3.one;
                collider.center = new Vector3(0, 0, posDist);
                collider.isTrigger = true;
                
                BeamColliders.Add(collider);
                if (vis)
                {
                    MeshFilter mf = beam.AddComponent<MeshFilter>();
                    MeshRenderer mr = beam.AddComponent<MeshRenderer>();
                    mf.sharedMesh = ModResource.GetMesh("Cube Mesh").Mesh;
                    mr.material = new Material(Shader.Find("Particles/Alpha Blended"));
                    Color displayColor = Color.green;
                    displayColor.a = 0.05f;
                    mr.material.SetColor("_TintColor", displayColor);
                }

                posDist += (sideLen / 2) * (1 + 3f / 2f);
                sideLen *= 3f / 2f;
            }
            ScanCollisonHit sch = gameObject.AddComponent<ScanCollisonHit>();
            sch.beam = this;
        }

    }
    public class RadarSensor : Sensor
    {
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
            beam.InitBeam(this);


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
