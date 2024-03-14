using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Driver : MonoBehaviour
    {
        public BlockBehaviour bb;

        public void Start()
        {
            bb = GetComponent<BlockBehaviour>();
            if (bb.ParentMachine.isSimulating)
            {
                DriverOnSimulateStart();
            }
            else
            {

            }
        }

        public virtual void DriverOnSimulateStart()
        {

        }

        public virtual void DriverGetData(Data data)
        {
            return;
        }
    }
}
