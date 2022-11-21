using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatter.Entities
{
	public class IdentifierInfo
	{
		public string Name { get; set; }
		public int Position { get; set; }

		public IdentifierInfo (string name, int position)
		{
			Name = name;
			Position = position;
		}

		public IdentifierInfo (int position)
		{ 
			Name = "";
			Position = position; 
		}
	}
}
