using Modding;
using Modding.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Wire : BlockScript
    {
        MSlider[] tailPose = new MSlider[6];

        public GameObject Tail;
        public GameObject Head;
        BlockBehaviour bb;

        public Vector3 TailPosition
        {
            get
            {
                return Tail.transform.position;
            }
            set
            {
                Tail.transform.position = value;
            }
        }

        public Quaternion TailRotation
        {
            get
            {
                return Tail.transform.rotation;
            }
            set
            {
                Tail.transform.rotation = value;
            }
        }

        public Vector3 preTailPosition;
        public Quaternion preTailRotation;

        public bool creatingWire = false;

        public GameObject WireBase;
        public GameObject[] WireObject = new GameObject[5];
        public GameObject[] JointObject = new GameObject[4];

        public InputPin DistPin = null;
        public OutputPin SrcPin = null;

        public Transform buildTailTarget = null;


        public void UpdateWireCurve()
        {
            Transform vis = Head.transform;
            Vector3 p0 = vis.position + vis.up * 0.25f;
            Vector3 p1 = vis.position + vis.up * 0.75f;
            Vector3 p2 = Tail.transform.position + Tail.transform.up * 0.75f;
            Vector3 p3 = Tail.transform.position + Tail.transform.up * 0.25f;
            JointObject[0].transform.position = Tool.BesselCurve(p0, p1, p2, p3, 0.15f);
            JointObject[1].transform.position = Tool.BesselCurve(p0, p1, p2, p3, 0.35f);
            JointObject[2].transform.position = Tool.BesselCurve(p0, p1, p2, p3, 0.65f);
            JointObject[3].transform.position = Tool.BesselCurve(p0, p1, p2, p3, 0.85f);

            WireObject[0].transform.position = (vis.position + vis.up * 0.25f + JointObject[0].transform.position) / 2;
            WireObject[1].transform.position = (JointObject[0].transform.position + JointObject[1].transform.position) / 2;
            WireObject[2].transform.position = (JointObject[1].transform.position + JointObject[2].transform.position) / 2;
            WireObject[3].transform.position = (JointObject[2].transform.position + JointObject[3].transform.position) / 2;
            WireObject[4].transform.position = (JointObject[3].transform.position + Tail.transform.position + Tail.transform.up * 0.25f) / 2;
            WireObject[0].transform.LookAt(JointObject[0].transform);
            WireObject[1].transform.LookAt(JointObject[1].transform);
            WireObject[2].transform.LookAt(JointObject[2].transform);
            WireObject[3].transform.LookAt(JointObject[3].transform);
            WireObject[4].transform.LookAt(Tail.transform.position + Tail.transform.up * 0.25f);
            WireObject[0].transform.localScale = new Vector3(1f, 1f, Vector3.Distance(vis.position + vis.up * 0.25f, JointObject[0].transform.position));
            WireObject[1].transform.localScale = new Vector3(1f, 1f, Vector3.Distance(JointObject[0].transform.position, JointObject[1].transform.position));
            WireObject[2].transform.localScale = new Vector3(1f, 1f, Vector3.Distance(JointObject[1].transform.position, JointObject[2].transform.position));
            WireObject[3].transform.localScale = new Vector3(1f, 1f, Vector3.Distance(JointObject[2].transform.position, JointObject[3].transform.position));
            WireObject[4].transform.localScale = new Vector3(1f, 1f, Vector3.Distance(JointObject[3].transform.position, Tail.transform.position + Tail.transform.up * 0.25f));
        }
        public bool UpdateWireMapper()
        {
            if (Vector3.Distance(preTailPosition, TailPosition) > 0.001f || Quaternion.Angle(preTailRotation, TailRotation) > 0.1f)
            {
                preTailRotation = TailRotation;
                preTailPosition = TailPosition;
                SaveWireToMapper();
                return true;
            }
            else
            {
                return false;
            }
        }
        public Vector3 GetVirtualTailPos()
        {
            Vector3 normal = transform.position - Camera.main.transform.position;
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = normal.magnitude;
            return Camera.main.ScreenToWorldPoint(screenPoint);
        }
        public void SaveWireToMapper()
        {
            tailPose[0].SetValue(TailPosition.x);
            bb.OnSave(new XDataHolder());
            tailPose[1].SetValue(TailPosition.y);
            bb.OnSave(new XDataHolder());
            tailPose[2].SetValue(TailPosition.z);
            bb.OnSave(new XDataHolder());
            tailPose[3].SetValue(TailRotation.eulerAngles.x);
            bb.OnSave(new XDataHolder());
            tailPose[4].SetValue(TailRotation.eulerAngles.y);
            bb.OnSave(new XDataHolder());
            tailPose[5].SetValue(TailRotation.eulerAngles.z);
            bb.OnSave(new XDataHolder());
        }
        public void LoadWireFromMapper()
        {
            Vector3 tailpos = new Vector3(tailPose[0].Value, tailPose[1].Value, tailPose[2].Value);
            Quaternion tailrot = Quaternion.Euler(tailPose[3].Value, tailPose[4].Value, tailPose[5].Value);
            TailPosition = tailpos;
            TailRotation = tailrot;
        }
        public void InitWire()
        {
            if (!transform.Find("Wire"))
            {
                WireBase = new GameObject("Wire");
                WireBase.transform.SetParent(transform);

                for (int i = 0; i < 4; i++)
                {
                    JointObject[i] = new GameObject("Joint");
                    JointObject[i].transform.SetParent(WireBase.transform);
                    JointObject[i].transform.localPosition = Vector3.zero;
                    JointObject[i].transform.localRotation = Quaternion.identity;
                    JointObject[i].transform.localScale = Vector3.one;
                    MeshFilter mf = JointObject[i].AddComponent<MeshFilter>();
                    mf.sharedMesh = ModResource.GetMesh("Wire Joint Mesh").Mesh;
                    MeshRenderer mr = JointObject[i].AddComponent<MeshRenderer>();
                    mr.material.mainTexture = ModResource.GetTexture("Wire Texture").Texture;
                }
                for (int i = 0; i < 5; i++)
                {
                    WireObject[i] = new GameObject("Joint");
                    WireObject[i].transform.SetParent(WireBase.transform);
                    WireObject[i].transform.localPosition = Vector3.zero;
                    WireObject[i].transform.localRotation = Quaternion.identity;
                    WireObject[i].transform.localScale = Vector3.one;
                    MeshFilter mf = WireObject[i].AddComponent<MeshFilter>();
                    mf.sharedMesh = ModResource.GetMesh("Wire Mesh").Mesh;
                    MeshRenderer mr = WireObject[i].AddComponent<MeshRenderer>();
                    mr.material.mainTexture = ModResource.GetTexture("Wire Texture").Texture;
                }
            }
        }
        public void InitTail()
        {
            if (!transform.Find("Vis").Find("Tail"))
            {
                Tail = new GameObject("Tail");
                Tail.transform.SetParent(transform.Find("Vis"));
                Tail.transform.localPosition = Vector3.zero;
                Tail.transform.localRotation = Quaternion.identity;
                Tail.transform.localScale = Vector3.one;
                MeshFilter mf = Tail.AddComponent<MeshFilter>();
                mf.sharedMesh = ModResource.GetMesh("Wire Tail Mesh").Mesh;
                MeshRenderer mr = Tail.AddComponent<MeshRenderer>();
                mr.material.mainTexture = ModResource.GetTexture("Wire Tail Texture").Texture;
            }
            else
            {
                Tail = transform.Find("Vis").Find("Tail").gameObject;
            }
            
        }
        public override void SafeAwake()
        {
            
            bb = GetComponent<BlockBehaviour>();
            Tool.SetOccluder(transform, new Vector3(0.06f, 0.06f, 1));
            tailPose[0] = AddSlider("tail pose 0", "tail pose 0", 0, float.MinValue, float.MaxValue);
            tailPose[1] = AddSlider("tail pose 1", "tail pose 1", 0, float.MinValue, float.MaxValue);
            tailPose[2] = AddSlider("tail pose 2", "tail pose 2", 0, float.MinValue, float.MaxValue);
            tailPose[3] = AddSlider("tail pose 3", "tail pose 3", 0, float.MinValue, float.MaxValue);
            tailPose[4] = AddSlider("tail pose 4", "tail pose 4", 0, float.MinValue, float.MaxValue);
            tailPose[5] = AddSlider("tail pose 5", "tail pose 5", 0, float.MinValue, float.MaxValue);
        }

        public override void OnBlockPlaced()
        {
            Head = transform.Find("Vis").gameObject;
            name = "Wire";
            InitTail();
            InitWire();
            if (tailPose[0].Value == 0 && tailPose[1].Value == 0 && tailPose[2].Value == 0 &&
                tailPose[3].Value == 0 && tailPose[4].Value == 0 && tailPose[5].Value == 0)
            {
                creatingWire = true;
            }
            else
            {
                creatingWire = false;
                LoadWireFromMapper();
            }
        }

        public override void BuildingUpdate()
        {
            if (creatingWire)
            {
                if (Input.GetMouseButton(0))
                {
                    UpdateWireCurve();
                    bool findPin = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Tool.RaycastAllSorted(ray, 20f);
                    if (hits.Length > 0)
                    {
                        foreach (var hit in hits)
                        {
                            if (!hit.collider.isTrigger)
                            {
                                continue;
                            }
                            else if (hit.collider.name == "Adding Point" && hit.collider.transform.parent.name == "Output Pin")
                            {
                                TailPosition = hit.collider.transform.position + hit.collider.transform.forward * 0.5f;
                                TailRotation = hit.collider.transform.parent.Find("Vis").rotation;
                                findPin = true;
                                break;
                            }
                        }
                    }
                    if (!findPin)
                    {
                        TailPosition = GetVirtualTailPos();
                        TailRotation = Quaternion.identity;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    SaveWireToMapper();
                    creatingWire = false;
                }
            }
            else
            {
                if (UpdateWireMapper())
                {
                    UpdateWireCurve();
                }
            }
        }

        public override void OnSimulateStart()
        {
            name = "Wire";
            bb = GetComponent<BlockBehaviour>();
            Head = transform.Find("Vis").gameObject;
            InitTail();

            // destroy the physics
            Destroy(GetComponent<ConfigurableJoint>());
            Destroy(GetComponentInChildren<BoxCollider>());
            GetComponent<Rigidbody>().useGravity = false;

            //find the output pin connected to tail
            RaycastHit[] hits = Tool.SphereCastSorted(TailPosition, 0.02f);
            foreach (var hit in hits)
            {
                if (hit.collider.name != "Adding Point")
                {
                    continue;
                }
                try
                {
                    SrcPin = hit.collider.transform.parent.GetComponent<OutputPin>();
                    if (SrcPin)
                    {
                        Tail.transform.SetParent(SrcPin.transform);
                        //Debug.Log("Output found");
                        break;
                    }
                }
                catch { }
            }

            hits = Tool.SphereCastSorted(transform.position, 0.02f);
            foreach (var hit in hits)
            {
                if (hit.collider.name != "Adding Point")
                {
                    continue;
                }
                try
                {
                    DistPin = hit.collider.transform.parent.GetComponent<InputPin>();
                    if (DistPin)
                    {
                        Head.transform.SetParent(DistPin.transform);
                        SrcPin.DstPins.Add(DistPin);
                        DistPin.SrcPin = SrcPin;
                        break;
                    }
                }
                catch { }
            }

        }

        public override void SimulateLateUpdateAlways()
        {
            UpdateWireCurve();
        }

        public override void SimulateFixedUpdateHost()
        {
        }

    }
}
