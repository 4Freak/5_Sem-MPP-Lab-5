using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using String_Formatter.Services;

namespace String_Formatter.Entities
{
    public class ParserData
    {
        public readonly ExpressionCashe Cashe;
        public readonly string Template;
        public readonly object Target;
        public StringBuilder Result = new StringBuilder();
        public StringBuilder IdentifierName = new StringBuilder();
        public int OpenBracketsCount;
        public int CloseBracketsCount;

		public ParserData(ExpressionCashe cashe, string template, object target)
		{
			Cashe = cashe;
			Template = template;
			Target = target;
			Result = new StringBuilder();
			IdentifierName = new StringBuilder();
			OpenBracketsCount = 0;
			CloseBracketsCount = 0;
		}
	}
}
