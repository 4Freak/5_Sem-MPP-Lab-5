using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter.Interfaces
{
	public interface IStringFormatter
	{
		string Format(string template, object target);
		string Parse(string template, object target);
	}
}
