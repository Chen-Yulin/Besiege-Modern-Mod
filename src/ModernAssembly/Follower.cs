using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Follower : MonoBehaviour
    {
        public Transform target;
        public Vector3 PosOffset;
        public Quaternion RotOffset;

        public void InitFollower(Transform t, Vector3 pos, Quaternion q)
        {
            target = t;
            PosOffset = pos;
            RotOffset = q;
        }
        public void LateUpdate()
        {
            transform.position = target.position + target.right * PosOffset.x + target.up * PosOffset.y + target.forward * PosOffset.z;
            transform.rotation = target.rotation * RotOffset;
        }
    }
}
