﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modding.Modules;
using Modding;
using Modding.Blocks;
using UnityEngine;
using UnityEngine.Networking;
using Modding.Blocks;
using mattmc3.dotmore.Collections.Generic;

namespace Modern
{
    public class Connection
    {
        public Vector2 joint1;
        public Vector2 joint2;

        public Connection()
        {
            joint1 = new Vector2(-1, -1);
            joint2 = new Vector2(-1, -1);
        }

        public Connection(Vector2 p1, Vector2 p2)
        {
            joint1 = p1;
            joint2 = p2;
        }
    }

    public class BoardWire : MonoBehaviour
    {
        public GameObject parent;
        public GameObject Wire;
        public Connection connection = new Connection();
        public GameObject[] JointVis = new GameObject[2];
        public GameObject WireVis;

        public void InitWire(Transform parentBoard)
        {
            parent = parentBoard.gameObject;
            Wire = gameObject;
            JointVis[0] = new GameObject("Joint 1");
            JointVis[0].transform.parent = Wire.transform;
            JointVis[0].transform.localPosition = Vector3.zero;
            JointVis[0].transform.localRotation = Quaternion.Euler(-90, 0, 0);
            JointVis[0].transform.localScale = new Vector3(0.3f, 1, 0.3f);
            JointVis[0].AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Board Joint Mesh").Mesh;
            JointVis[0].AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Board Wire Texture").Texture;
            JointVis[0].SetActive(false);
            JointVis[1] = new GameObject("Joint 2");
            JointVis[1].transform.parent = Wire.transform;
            JointVis[1].transform.localPosition = Vector3.zero;
            JointVis[1].transform.localRotation = Quaternion.Euler(-90, 0, 0);
            JointVis[1].transform.localScale = new Vector3(0.3f, 1, 0.3f);
            JointVis[1].AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Board Joint Mesh").Mesh;
            JointVis[1].AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Board Wire Texture").Texture;
            JointVis[1].SetActive(false);

            WireVis = new GameObject("Wire");
            WireVis.transform.parent = Wire.transform;
            WireVis.transform.localPosition = Vector3.zero;
            WireVis.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            WireVis.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            WireVis.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Board Wire Mesh").Mesh;
            WireVis.AddComponent<MeshRenderer>().material.mainTexture = ModResource.GetTexture("Board Wire Texture").Texture;
            WireVis.SetActive(false);
        }

        public void SetJointPosition(Vector2 p1, Vector2 p2)
        {
            bool changed = false;
            if (p1 != connection.joint1)
            {
                if (p1.x == -1)
                {
                    JointVis[0].SetActive(false);
                }
                else
                {
                    JointVis[0].SetActive(true);
                }
                connection.joint1 = p1;
                JointVis[0].transform.localPosition = new Vector3(p1.x * 0.058f - 0.058f * 31f - 0.029f, p1.y * 0.058f - 0.058f * 31f - 0.029f, 0.09f);
                changed = true;
            }

            if (p2 != connection.joint2)
            {
                if (p2.x == -1)
                {
                    JointVis[1].SetActive(false);
                }
                else
                {
                    JointVis[1].SetActive(true);
                }
                connection.joint2 = p2;
                JointVis[1].transform.localPosition = new Vector3(p2.x * 0.058f - 0.058f * 31f - 0.029f, p2.y * 0.058f - 0.058f * 31f - 0.029f, 0.09f);
                changed |= true;
            }

            if (changed)
            {
                // update wire
                if (p2.x != -1 && p1.x != -1)
                {
                    WireVis.SetActive(true);
                    WireVis.transform.localPosition = (JointVis[0].transform.localPosition + JointVis[1].transform.localPosition) / 2f;
                    WireVis.transform.localScale = new Vector3(0.3f, 0.3f, (JointVis[0].transform.localPosition - JointVis[1].transform.localPosition).magnitude * 5f - 0.15f);
                    WireVis.transform.localRotation = Quaternion.LookRotation(JointVis[0].transform.localPosition - JointVis[1].transform.localPosition, Vector3.up);
                }
                else
                {
                    WireVis.SetActive(false);
                }
                
            }

        }


    }

    public class Board : BlockScript
    {
        public MText CircuitText;

        public bool Spotted;

        public GameObject JointGhost;
        public GameObject WireGhost;

        public BlockBehaviour bb;

        public Stack<Connection> Connections = new Stack<Connection>();

        public List<BoardWire> wires = new List<BoardWire>();

        private BoardWire currentWire;

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
            return joint;
        }

        public BoardWire CreateConnection(Vector2 p1, Vector2 p2)
        {
            Connections.Push(new Connection(p1, p2));
            CircuitText.SetValue(CircuitText.Value + ";"+ p1.ToString() + "-" + p2.ToString());
            bb.OnSave(new XDataHolder());

            GameObject WireObject = new GameObject("Circuit Wire");
            WireObject.transform.parent = transform;
            WireObject.transform.localPosition = Vector3.zero;
            WireObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            BoardWire wire = WireObject.AddComponent<BoardWire>();
            wire.InitWire(transform);
            wire.SetJointPosition(p1, p2);
            wires.Add(wire);
            return wire;
        }
        
        public void DragCurrentWireJoint(Vector2 pos)
        {
            if (currentWire)
            {
                currentWire.SetJointPosition(currentWire.connection.joint1, pos);
                Connections.Peek().joint2 = pos;
                CircuitText.Value = Tool.RemoveLastLine(CircuitText.Value);
                CircuitText.SetValue(CircuitText.Value + ";" + currentWire.connection.joint1.ToString() + "-" + pos.ToString());
                bb.OnSave(new XDataHolder());
            }
        }

        public void LoadWire()
        {
            string wireData = CircuitText.Value;

            bool hasWire = wireData.Length > 0;

            if (hasWire)
            {
                string[] wires = wireData.Split(';');
                foreach (string wire in wires)
                {
                    if (wire.Length == 0)
                    {
                        continue;
                    }
                    string[] joints = wire.Split('-');
                    Debug.Log(joints[0] + " " + joints[1]);
                    Vector2 p1 = Tool.StringToVector2(joints[0]);
                    Vector2 p2 = Tool.StringToVector2(joints[1]);
                    CreateConnection(p1, p2);
                }
            }
        }

        public override void SafeAwake()
        {
            bb = GetComponent<BlockBehaviour>();
            CircuitText = AddText("Circuit Wire", "CW", "");
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

            LoadWire();

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
                        JointGhost.transform.localPosition = new Vector3(spotCoord.x * 0.058f - 0.058f * 31f - 0.029f, spotCoord.y * 0.058f - 0.058f * 31f - 0.029f, 0.09f);
                        ShowJointGhost = true;

                        if (Input.GetMouseButtonDown(0))
                        {
                            currentWire = CreateConnection(spotCoord, spotCoord);
                            Debug.Log("Created wire at: " + spotCoord);
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            
                            DragCurrentWireJoint(spotCoord);
                        }
                        else if(Input.GetMouseButtonUp(0))
                        {
                            Debug.Log("Finish wire at: " + spotCoord);
                            currentWire = null;
                        }

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
            bb = GetComponent<BlockBehaviour>();
            gameObject.name = "Circuit Board";
            transform.Find("Adding Point").GetComponent<BoxCollider>().size = new Vector3(3.8f, 3.8f, 0.1f);
        }

    }
}