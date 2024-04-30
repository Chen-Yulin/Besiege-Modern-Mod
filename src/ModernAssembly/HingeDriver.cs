using Modding.Modules.Official;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Modern
{
    public class HingeDriver : Driver
    {
        public SteeringWheel sw;

        public override void DriverOnSimulateStart()
        {
            sw = GetComponent<SteeringWheel>();
        }

        public override void DriverGetData(Data data)
        {
            if (sw)
            {
                if (data.Type == Data.DataType.Float)
                {
                    sw.AngleToBe = data.Flt;
                }
                else
                {
                    sw.AngleToBe = 0;
                }
            }
            
        }
    }
}
