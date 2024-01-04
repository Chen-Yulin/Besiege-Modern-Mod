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
    public class Connection
    {
        public Vector2 joint1;
        public Vector2 joint2;

        public Connection(Vector2 p1, Vector2 p2)
        {
            joint1 = p1;
            joint2 = p2;
        }
    }

    public class Board : BlockScript
    {
        public MText CircuitText;

        public bool Spotted;

        public GameObject JointGhost;
        public GameObject WireGhost;

        private bool _showJointGhost = false;

        public bool ShowJointGhost
        {
            get
            {
                return _showJointGhost;
            }
            set
            {
                if (_showJointGhost != value)
                {
                    _showJointGhost = value;
                    JointGhost.SetActive(value);
                }
            }
        }

        public Vector2 GetJointCoordinate(Vector3 point)
        {
            Vector3 localPoint = transform.InverseTransformPoint(point);
            Vector2 joint = new Vector2(localPoint.x, localPoint.y);
            joint.x += 0.058f * 31f + 0.029f;
            joint.y += 0.058f * 31f + 0.029f;
            joint.x = Mathf.Round(joint.x/0.058f);
            joint.x = Mathf.Clamp(joint.x, 0, 63);
            joint.y = Mathf.Round(joint.y/0.058f);
            joint.y = Mathf.Clamp(joint.y, 0, 63);
            Debug.Log(joint);
            return joint;
        }

        public override void OnBlockPlaced()
        {
            if (!transform.Find("Circuit Joint Ghost"))
            {
                JointGhost = new GameObject("Circuit Joint Ghost");
                JointGhost.transform.parent = transform;
                JointGhost.transform.localPosition = Vector3.zero;
                JointGhost.transform.localRotation = Quaternion.Euler(-90,0,0);
                JointGhost.transform.localScale = new Vector3(0.3f, 1, 0.3f);
                JointGhost.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Board Joint Mesh").Mesh;
                JointGhost.AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Board Wire Texture").Texture;
                JointGhost.SetActive(false);
            }
            else
            {
                JointGhost = transform.Find("Circuit Joint Ghost").gameObject;
            }
        }

        public override void BuildingUpdate()
        {
            if (Spotted)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Tool.RaycastAllSorted(ray, 20f);
                if (hits.Length > 0)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.collider.isTrigger)
                        {
                            continue;
                        }
                        Vector2 spotCoord = GetJointCoordinate(hit.point);
                        JointGhost.transform.localPosition = new Vector3(spotCoord.x * 0.058f - 0.058f * 31f - 0.029f, spotCoord.y * 0.058f - 0.058f * 31f - 0.029f, 0.1f);
                        ShowJointGhost = true;
                        break;
                    }
                }
                Spotted = false;
            }
            else
            {
                ShowJointGhost = false;
            }
        }

        public void Start()
        {
            gameObject.name = "Circuit Board";
            transform.Find("Adding Point").GetComponent<BoxCollider>().size = new Vector3(3.8f, 3.8f, 0.1f);
        }

    }
}
