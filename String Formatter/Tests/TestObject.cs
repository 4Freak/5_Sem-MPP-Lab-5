using String_Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class TestObject
	{
	    public string Field1 { get; }
		public string Field2 { get; }
    
		public TestObject(string field1, string field2)
		{
			Field1 = field1;
			Field2 = field2;
		}

		public string GetFileds()
		{
			return StringFormatter.Shared.Format(
				"Field1: {Field1}, Filed2: {Field2}", this);
		}

		public string GetFiledsReverse()
		{
			return StringFormatter.Shared.Format(
				"Field2: {Field2}, Filed1: {Field1}", this);
		}

	}
}
