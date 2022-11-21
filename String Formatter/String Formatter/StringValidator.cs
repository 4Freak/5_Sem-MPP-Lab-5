using String_Formatter.Entities;
using String_Formatter.Interfaces;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter
{
	public class StringValidator : IStringValidator
	{
		private string Letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private string Number = "0123456789";
		private char Underscore = '_';
		private char StartBracket = '{';
		private char EndBracket = '}';
		private enum ValidatorState
		{
			Text,
			StartBracket,
			EndBracket,
			Identifier
		}
	
		public List<IdentifierInfo> IsValid (string template)
		{
			var idInfos = new List<IdentifierInfo>();

			int startBracketCount = 0, endBracketCount = 0;
			StringBuilder sbName = new StringBuilder ();
			IdentifierInfo idInfo= new IdentifierInfo(0);
			var validatorState = ValidatorState.Text;
			for (int i = 0; i < template.Length; i++)
			{
				switch (validatorState)
				{
					case ValidatorState.Text:
					{
						if (template[i] == StartBracket)
						{
							validatorState = ValidatorState.StartBracket;
							startBracketCount++;
							idInfo = new IdentifierInfo(i);
						}
						else if (template[i] == EndBracket)
						{
							validatorState = ValidatorState.EndBracket;
							endBracketCount++;
						}
						else
						{
							validatorState = ValidatorState.Text;
						}
						break;
					}

					case ValidatorState.StartBracket:
					{
						if (startBracketCount <= endBracketCount)
						{
							throw new ArgumentException ("Incorrect brackets order. Too mach start brackets");
						}
						else if (Letter.Contains(char.ToUpper(template[i])) ||
								template[i] == Underscore)
						{
							validatorState = ValidatorState.Identifier;
							sbName.Clear();
							sbName.Append(template[i]);
						}
						else if (template[i] == StartBracket)
						{
							validatorState = ValidatorState.StartBracket;
							startBracketCount++;
							idInfo.Position = i;
						}
						else
						{
							throw new ArgumentException ("Incorrect identifier");
						}
						break;
					}

					case ValidatorState.EndBracket:
					{
						if (endBracketCount > startBracketCount)
						{
							throw new ArgumentException ("Incorrect brackets order. Too mach end brackets");
						}
						if (template[i] == StartBracket)
						{
							validatorState = ValidatorState.StartBracket;
							startBracketCount++;
							idInfo = new IdentifierInfo(i);
						}
						else if (template[i] == EndBracket)
						{
							validatorState = ValidatorState.EndBracket;
							endBracketCount++;
						}
						else
						{
							validatorState = ValidatorState.Text;
						}
						break;
					}

					case ValidatorState.Identifier:
					{
						if (template[i] == EndBracket)
						{
							validatorState = ValidatorState.EndBracket;
							endBracketCount++;
							idInfo.Name = sbName.ToString();
							idInfos.Add(idInfo);
						}
						else if (Letter.Contains(char.ToUpper(template[i])) ||
								Number.Contains(template[i]) || 
								template[i] == Underscore)
						{
							validatorState = ValidatorState.Identifier;
							sbName.Append (template[i]);
						}
						else
						{
							throw new ArgumentException("Incorrect identifier");
						}
						break;
					}
				}
			}

			if (startBracketCount != endBracketCount)
			{
				throw new ArgumentException("Start and End bracksts count mismatch");
			}

			return idInfos;
		}


	}
}
