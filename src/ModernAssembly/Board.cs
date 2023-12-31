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

        public bool ShowGhost;

        public GameObject JointGhost;
        public GameObject WireGhost;

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

        public override void BuildingUpdate()
        {
            
        }

    }
}
