using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
	public static class CalculatorClass {
		static string operators = "^*x/%+-";

		/*
		 * Evaluate an infix mathematical expression
		*/
		public static double evaluateExpression( string sExpression ){
			return evaluatePostfixExpression (toPostfixExpression (sExpression));
		}

		/*
		 * Evaluate a postfix mathematical expression
		 */
		public static double evaluatePostfixExpression( string sPostfixExpression ) {
			double result = 0;
			Stack<double> operandStack = new Stack<double> ();

			var tokenRegex = new Regex (@"[\d\.]+|[" + Regex.Escape( operators ) + "]", RegexOptions.IgnoreCase); // Match a number or an operand
			MatchCollection tokenMatches = tokenRegex.Matches (sPostfixExpression);

			foreach (Match tokenMatch in tokenMatches) {
				string token = tokenMatch.Groups[0].Value;
				if (isNumeric (token)) {
					operandStack.Push (Double.Parse (token));
				} else if (operandStack.Count > 1) { // If its not a number then it must be an operator and if the stack count is less than 2 then something must be wrong with the expression

					// The first number popped from the stack will be the second operator because Stacks are LIFO
					var b = operandStack.Pop ();
					var a = operandStack.Pop ();

					switch (token) {
					case "^":
						result = Math.Pow (a, b);
						break;
					case "*":
					case "x":
						result = a * b;
						break;
					case "/":
						result = a / b;
						break;
					case "%":
						result = a % b;
						break;
					case "+":
						result = a + b;
						break;
					case "-":
						result = a - b;
						break;
					}

					operandStack.Push (result);
				} else {
					// We've got ourselves an invalid expression that caused our stack count to be off
					return 0;
				}
					
			}

			return operandStack.Pop ();
		}

		/*
		 * Convert an infix expression to postfix
		 */
		public static string toPostfixExpression( string sInfixEpression ) {
			string sPostfixEspression = "";
			Stack<string> expressionStack = new Stack<string> ();

			// Deal with signed terms by converting -1 to (0 - 1)
			var regSignedTerms = new Regex( @"(^|\D)([\+\-][\d+\.])" ); // Match a stranded + or - followed by a number
			sInfixEpression = regSignedTerms.Replace( sInfixEpression, new MatchEvaluator( expandSignedTerms ) );

			var tokenRegex = new Regex (@"[\d\.]+|\(|\)|[" + Regex.Escape( operators ) + "]", RegexOptions.IgnoreCase); //Match a number, parenthesis, or operand
			MatchCollection tokenMatches = tokenRegex.Matches (sInfixEpression);

			foreach (Match tokenMatch in tokenMatches) {
				string item, token = tokenMatch.Groups[0].Value;
				if (isNumeric (token)) {
					sPostfixEspression += token + " ";
				} else if (token == "(") {
					expressionStack.Push (token);
				} else if (token == ")") {
					item = expressionStack.Pop ();
					while (item != "(") {
						sPostfixEspression += item + " ";
						item = expressionStack.Pop ();
					}
				} else {
					// Handle an operator
					while( expressionStack.Count > 0 ) {
						item = expressionStack.Peek();
						if (operators.IndexOf (item) < 0) {
							// Not an operator
							break;
						}

						// We've got an operator so check precedence
						if (operators.IndexOf (item) <= operators.IndexOf (token)) {
							item = expressionStack.Pop ();
							sPostfixEspression += item + " ";
						} else {
							break;
						}
					}

					expressionStack.Push (token);
				}
			}

			while (expressionStack.Count > 0) {
				sPostfixEspression += expressionStack.Pop() + " ";
			}

			return sPostfixEspression.TrimEnd();
		}

		/*
		 * Determine if a string is a number
		 */
		public static bool isNumeric( string s ){
			return (new Regex (@"^[\d\.]+$")).IsMatch (s); 
		}

		private static string expandSignedTerms(Match m)
		{
			return m.Groups[1].Value + "(0" + m.Groups[2].Value + ")";
		}

	}
}

