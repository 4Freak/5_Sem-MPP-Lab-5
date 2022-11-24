using System.ComponentModel.DataAnnotations;
using String_Formatter;
using String_Formatter.Entities;
using String_Formatter.Interfaces;
using String_Formatter.Services;

namespace Tests
{
	public class StringFormatterTests
	{
		public TestExpressionCashe Cashe;
		public StringFormatter StringFormatter;

		[SetUp]
		public void Setup ()
		{
			Cashe = new TestExpressionCashe ();
			StringFormatter = new StringFormatter(Cashe);
		}

		[Test]
		public void stringFormatterTest ()
		{


			var obj = new ParserData(Cashe, "You can read this", Cashe);
			
			var ids = StringFormatter.Format("string", obj);
			ids = StringFormatter.Format("{Template}", obj);
			ids = StringFormatter.Format("SomeText {Template} some text", obj);
			ids = StringFormatter.Format("SomeText {Template} some text {Template}", obj);
			ids = StringFormatter.Format("{{Template}}", obj);
			ids = StringFormatter.Format("{{{Template}}}", obj);
			ids = StringFormatter.Format("Some text {{0Template}}", obj);
			int catchCount = 0, tryCounts = 0;
			try
			{
				tryCounts++;
				ids = StringFormatter.Format("}{", obj);
			}
			catch {catchCount++; };
			try
			{
				tryCounts++;
				ids = StringFormatter.Format("{{", obj);
			}
			catch {catchCount++; }
			try
			{
				tryCounts++;
				ids = StringFormatter.Format("}", obj);
			}
			catch {catchCount++; }
			try
			{
				tryCounts++;
				ids = StringFormatter.Format("{0Id}", obj);
			}
			catch {catchCount++; }
			Assert.That(catchCount, Is.EqualTo(tryCounts));
			Assert.That(Cashe.ReadAttemptsCount, Is.EqualTo(5));
			Assert.That(Cashe.SuccessReadCount, Is.EqualTo(Cashe.ReadAttemptsCount-1));
			Assert.That(Cashe.WriteCount, Is.EqualTo(1));
			Assert.Pass();
		}
	}
}