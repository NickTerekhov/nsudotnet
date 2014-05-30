using System;
using System.Text;

namespace Terekhov.Nsudotnet.JsonSerializer
{	
	class MainClass
	{				
		public static void Main (string[] args)
		{
			TestClass testObj = new TestClass();

            StringBuilder stringToPrint = new StringBuilder();			
			Serializator serializator = new Serializator();
            //serializator.PiuPiuString = "piu-piu";

            Console.WriteLine(serializator.Serialize(testObj));
            Console.WriteLine(typeof(int).IsPrimitive);

		}
	}
}