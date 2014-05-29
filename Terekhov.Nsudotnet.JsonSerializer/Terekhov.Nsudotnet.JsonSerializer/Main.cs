using System;

namespace JSON
{	
	class MainClass
	{				
		public static void Main (string[] args)
		{
			TestClass testObj = new TestClass();
			testObj.i = 1;
			testObj.s = "string";
			testObj.arrayMember = new int[3] {1, 2, 3};
			
			Serializator serializator = new Serializator();
			Console.WriteLine (serializator.serial(testObj));
		}
	}
}