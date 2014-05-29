using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSON
{
	public class Serializator
	{
		public StringBuilder serial(Object obj)
		{
			bool flag = false;
			
			StringBuilder sb = new StringBuilder();
			Attribute[] atr = Attribute.GetCustomAttributes(obj.GetType());
			foreach(Attribute at in atr){
				if(at.GetType()!=typeof(System.SerializableAttribute))
					throw new System.ArgumentException("Class is not serializable"); 
			}
			
			sb.Append("{\n");
			
			foreach (FieldInfo mi in obj.GetType().GetFields()) {
				
				flag = Attribute.GetCustomAttributes(mi).Any(attr=>attr.GetType()==typeof(System.NonSerializedAttribute));
				
				if(flag==true)
						continue;
				
				if (mi.FieldType.IsArray) {
					sb.Append("\t" + "\"" + mi.Name + "\"" + " : " + "[");
					Array arr = mi.GetValue(obj) as Array;
					
					for (int i = 0; i < arr.Length-1; i++)
                        sb.Append(arr.GetValue(i).ToString()+ ", ");
					sb.Append(arr.GetValue(arr.Length-1));
					sb.Append("]");
				}
    			else 
					sb.Append("\t" + "\"" + mi.Name + "\"" + " : " + "\"" + mi.GetValue(obj) + "\"" + ",\n");
			}
			
			sb.Append("\n}\n");
			
			return sb;
		}
	}
}

