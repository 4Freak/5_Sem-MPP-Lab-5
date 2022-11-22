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
		public void StringValidatorTest ()
		{
			var stringValidator = new StringValidator();
			var ids = stringValidator.IsValid("string");
			ids = stringValidator.IsValid("{id}");
			ids = stringValidator.IsValid("SomeText {id} some text");
			ids = stringValidator.IsValid("SomeText {id} some text {_id2}");
			ids = stringValidator.IsValid("{{CorrectId}}");
			try
			{
				ids = stringValidator.IsValid("Some text {{0InCorrectId}}");
			}
			catch{ };
			try
			{
				ids = stringValidator.IsValid("}{");
			}
			catch{ };
			try
			{
				ids = stringValidator.IsValid("{{");
			}
			catch{ }
			try
			{ 
				ids = stringValidator.IsValid("}");
			}
			catch{ } 
			Assert.Pass();
		}
	}
}