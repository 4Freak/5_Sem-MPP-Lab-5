using String_Formatter.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter.Interfaces
{
	public interface IStringValidator
	{
		List<IdentifierInfo> IsValid(string value);
	}
}
