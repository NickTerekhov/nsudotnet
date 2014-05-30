using System;

namespace Terekhov.Nsudotnet.JsonSerializer
{
	[Serializable]
	public class TestClass
	{
	    public int i {
			get ;
			set ;
		}
	   	public string Str;
	    public int[]  EmptyArray;
		[NonSerialized]
	   	public string Ignore; // это поле не должно сериализоваться
	
	   	public int[] ArrayMember;
	    public Serializator SerializatorClassTest;
	}
}


