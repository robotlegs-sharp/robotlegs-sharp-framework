using System;
using NUnit.Framework;
using System.Runtime.Remoting.Contexts;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.matching;

namespace robotlegs.bender.framework.impl
{
	public class ConfigManagerTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Context context;

		private IInjector injector;

		private ConfigManager configManager;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
			injector = context.injector;
			configManager = new ConfigManager(context);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void publicaddConfig()
		{
			configManager.AddConfig(new object());
		}

		[Test]
		public void addHandler()
		{
			configManager.AddConfigHandler(new InstanceOfMatcher(typeof(String)), delegate(object obj) {  });
		}

		[Test]
		public void handler_is_called()
		{
			string expected = "config";
			object actual = null;
			configManager.AddConfigHandler(new InstanceOfMatcher(typeof(String)), delegate(object config) {
				actual = config;
			});
			configManager.AddConfig(expected);
			Assert.AreEqual (actual, expected);
		}

		[Test]
		public void plain_config_class_added_before_initialization_is_not_immediately_instantiated()
		{
			PlainConfig actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			configManager.AddConfig<PlainConfig>();

//			assertThat(actual, nullValue());
			Assert.Null(actual);
		}

		[Test]
		public void plain_config_class_added_before_initialization_is_instantiated_at_initialization()
		{
			PlainConfig actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			configManager.AddConfig(typeof(PlainConfig));
			context.Initialize();

//			assertThat(actual, instanceOf(PlainConfig));
			Assert.IsInstanceOf<PlainConfig>(actual);
		}

		/*
		[Test]
		public void plain_config_class_added_after_initialization_is_immediately_instantiated()
		{
			var actual:Object = null;
			injector.map(Function, 'callback').toValue(function(config:PlainConfig) {
				actual = config;
			});

			context.initialize();
			configManager.addConfig(PlainConfig);

			assertThat(actual, instanceOf(PlainConfig));
		}

		[Test]
		public void plain_config_object_added_before_initializiation_is_not_injected_into()
		{
			const expected:PlainConfig = new PlainConfig();
			var actual:Object = null;
			injector.map(Function, 'callback').toValue(function(config:Object) {
				actual = config;
			});

			configManager.addConfig(expected);

			assertThat(actual, nullValue());
		}

		[Test]
		public void plain_config_object_added_before_initializiation_is_injected_into_at_initialization()
		{
			const expected:PlainConfig = new PlainConfig();
			var actual:Object = null;
			injector.map(Function, 'callback').toValue(function(config:Object) {
				actual = config;
			});

			configManager.addConfig(expected);
			context.initialize();

			assertThat(actual, equalTo(expected));
		}

		[Test]
		public void plain_config_object_added_after_initializiation_is_injected_into()
		{
			const expected:PlainConfig = new PlainConfig();
			var actual:Object = null;
			injector.map(Function, 'callback').toValue(function(config:Object) {
				actual = config;
			});

			context.initialize();
			configManager.addConfig(expected);

			assertThat(actual, equalTo(expected));
		}

		[Test]
		public void configure_is_invoked_for_IConfig_object()
		{
			const expected:TypedConfig = new TypedConfig();
			var actual:Object = null;
			injector.map(Function, 'callback').toValue(function(config:Object) {
				actual = config;
			});
			configManager.addConfig(expected);
			context.initialize();
			assertThat(actual, equalTo(expected));
		}

		[Test]
		public void configure_is_invoked_for_IConfig_class()
		{
			var actual:Object = null;
			injector.map(Function, 'callback').toValue(function(config:Object) {
				actual = config;
			});
			configManager.addConfig(TypedConfig);
			context.initialize();
			assertThat(actual, instanceOf(TypedConfig));
		}

		[Test]
		public void config_queue_is_processed_after_other_initialize_listeners()
		{
			const actual:Array = [];
			injector.map(Function, 'callback').toValue(function(config:Object) {
				actual.push('config');
			});
			configManager.addConfig(TypedConfig);
			context.whenInitializing(function() {
				actual.push('listener1');
			});
			context.whenInitializing(function() {
				actual.push('listener2');
			});
			context.initialize();
			assertThat(actual, array(['listener1', 'listener2', 'config']));
		}

		[Test]
		public void destroy()
		{
			configManager.addConfigHandler(instanceOfType(String), function(config:Object) {
				fail("Handler should not fire after call to destroy");
			});
			configManager.destroy();
			configManager.addConfig("string");
		}
		*/
	}
}

