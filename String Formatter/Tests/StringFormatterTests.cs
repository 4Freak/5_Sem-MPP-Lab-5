using System.ComponentModel.DataAnnotations;
using String_Formatter;
using String_Formatter.Entities;
using String_Formatter.Interfaces;
using String_Formatter.Services;

namespace Tests
{
	public class StringFormatterTests
	{
		[SetUp]
		public void Setup ()
		{
			
		}

		[Test]
		public void stringFormatterTest ()
		{
			var cashe = new ExpressionCashe();
			var stringFormatter = new StringFormatter(cashe);

			var obj = new ParserData(cashe, "You can read this", cashe);
			
			var ids = stringFormatter.Format("string", obj);
			ids = stringFormatter.Format("{Template}", obj);
			ids = stringFormatter.Format("SomeText {Template} some text", obj);
			ids = stringFormatter.Format("SomeText {Template} some text {Template}", obj);
			ids = stringFormatter.Format("{{Template}}", obj);
			ids = stringFormatter.Format("{{{Template}}}", obj);
			ids = stringFormatter.Format("Some text {{0Template}}", obj);
			int catchCount = 0, tryCounts = 0;
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("}{", obj);
			}
			catch {catchCount++; };
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("{{", obj);
			}
			catch {catchCount++; }
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("}", obj);
			}
			catch {catchCount++; }
			Assert.That(catchCount, Is.EqualTo(tryCounts));
			Assert.Pass();
		}
	}
}