using String_Formatter.Entities;
using String_Formatter.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter
{
	internal class StringValidator : IStringValidator
	{
		private enum ValidatorState
		{
		}
	
		public List<IdentifierInfo> IsValid (string value)
		{
			var idInfos = new List<IdentifierInfo>();
			
			
			return idInfos;
		}


	}
}
