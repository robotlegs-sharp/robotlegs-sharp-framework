//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.Mediation.API;


namespace Robotlegs.Bender.Extensions.ViewProcessor.Utils 
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