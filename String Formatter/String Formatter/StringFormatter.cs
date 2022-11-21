using String_Formatter.Entities;
using String_Formatter.Interfaces;
using System;
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


		public StringFormatter() 
		{
			_validator = new StringValidator();	
		}

		public string Format (string template, object target)
		{
			var idInfos = _validator.IsValid(template);

			var resultStr = Parse(idInfos, target);
			return resultStr;
		}

		private string Parse(List<IdentifierInfo> idInfos, object target)
		{
			return "";
		}
	}
}
