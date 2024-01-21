using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    public class GPS : Sensor
    {
        public override string GetName()
        {
            return "GPS";
        }
        public override Data SensorGenerate()
        {
            return new Data(transform.position);
        }
    }
}
