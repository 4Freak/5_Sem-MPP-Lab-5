using String_Formatter.Entities;
using String_Formatter.Interfaces;
using String_Formatter.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter
{
    public class StringFormatter : IStringFormatter
    {
		public static readonly StringFormatter Shared = new StringFormatter();
		
		private readonly StringValidator _validator;
		private readonly ExpressionCashe _cashe;


		public StringFormatter() 
		{
			_validator = new StringValidator();	
			_cashe = new ExpressionCashe();
		}

		public string Format (string template, object target)
		{
			List<IdentifierInfo> idInfos = _validator.IsValid(template);
			if (idInfos.Count == 0)
			{
				return template;
			}

			var resultStr = Parse(template, idInfos, target);
			return resultStr;
		}

		private string Parse(string template, List<IdentifierInfo> idInfos, object target)
		{
			var result = new StringBuilder(template);
			
			return result.ToString();
		}
	}
}
