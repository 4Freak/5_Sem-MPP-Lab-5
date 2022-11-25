using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NuGet.Frameworks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
		public void CorrectTemplateTests ()
		{

			var cashe = new TestExpressionCashe ();
			var stringFormatter = new StringFormatter(cashe);
			var parserData = new ParserData(cashe, "You can read this", cashe);
			
			var ids = stringFormatter.Format("string", parserData);
			ids = stringFormatter.Format("{Template}", parserData);
			Assert.That(ids, Is.EquivalentTo($"{parserData.Template}"));

			ids = stringFormatter.Format("SomeText {Template} some text", parserData);
			Assert.That(ids, Is.EqualTo($"SomeText {parserData.Template} some text"));
			
			ids = stringFormatter.Format("SomeText {Template} some text {Template}", parserData);
			Assert.That(ids, Is.EqualTo($"SomeText {parserData.Template} some text {parserData.Template}"));

			parserData.OpenBracketsCount = 4;
			ids = stringFormatter.Format("SomteTest {OpenBracketsCount}", parserData);
			Assert.That(ids, Is.EqualTo($"SomteTest {parserData.OpenBracketsCount}"));

			Assert.That(cashe.ReadAttemptsCount, Is.EqualTo(5));
			Assert.That(cashe.WriteAttemptsCount, Is.EqualTo(2));
			Assert.That(cashe.SuccessReadCount, Is.EqualTo(cashe.ReadAttemptsCount-cashe.WriteAttemptsCount));

			Assert.Pass();
		}

		[Test]
		public void IncorrectTest()
		{
			var cashe = new TestExpressionCashe ();
			var stringFormatter = new StringFormatter(cashe);
			var parserData = new ParserData(cashe, "You can read this", cashe);

			string ids;
			int catchCount = 0, tryCounts = 0;
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("}{", parserData);
			}
			catch {catchCount++; };
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("{{", parserData);
			}
			catch {catchCount++; }
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("}", parserData);
			}
			catch {catchCount++; }
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("{0Id}", parserData);
			}
			catch {catchCount++; }
			try
			{
				tryCounts++;
				ids = stringFormatter.Format("{}", parserData);
			}

			catch {catchCount++; }

			Assert.That(catchCount, Is.EqualTo(tryCounts));
			Assert.That(cashe.ReadAttemptsCount, Is.EqualTo(0));
			Assert.That(cashe.WriteAttemptsCount, Is.EqualTo(0));
			Assert.That(cashe.SuccessReadCount, Is.EqualTo(cashe.ReadAttemptsCount-cashe.SuccessReadCount));

			Assert.Pass();
		}

		[Test]
		public void EscapeTest()
		{
			var cashe = new TestExpressionCashe ();
			var stringFormatter = new StringFormatter(cashe);
			var parserData = new ParserData(cashe, "You can read this", cashe);

			string ids = stringFormatter.Format("{{Template}}", parserData);
			Assert.That(ids, Is.EqualTo("{Template}"));

			ids = stringFormatter.Format("{{0Template}}", parserData);
			Assert.That(ids, Is.EqualTo("{0Template}"));

			ids = stringFormatter.Format("{{{Template}}}", parserData);
			Assert.That(ids, Is.EqualTo($"{{{parserData.Template}}}"));

			ids = stringFormatter.Format("{{{{Template}}}}", parserData);
			Assert.That(ids, Is.EqualTo("{{Template}}"));

			Assert.That(cashe.ReadAttemptsCount, Is.EqualTo(1));
			Assert.That(cashe.WriteAttemptsCount, Is.EqualTo(1));
			Assert.That(cashe.SuccessReadCount, Is.EqualTo(cashe.ReadAttemptsCount - cashe.WriteAttemptsCount));

			Assert.Pass();
		}

		[Test]
		public void SharedCallTest()
		{
			var testObject = new TestObject("MyField1", "MyField2");

			var ids = testObject.GetFileds();
			Assert.That(ids, Is.EqualTo($"Field1: {testObject.Field1}, Filed2: {testObject.Field2}"));

			ids = testObject.GetFiledsReverse();
			Assert.That(ids, Is.EqualTo($"Field2: {testObject.Field2}, Filed1: {testObject.Field1}"));

			Assert.Pass();
		}

		[Test]
		public void ThreadSafetyTest()
		{
			var cashe = new TestExpressionCashe ();
			var stringFormatter = new StringFormatter(cashe);
			var random = new Random();
			var threads = new List<Thread> ();
			var threadsCount = 10;

			var results = new ConcurrentDictionary<int, Boolean>();
			for (int i = 0; i < threadsCount; i++)
			{
				var j = i;
				var parserData = new ParserData(cashe, "You can read this", cashe);
				var thread = new Thread(() => 
					{ 
						results.TryAdd(j, ThreadProc(stringFormatter, parserData, random, threadsCount)); 
					});
				threads.Add(thread);
				thread.Start();
			}

			Assert.That(threads.Count, Is.EqualTo(threadsCount));
			
			for (int i = 0; i < threadsCount; i++)
			{
				threads[i].Join();
				Assert.That(results[i], Is.True);
			}

			Assert.That(cashe.ReadAttemptsCount, Is.LessThanOrEqualTo(threadsCount*threadsCount*10));
			Assert.That(cashe.WriteAttemptsCount, Is.LessThanOrEqualTo(threadsCount));
			Assert.That(cashe.SuccessReadCount, Is.LessThanOrEqualTo(cashe.ReadAttemptsCount - cashe.WriteAttemptsCount));

			Assert.Pass();
		}

		public Boolean ThreadProc(StringFormatter stringFormatter, ParserData parserData, Random random, int testsAmount)
		{
			for (int i = 0; i < testsAmount*10; i++)
			{
				parserData.OpenBracketsCount = random.Next();
				if (stringFormatter.Format("some text {OpenBracketsCount}", parserData) != $"some text {parserData.OpenBracketsCount}")
				{
					return false;
				}
			}
			return true;
		}
	}
}