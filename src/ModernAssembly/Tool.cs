﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Tool
    {
        public static RaycastHit[] RaycastAllSorted(Ray ray, float dist)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, dist);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }
    }
}
