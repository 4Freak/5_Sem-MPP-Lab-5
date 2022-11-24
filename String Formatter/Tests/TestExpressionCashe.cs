using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using String_Formatter.Interfaces;
using String_Formatter.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class TestExpressionCashe : ExpressionCashe
	{
		public int ReadAttemptsCount {get; private set;}
		public int SuccessReadCount {get; private set;}
		public int WriteCount {get; private set;}

		public TestExpressionCashe() : base()
		{
			ResetCounters();
		}

		public void ResetCounters()
		{
			ReadAttemptsCount = 0;
			SuccessReadCount = 0;
			WriteCount = 0;
		}

		public override string? ReadCashe (string propertyOrFieldName, object target)
		{
			ReadAttemptsCount++;
			var result = base.ReadCashe (propertyOrFieldName, target); 
			if (result != null)
			{
				SuccessReadCount++;
			}
			return result;
		}

		public override string? TryWriteCashe (string propertyOrFieldName, object target)
		{
			WriteCount++;
			var result = base.TryWriteCashe(propertyOrFieldName, target);
			return result; 
		}
	}
}
