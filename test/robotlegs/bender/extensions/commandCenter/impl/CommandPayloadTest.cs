using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.commandCenter.api;
using NUnit.Framework;

namespace robotlegs.bender.extensions.commandCenter.impl
{
	public class CommandPayloadTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private CommandPayload subject;

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void test_values_by_default_null() {
			createConfig();

			Assert.That(subject.Values, Is.Null);
		}

		[Test]
		public void test_classes_by_default_null() {
			createConfig();

			Assert.That(subject.Classes, Is.Null);
		}

		[Test]
		public void test_values_are_stored()
		{
			List<object> expected = new List<object>{ "string", 0 };

			createConfig(expected);

			Assert.That(subject.Values, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void test_classes_are_stored()
		{
			List<Type> expected = new List<Type>{ typeof(String), typeof(int) };

			createConfig(null, expected);

			Assert.That(subject.Classes, Is.EqualTo(expected).AsCollection);
		}

		[Test]
		public void test_adding_stores_values() {
			createConfig();

			subject.AddPayload( "string", typeof(String));

			bool hasValue  = subject.Values.Contains( "string" );
			Assert.That( hasValue, Is.True);
		}

		[Test]
		public void test_adding_stores_classes() {
			createConfig();

			subject.AddPayload( "string", typeof(String));

			bool hasClass  = subject.Classes.Contains( typeof(string) );
			Assert.That( hasClass, Is.True );
		}

		[Test]
		public void test_adding_stores_in_lockstep() {
			createConfig(new List<object>{"string", 0},new List<Type>{typeof(String), typeof(int)});
			float value = 5f;

			subject.AddPayload( value, typeof(float) );

			int valueIndex = subject.Values.IndexOf( value );
			int classIndex = subject.Classes.IndexOf( typeof(float) );
			Assert.That(valueIndex, Is.EqualTo(classIndex));
		}

		[Test]
		public void can_ask_for_length_without_classes()
		{
			createConfig();
			Assert.That(subject.length, Is.EqualTo(0));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private CommandPayload createConfig(List<object> values = null, List<Type> classes = null)
		{
			return subject = new CommandPayload(values, classes);
		}
	}
}

