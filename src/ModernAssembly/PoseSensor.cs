﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    class PoseSensor : Sensor
    {
        public MSlider Range;
        public override string GetName()
        {
            return "Pose Sensor";
        }

        public override void SensorSafeAwake()
        {
        }
        public override void SensorBuildUpdate()
        {
            base.SensorBuildUpdate();
        }
        public override Data SensorGenerate()
        {
            M_Package pkg = new M_Package(new Data(transform.right), new Data(transform.up), new Data(transform.forward), new Data());
            return new Data(pkg);
        }
    }
}
