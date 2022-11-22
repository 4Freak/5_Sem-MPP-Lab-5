using String_Formatter.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter.Services
{
	public class ExpressionCashe : IExpressionCashe
	{
		private readonly ConcurrentDictionary<string, Func<object, string>> _cashe;

		public ExpressionCashe()
		{
			_cashe = new ConcurrentDictionary<string, Func<object, string>>();
		}

		public string? ReadCashe (string PropertyOrFieldName, object target)
		{
			var keyStr = $"{target.GetType()}.{PropertyOrFieldName}";
			Func<object, string>? func;
			if (_cashe.TryGetValue(keyStr, out func))
			{
				return func(target);
			}
			return null;
		}

		public string? TryWriteCashe (string PropertyOrFieldName, object target)
		{
			var targetProperties = target.GetType().GetProperties();
			var targetFields = target.GetType().GetFields();

			if (targetProperties.Where(prop => prop.Name == PropertyOrFieldName).ToList().Count != 0 ||
				targetFields.Where(fld => fld.Name == PropertyOrFieldName).ToList().Count != 0)
			{
				var objParam = Expression.Parameter(typeof(object), "obj");
				var propOrFldValue = Expression.PropertyOrField(Expression.TypeAs(objParam, target.GetType()), PropertyOrFieldName);
				var propOrFldValueToStr = Expression.Call(propOrFldValue, "ToString", null, null);
				var func = Expression.Lambda<Func<object, string>>(propOrFldValueToStr, objParam).Compile();
				return func(target);
			}

			return null; 
		}
	}
}
