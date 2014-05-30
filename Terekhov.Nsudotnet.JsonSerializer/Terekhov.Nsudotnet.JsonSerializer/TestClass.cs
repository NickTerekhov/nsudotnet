using System;
using System.Reflection;

namespace Terekhov.Nsudotnet.JsonSerializer
{
	[Serializable]
	public class TestClass
	{
	    public int i {
			get ;
			set ;
		}
	   	public string s;
	    public int[]  emptyArray;
		[NonSerialized]
	   	public string ignore; // это поле не должно сериализоваться
	
	   	public int[] arrayMember;
	    public Serializator serializatorClassTest;
	}
}


