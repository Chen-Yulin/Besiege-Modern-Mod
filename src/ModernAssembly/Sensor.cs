using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modern
{
    class Sensor : Unit
    {
        public MToggle OnBoard;

        public override void SafeAwake()
        {
            OnBoard = AddToggle("On Board", "OnBoard", "On Board", false);
            SensorSafeAwake();
        }

        public virtual void SensorSafeAwake()
        {

        }
    }
}
