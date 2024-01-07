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
        public static string RemoveLastLine(string str)
        {
            int index = str.LastIndexOf(";");
            if (index >= 0)
            {
                return str.Substring(0, index);
            }
            else
            {
                return str;
            }
        }
        public static Vector2 StringToVector2(string input)
        {
            input = input.Trim('(', ')', ' '); // 移除括号空格
            Debug.Log(input);
            string[] values = input.Split(','); // 使用逗号分割字符串
            Debug.Log("Try Parsing " + values[0]);
            Debug.Log("Try Parsing " + values[1]);
            float x = float.Parse(values[0]); // 解析x值
            float y = float.Parse(values[1]); // 解析y值

            return new Vector2(x, y); // 返回Vector2实例
        }
    }

    
}