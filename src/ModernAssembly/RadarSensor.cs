using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    public class RadarSensor : Sensor
    {
        public override string GetName()
        {
            return "Radar Sensor";
        }
        public override Data SensorGenerate()
        {
            M_Package pkg = new M_Package(new Data(transform.right), new Data(transform.up), new Data(transform.forward), new Data());
            return new Data(pkg);
        }
    }
}
