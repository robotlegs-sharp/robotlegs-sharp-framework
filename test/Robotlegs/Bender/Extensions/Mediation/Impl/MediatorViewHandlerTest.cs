//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Extensions.Mediation.Impl;
using NUnit.Framework;
using Robotlegs.Bender.Framework.Impl;
using System;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Extensions.Mediation.Support;

namespace Robotlegs.Bender.Extensions.Mediation.Impl
{

	public class MediatorViewHandlerTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private MediatorViewHandler handler;

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			injector = new RobotlegsInjector();
			handler = new MediatorViewHandler(new MediatorFactory(injector));
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void View_Is_Handled()
		{
			object createdMediator = null;
			Action<object> callback = delegate(object mediator) {
				createdMediator = mediator;
			};
			injector.Map(typeof(Action<object>), "callback").ToValue(callback);
			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type [1] {typeof(SupportView)}), typeof(CallbackMediator));
			handler.AddMapping(mapping);
			handler.HandleView(new SupportView(), typeof(SupportView));
			Assert.That(createdMediator, Is.Not.Null);
		}

		[Test]
		public void View_Is_Not_Handled()
		{
			Object createdMediator = null;
			Action<object> callback = delegate(object mediator) {
				createdMediator = mediator;
			};
			injector.Map(typeof(Action<object>), "callback").ToValue(callback);
			MediatorMapping mapping = new MediatorMapping(CreateTypeFilter(new Type[1] { typeof(ObjectWhichExtendsSupportView) }), typeof(CallbackMediator));
			handler.AddMapping(mapping);
			handler.HandleView(new SupportView(), typeof(SupportView));

			Assert.That(createdMediator, Is.Null);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private ITypeFilter CreateTypeFilter(Type[] allOf, Type[] anyOf = null, Type[] noneOf = null)
		{
			TypeMatcher matcher = new TypeMatcher();
			if (allOf != null)
				matcher.AllOf(allOf);
			if (anyOf != null)
				matcher.AnyOf(anyOf);
			if (noneOf != null)
				matcher.NoneOf(noneOf);

			return matcher.CreateTypeFilter();
		}
	}
}
