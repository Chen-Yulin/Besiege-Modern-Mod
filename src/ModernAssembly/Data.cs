using mattmc3.dotmore.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modding.Modules;
using Modding;
using Modding.Blocks;
using UnityEngine;
using UnityEngine.Networking;
using Modding.Blocks;

namespace Modern
{
    public class Data
    {
        public enum DataType
        {
            Null,
            String,
            Float,
            Bool,
            Vector2,
            Vector3,
            Quaternion,
            Icon,
            Package,
            Any
        }
        public string Str;
        public float Flt;
        public bool Bool;
        public Vector2 Vec2;
        public Vector3 Vec3;
        public Quaternion Quat;
        public M_Icon Icon;
        public M_Package Package;
    }

    public class M_Icon
    {
        enum IconType
        {
            SolidBox,
            SolidCircle,
            HollowBox,
            HollowCircle,
            Line,
            Characters
        }
    }

    public class M_Package
    {
        public List<Data> DataArr;
        public void AddData(Data data)
        {
            DataArr.Append(data);
        }
    }
}
