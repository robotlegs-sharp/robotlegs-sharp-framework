//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Robotlegs.Bender.Framework.Impl.ContextSupport;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Framework.Impl
{
	public class ExtensionInstallerTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ExtensionInstaller installer;

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			Context context = new Context ();
			injector = context.injector;
			installer = new ExtensionInstaller(context);
		}

		[TearDown]
		public void after()
		{
			CallbackExtension.staticCallback = null;
			CallbackBundle.staticCallback = null;
			CallbackExtensionInjectable.staticCallback = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void extension_instance_is_installed()
		{
			int callCount = 0;
			installer.Install(new CallbackExtension((Action<IContext>)delegate(IContext ctx) {
				callCount++;
			}));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void extension_class_is_installed()
		{
			int callCount = 0;
			CallbackExtension.staticCallback = (Action<IContext>)delegate(IContext ctx) {
				callCount++;
			};
			installer.Install(typeof(CallbackExtension));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void extension_is_installed_once_for_same_instance()
		{
			int callCount = 0;
			Action<IContext> callback = (Action<IContext>)delegate(IContext ctx) {
				callCount++;
			};
			IExtension extension = new CallbackExtension(callback);
			installer.Install(extension);
			installer.Install(extension);
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void extension_is_installed_once_for_same_class()
		{
			int callCount = 0;
			Action<IContext> callback = (Action<IContext>)delegate(IContext ctx) {
				callCount++;
			};
			installer.Install(new CallbackExtension(callback));
			installer.Install(new CallbackExtension(callback));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void extension_instance_is_not_injected_into()
		{
			CallbackExtensionInjectable extension = new CallbackExtensionInjectable ();
			installer.Install (extension);
			Assert.That (extension.injector, Is.Null);
		}
			
		[Test]
		public void extension_class_is_not_injected_into()
		{
			CallbackExtensionInjectable extension = null;
			CallbackExtensionInjectable.staticCallback = (Action<CallbackExtensionInjectable>)delegate (CallbackExtensionInjectable value) {
				extension = value;
			};
			installer.Install <CallbackExtensionInjectable>();
			Assert.That (extension, Is.Not.Null);
			Assert.That (extension.injector, Is.Null);
		}

		[Test]
		public void bundle_instance_is_installed()
		{
			int callCount = 0;
			installer.Install(new CallbackBundle((Action<IContext>)delegate(IContext ctx) {
				callCount++;
			}));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void bundle_class_is_installed()
		{
			int callCount = 0;
			CallbackBundle.staticCallback = (Action<IContext>)delegate(IContext ctx) {
				callCount++;
			};
			installer.Install(typeof(CallbackBundle));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void instance_with_extend_method_is_installed()
		{
			int callCount = 0;
			installer.Install(new CallbackUntypedExtension((Action<IContext>)delegate(IContext ctx) {
				callCount++;
			}));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void class_with_extend_method_is_installed()
		{
			int callCount = 0;
			CallbackUntypedExtension.staticCallback = (Action<IContext>)delegate(IContext ctx) {
				callCount++;
			};
			installer.Install(typeof(CallbackUntypedExtension));
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void class_with_multiple_construtors_is_instantiated_with_the_least_number_of_arguments()
		{
			ExtensionWithMultipleConstructors instance = null;
			ExtensionWithMultipleConstructors.staticCallback = (Action<ExtensionWithMultipleConstructors>)delegate(ExtensionWithMultipleConstructors value) {
				instance = value;
			};
			installer.Install(typeof(ExtensionWithMultipleConstructors));
			Assert.That (instance.constructorArguments, Is.EqualTo (2));
		}

		[Test]
		public void class_with_multiple_construtors_is_passed_defaults()
		{
			ExtensionWithMultipleConstructors instance = null;
			ExtensionWithMultipleConstructors.staticCallback = (Action<ExtensionWithMultipleConstructors>)delegate(ExtensionWithMultipleConstructors value) {
				instance = value;
			};
			installer.Install(typeof(ExtensionWithMultipleConstructors));
			Assert.That (instance.value1, Is.EqualTo (1));
			Assert.That (instance.value2, Is.EqualTo ("arg"));
		}
	}
}

