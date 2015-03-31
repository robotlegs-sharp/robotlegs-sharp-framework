//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using System.Runtime.Remoting.Contexts;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.framework.impl.configSupport;
using System.Collections.Generic;

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
			Assert.That (actual, Is.EqualTo (expected));
		}

		[Test]
		public void plain_config_class_added_before_initialization_is_not_immediately_instantiated()
		{
			PlainConfig actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			configManager.AddConfig<PlainConfig>();

			Assert.That (actual, Is.Null);
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

			Assert.That(actual, Is.InstanceOf<PlainConfig>());
		}

		[Test]
		public void plain_config_class_added_after_initialization_is_immediately_instantiated()
		{
			object actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			context.Initialize();
			configManager.AddConfig(typeof(PlainConfig));

			Assert.That(actual, Is.InstanceOf<PlainConfig>());
		}

		[Test]
		public void plain_config_object_added_before_initializiation_is_not_injected_into()
		{
			PlainConfig expected = new PlainConfig();
			object actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			configManager.AddConfig(expected);

			Assert.That(actual, Is.Null);
		}

		[Test]
		public void plain_config_object_added_before_initializiation_is_injected_into_at_initialization()
		{
			PlainConfig expected = new PlainConfig();
			object actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			configManager.AddConfig(expected);
			context.Initialize();

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void plain_config_object_added_after_initializiation_is_injected_into()
		{
			PlainConfig expected = new PlainConfig();
			object actual = null;
			injector.Map(typeof(Action<PlainConfig>), "callback").ToValue((Action<PlainConfig>)delegate(PlainConfig config) {
				actual = config;
			});

			context.Initialize();
			configManager.AddConfig(expected);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void configure_is_invoked_for_IConfig_object()
		{
			TypedConfig expected = new TypedConfig();
			object actual = null;
			injector.Map(typeof(Action<TypedConfig>), "callback").ToValue ((Action<TypedConfig>)delegate(TypedConfig config) {
				actual = config;
			});
			configManager.AddConfig(expected);
			context.Initialize();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void configure_is_invoked_for_IConfig_class()
		{
			object actual = null;
			injector.Map(typeof(Action<TypedConfig>), "callback").ToValue ((Action<TypedConfig>)delegate(TypedConfig config) {
				actual = config;
			});
			configManager.AddConfig(typeof(TypedConfig));
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<TypedConfig>());
		}

		[Test]
		public void config_queue_is_processed_after_other_initialize_listeners()
		{
			List<string> actual = new List<string>();
			injector.Map(typeof(Action<TypedConfig>), "callback").ToValue((Action<TypedConfig>)delegate(TypedConfig config) {
				actual.Add("config");
			});
			configManager.AddConfig(typeof(TypedConfig));
			context.WhenInitializing(delegate() {
				actual.Add("listener1");
			});
			context.WhenInitializing(delegate() {
				actual.Add("listener2");
			});
			context.Initialize();
			List<string> expected = new List<string>{"listener1", "listener2", "config" };
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}
			
		[Test]
		public void destroy()
		{
			configManager.AddConfigHandler(new InstanceOfMatcher (typeof(string)), delegate(object config) {
				Assert.Fail("Handler should not fire after call to destroy");
			});
			configManager.Destroy();
			configManager.AddConfig("string");
		}

		[Test]
		public void test_untyped_config_calls_configure()
		{
			object actual = null;
			injector.Map(typeof(Action<UntypedConfig>), "callback").ToValue ((Action<UntypedConfig>)delegate(UntypedConfig config) {
				actual = config;
			});
			configManager.AddConfig(typeof(UntypedConfig));
			context.Initialize();
			Assert.That(actual, Is.InstanceOf<UntypedConfig>());
		}
	}
}

