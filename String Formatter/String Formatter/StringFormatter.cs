using String_Formatter.Entities;
using String_Formatter.Interfaces;
using String_Formatter.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter
{
    public class StringFormatter : IStringFormatter
    {
		public static readonly StringFormatter Shared = new StringFormatter();
		
		private readonly ExpressionCashe _cashe;


		public StringFormatter(ExpressionCashe cashe) 
		{	
			_cashe = cashe;
		}

		public StringFormatter() 
		{
			_cashe = new ExpressionCashe();
		}

		public string Format (string template, object target)
		{
			var resultStr = Parse(template, target);
			return resultStr;
		}

		private string Letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private string Number = "0123456789";
		private char Underscore = '_';
		private char StartBracket = '{';
		private char EndBracket = '}';

		private delegate bool StateMatrixDelegate (char item);
		Func<char, ParserData, Boolean>[,] StateMatrix = new Func<char, ParserData, Boolean>[5, 5]
		{
			{ ErrorState,	ErrorState,	ErrorState,	ErrorState,	ErrorState }, 
			{ ErrorState,	State11,	State12,	ErrorState,	State14 }, 
			{ ErrorState,	State21,	ErrorState,	State23,	ErrorState }, 
			{ ErrorState,	State31,	ErrorState,	State33,	ErrorState }, 
			{ ErrorState,	State41,	ErrorState,	ErrorState, ErrorState } 
		};

		private enum ValidatorState
		{
			Error = 0,
			Text = 1,
			StartBracket = 2,
			Identifier = 3,
			EndBracket = 4
		}
		public string Parse(string template, object target)
		{
			var parserData = new ParserData(_cashe, template, target);
			var validatorState = ValidatorState.Text;
			var nextState = ValidatorState.Text;
			for (int i = 0; i < template.Length; i++)
			{
				switch(validatorState)
				{
					case ValidatorState.Text:
					{
						if (template[i] == StartBracket)
							nextState = ValidatorState.StartBracket;
						else if (template[i] ==EndBracket)
							nextState = ValidatorState.EndBracket;
						else
							nextState = ValidatorState.Text;
						
						StateMatrix[(int)validatorState, (int)nextState](template[i], parserData);
						validatorState = nextState;
						break;
					}
					case ValidatorState.StartBracket:
					{	
						if (template[i] == StartBracket)
							nextState = ValidatorState.Text;
						else if (Letter.Contains(char.ToUpper(template[i])) || 
								template[i] == Underscore)
							nextState = ValidatorState.Identifier;
						else
							throw new ArgumentException($"Incorrect identifier {template[i]} at {i}");
						
						StateMatrix[(int)validatorState, (int)nextState](template[i], parserData);
						validatorState = nextState;
						break;
					}
					case ValidatorState.Identifier:
					{
						if (Letter.Contains(char.ToUpper(template[i])) ||
							Number.Contains(char.ToUpper(template[i])) ||
							template[i] == Underscore)
							nextState = ValidatorState.Identifier;
						else if (template[i] == EndBracket)
							nextState = ValidatorState.Text;
						else
							throw new ArgumentException($"Incorrect identifier {template[i]} at {i}");

						StateMatrix[(int)validatorState, (int)nextState](template[i], parserData);
						validatorState = nextState;
						break;
					}
					case ValidatorState.EndBracket:
					{
						if (template[i] == EndBracket)
							nextState= ValidatorState.Text;
						else
							throw new ArgumentException($"Incorrect text {template[i]} at {i}");

						StateMatrix[(int)validatorState, (int)nextState](template[i], parserData);
						validatorState = nextState;
						break;
					}

				}
			}
			if (parserData.OpenBracketsCount != parserData.CloseBracketsCount)
			{
				throw new ArgumentException("Incorrect amounts of brackets");
			}
			return parserData.Result.ToString();
		}

		private static Boolean ErrorState(char item, ParserData parserData)
		{
			throw new Exception("Automaton's Error State");
		}

	
		private static Boolean State11(char item, ParserData parserData)
		{
			parserData.Result.Append(item);
			return true;
		}

		private static Boolean State12(char item, ParserData parserData) 
		{
			parserData.OpenBracketsCount++;
			parserData.Result.Append(item);
			return true; 
		}

		private static Boolean State14(char item, ParserData parserData) 
		{
			parserData.Result.Append(item);
			parserData.CloseBracketsCount++;
			if (parserData.OpenBracketsCount < parserData.CloseBracketsCount)
			{
				throw new ArgumentException("Incorrect amounts of brackets");
			}
			return true; 
		}

		private static Boolean State21(char item, ParserData parserData) 
		{
			parserData.OpenBracketsCount++;
			return true; 
		}

		private static Boolean State23(char item, ParserData parserData) 
		{
			parserData.Result.Remove(parserData.Result.Length-1, 1);
			parserData.IdentifierName.Clear();
			parserData.IdentifierName.Append(item);
			return true; 
		}

		private static Boolean State31(char item, ParserData parserData) 
		{
			// TODO: change to cahse
			//parserData.Result.Append(parserData.IdentifierName + "4");

			var resultStr = parserData.Cashe.ReadCashe(parserData.IdentifierName.ToString(), parserData.Target);
			if (resultStr == null)
			{
				try
				{
					resultStr = parserData.Cashe.TryWriteCashe(parserData.IdentifierName.ToString(), parserData.Target);
				}
				catch(Exception ex){ }
			}
			parserData.Result.Append(resultStr);
			parserData.CloseBracketsCount++;
			return true; 
		}

		private static Boolean State33(char item, ParserData parserData) 
		{
			parserData.IdentifierName.Append(item);
			return true; 
		}

		private static Boolean State41(char item, ParserData parserData) 
		{
			parserData.CloseBracketsCount++;
			if (parserData.OpenBracketsCount < parserData.CloseBracketsCount)
			{
				throw new ArgumentException("Incorrect amounts of brackets");
			}
			return true; 
		}	

	}
}
