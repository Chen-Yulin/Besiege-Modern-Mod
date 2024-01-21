using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class IMU : Sensor
    {
        public Rigidbody body;
        public Vector3 pre_vel = Vector3.zero;
        public Vector3 accel = Vector3.zero;
        public override string GetName()
        {
            return "IMU";
        }
        public override void SensorSimulateStart()
        {
            body = GetComponent<Rigidbody>();
            if (!body)
            {
                body = GetComponentInParent<Rigidbody>();
            }
        }
        public override void SensorSimulateFixedUpdate()
        {
            if (body)
            {
                accel = body.velocity - pre_vel;
                pre_vel = body.velocity;
            }
        }
        public override Data SensorGenerate()
        {
            if (body)
            {
                M_Package pkg = new M_Package(new Data(body.velocity), new Data(accel), new Data(body.angularVelocity), new Data());
                return new Data(pkg);
            }
            else
            {
                return new Data();
            }
            
        }
    }
}
