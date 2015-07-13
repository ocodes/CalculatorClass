using System;

namespace Calculator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string sExpression  = "8*5+9/3"; // (((((((1+2.5)*(0-4))*2)/9)*9)/5)-2)+(8+(7/(8*5)))
			Console.WriteLine ( CalculatorClass.evaluateExpression( sExpression ));

			sExpression = "(8*5)+(9/3)";
			Console.WriteLine ( CalculatorClass.evaluateExpression( sExpression ));
		}
		 
	}
}
