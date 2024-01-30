using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class StackLimiter : MonoBehaviour
    {
        public static float stackCnt = 0;

        public static float MaxStack = 200;

        public static bool Capable
        {
            get
            {
                return MaxStack > stackCnt;
            }
        }

        public void FixedUpdate()
        {
            stackCnt = 0;
        }
    }
}
