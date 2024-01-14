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
            Any // only for port
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
        public Data(Data data)
        {
            Type = data.Type;
            Str = data.Str;
            Flt = data.Flt;
            Bool = data.Bool;
            Vec2 = data.Vec2;
            Vec3 = data.Vec3;
            Quat = data.Quat;
            Icon = data.Icon;
            Package = data.Package;
        }

        public bool Equal(Data d)
        {
            if (Type != d.Type) return false;
            switch (Type)
            {
                case DataType.Null:
                    return true;
                case DataType.String:
                    return Str == d.Str;
                case DataType.Float:
                    return Flt == d.Flt;
                case DataType.Bool:
                    return Bool == d.Bool;
                case DataType.Vector2:
                    return Vec2 == d.Vec2;
                case DataType.Vector3:
                    return Vec3 == d.Vec3;
                case DataType.Quaternion:
                    return Quat == d.Quat;
                case DataType.Icon:
                    return Icon == d.Icon;
                case DataType.Package:
                    return Package == d.Package;
                default:
                    return false;
            }
        }

        public Data()
        {
            Type = DataType.Null;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case DataType.Null:
                    return "<null>";
                case DataType.String:
                    return "<str>\n      "+Str;
                case DataType.Float:
                    return "<float>\n      " + Flt.ToString();
                case DataType.Bool:
                    return "<bool>\n      " + Bool.ToString();
                case DataType.Vector2:
                    return "<vec2>\n      " + Vec2.ToString();
                case DataType.Vector3:
                    return "<vec3>\n      " + Vec3.ToString();
                case DataType.Quaternion:
                    return "<quat>\n      " + Quat.ToString();
                case DataType.Icon:
                    return "<icon>\n      ";
                case DataType.Package:
                    return "<package>\n      ";
                default:
                    return "<null>\n      ";
            }
        }

        public object GetValue()
        {
            switch (Type)
            {
                case DataType.Null:
                    return null;
                case DataType.String:
                    return Str;
                case DataType.Float:
                    return Flt;
                case DataType.Bool:
                    return Bool;
                case DataType.Vector2:
                    return Vec2;
                case DataType.Vector3:
                    return Vec3;
                case DataType.Quaternion:
                    return Quat;
                case DataType.Icon:
                    return Icon;
                case DataType.Package:
                    return Package;
                default:
                    return null;
            }
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
