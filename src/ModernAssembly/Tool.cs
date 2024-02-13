using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Modern
{
    public class Tool
    {
        public static void SetOccluder(Transform t, Vector3 size)
        {
            try
            {
                t.Find("Occluder").GetComponent<BoxCollider>().size = size;
            }
            catch { }
        }
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
        public static RaycastHit[] SphereCastSorted(Vector3 pos, float radius)
        {
            RaycastHit[] hits = Physics.SphereCastAll(pos, radius, Vector3.forward);
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
            //Debug.Log(input);
            string[] values = input.Split(','); // 使用逗号分割字符串
            //Debug.Log("Try Parsing " + values[0]);
            //Debug.Log("Try Parsing " + values[1]);
            float x = float.Parse(values[0]); // 解析x值
            float y = float.Parse(values[1]); // 解析y值

            return new Vector2(x, y); // 返回Vector2实例
        }
        public static Vector2 GetBoardCoordinate(Vector3 point, Transform board_t)
        {
            Vector3 localPoint = board_t.InverseTransformPoint(point);
            Vector2 joint = new Vector2(localPoint.x, localPoint.y);
            joint.x += 0.058f * 31f + 0.029f;
            joint.y += 0.058f * 31f + 0.029f;
            joint.x = Mathf.Round(joint.x / 0.058f);
            joint.x = Mathf.Clamp(joint.x, 0, 63);
            joint.y = Mathf.Round(joint.y / 0.058f);
            joint.y = Mathf.Clamp(joint.y, 0, 63);
            return joint;
        }

        public static Vector3 BesselCurve(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;
            Vector3 p = uuu * p1 + 3 * uu * t * p2 + 3 * u * tt * p3 + ttt * p4;
            return p;
        }
    }

    
}
