using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Terekhov.Nsudotnet.JsonSerializer
{
    [Serializable]
    public class Serializator
    {
        public string PiuPiuString;

        public StringBuilder Serialize(Object objectToSerialize)
        {
            StringBuilder builder = new StringBuilder();
            Serial(objectToSerialize, builder);
            return builder;
        }

        public void Serial(Object obj, StringBuilder stringBuilder)
        {
            bool flag = false;
            StringBuilder sb = stringBuilder;
            if (null == obj)
            {
                sb.Append("null");
                return;
            }

            Attribute[] atr = Attribute.GetCustomAttributes(obj.GetType());
            foreach (Attribute at in atr)
            {
                if (at.GetType() != typeof (System.SerializableAttribute))
                    throw new System.ArgumentException("Class is not serializable");
            }

            sb.Append("{\n");

            foreach (FieldInfo mi in obj.GetType().GetFields())
            {
                flag =
                    Attribute.GetCustomAttributes(mi)
                        .Any(attr => attr is NonSerializedAttribute);

                if (flag == true)
                    continue;

                sb.Append("\t").Append("\"").Append(mi.Name).Append("\"").Append(" : ");
                if (mi.FieldType.IsArray)
                {
                    Array arr = mi.GetValue(obj) as Array;
                    if (null == arr)
                    {
                        sb.Append("null").Append("\n");
                        continue;
                    }
                    sb.Append("[");

                    if (0 != arr.Length)
                    {
                        for (int i = 0; i < arr.Length - 1; i++)
                        {
                            sb.Append(arr.GetValue(i)).Append(", ");
                        }
                        sb.Append(arr.GetValue(arr.Length - 1));
                    }
                    sb.Append("]");
                } else if (mi.FieldType.IsPrimitive)
                {
                    if (mi.FieldType == typeof (char))
                    {
                        sb.Append(mi.GetValue(obj)).Append("\"").Append("\n");
                    } else if (mi.FieldType == typeof (bool))
                    {
                        sb.Append(mi.GetValue(obj).ToString().ToLower());
                    } else 
                    {
                        sb.Append(mi.GetValue(obj));
                    }
                } else if (mi.FieldType == typeof (string))
                {
                    sb.Append("\"").Append(mi.GetValue(obj)).Append("\"");
                }
                else
                {
                    this.Serial(mi.GetValue(obj), sb);
                }
                sb.Append("\n");
            }

            sb.Append("}\n");
            return;
        }
    }
}