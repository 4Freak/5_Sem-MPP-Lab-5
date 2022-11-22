using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter.Interfaces
{
	public interface IExpressionCashe
	{
		string? TryWriteCashe(string PropertyOrFieldName, object target);
		string? ReadCashe(string PropertyOrFieldName, object target);
	}
}
