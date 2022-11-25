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
		public int WriteAttemptsCount {get; private set;}

		private object LockObject;

		public TestExpressionCashe() : base()
		{
			LockObject = new object();
			ResetCounters();
		}

		public void ResetCounters()
		{
			ReadAttemptsCount = 0;
			SuccessReadCount = 0;
			WriteAttemptsCount = 0;
		}

		public override string? ReadCashe (string propertyOrFieldName, object target)
		{
			lock (LockObject)
			{
				ReadAttemptsCount++;
			}
				var result = base.ReadCashe (propertyOrFieldName, target); 
				if (result != null)
				{
					lock (LockObject)
					{
						SuccessReadCount++;
					}	
				}
				return result;
		}

		public override string? TryWriteCashe (string propertyOrFieldName, object target)
		{
			lock (LockObject)
			{
				WriteAttemptsCount++;
			}
			var result = base.TryWriteCashe(propertyOrFieldName, target);
			return result; 
		}
	}
}
