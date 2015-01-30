using System.Collections.Generic;
using NUnit.Framework;
using robotlegs.bender.extensions.mediatorMap.api;


namespace robotlegs.bender.extensions.viewProcessorMap.utils 
{
	[TestFixture]
	public class PropertyValueInjectorTest 
	{
		private PropertyValueInjector instance;

		private int INT_VALUE = 3;
		private string STRING_VALUE = "someValue";

		[SetUp]
		public void SetUp()
		{
			Dictionary<string,object> config = new Dictionary<string, object> ();
			config.Add ("intValue", INT_VALUE);
			config.Add ("stringValue", STRING_VALUE);
			instance = new PropertyValueInjector(config);
		}

		[TearDown]
		public void TearDown()
		{
			instance = null;
		}

		[Test]
		public void Can_Be_Instantiated()
		{
			Assert.That(instance, Is.InstanceOf(typeof(PropertyValueInjector)), "instance is PropertyValueInjector");
		}

		[Test]
		public void Process_Properties_Are_Injected()
		{
			ViewToBeInjected view = new ViewToBeInjected();
			instance.Process(view, typeof(ViewToBeInjected), null);

			Assert.That (view.intValue, Is.EqualTo (INT_VALUE));
			Assert.That (view.stringValue, Is.EqualTo (STRING_VALUE));
		}
	}
}

class ViewToBeInjected
{
	public int intValue = 0;
	public string stringValue = "";
}