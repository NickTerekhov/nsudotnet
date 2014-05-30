using System;
using System.Text;

namespace Terekhov.Nsudotnet.JsonSerializer
{	
	class MainClass
	{				
		public static void Main (string[] args)
		{
			TestClass testObj = new TestClass();
			testObj.i = 1;
			testObj.Str = "string";
			testObj.ArrayMember = new int[3] {1, 2, 3};
            StringBuilder stringToPrint = new StringBuilder();			
			Serializator serializator = new Serializator();
            serializator.PiuPiuString = "piu-piu";

            Console.WriteLine(serializator.Serialize(testObj));

		}
	}
}