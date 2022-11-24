using System.ComponentModel.DataAnnotations;
using String_Formatter;
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
			var stringFormatter = new StringFormatter();
			var obj = new Object();
			var ids = stringFormatter.Format("string", obj);
			ids = stringFormatter.Format("{id}", obj);
			ids = stringFormatter.Format("SomeText {id} some text", obj);
			ids = stringFormatter.Format("SomeText {id} some text {_id2}", obj);
			ids = stringFormatter.Format("{{CorrectId}}", obj);
			ids = stringFormatter.Format("{{{CorrectId}}}", obj);
			ids = stringFormatter.Format("Some text {{0InCorrectId}}", obj);
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