using System;

namespace Terekhov.Nsudotnet.JsonSerializer
{
	[Serializable]
	public class TestClass
	{
	    public int i = 1000;
	   	public string Str = "Hello!";
	    public int[]  EmptyArray;
		[NonSerialized]
	   	public string Ignore; // это поле не должно сериализоваться

        public int[] ArrayMember = new int[3] { 1, 2, 3 };
	    public Serializator SerializatorClassTest;
	}
}


