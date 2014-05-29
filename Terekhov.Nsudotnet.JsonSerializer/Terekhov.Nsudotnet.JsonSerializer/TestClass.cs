using System;
using System.Reflection;

namespace JSON
{
	[Serializable]
	public class TestClass
	{
	    public int i {
			get ;
			set ;
		}
	   	public string s;
			
		[NonSerialized]
	   	public string ignore; // это поле не должно сериализоваться
	
	   	public int[] arrayMember;
	}
}


