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
        public DataType Type = DataType.Null;
        public string Str;
        public float Flt;
        public bool Bool;
        public Vector2 Vec2;
        public Vector3 Vec3;
        public Quaternion Quat;
        public M_Icon Icon;
        public M_Package Package;

        public Data(string str)
        {
            Type = DataType.String;
            this.Str = str;
        }
        public Data(float flt)
        {
            Type = DataType.Float;
            this.Flt = flt;
        }
        public Data(bool b)
        {
            Type = DataType.Bool;
            this.Bool = b;
        }
        public Data(Vector2 vec2)
        {
            Type = DataType.Vector2;
            this.Vec2 = vec2;
        }
        public Data(Vector3 vec3)
        {
            Type = DataType.Vector3;
            this.Vec3 = vec3;
        }
        public Data(Quaternion quat)
        {
            Type = DataType.Quaternion;
            this.Quat = quat;
        }
        public Data(M_Icon icon)
        {
            Type = DataType.Icon;
            this.Icon = icon;
        }
        public Data(M_Package package)
        {
            Type = DataType.Package;
            this.Package = package;
        }
        public Data()
        {
            Type = DataType.Null;
        }

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
