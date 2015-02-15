using swiftsuspenders;
using NUnit.Framework;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.extensions.mediatorMap.impl.support;
using System.Collections.Generic;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorMapMemoryLeakTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private MediatorMap mediatorMap;

		private MediatorWeakMapTracker mediatorTracker;

		private MediatorManager mediatorManager;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void SetUp()
		{
			Context context = new Context();
			mediatorMap = new MediatorMap(context);

			context.injector.Map(typeof(MediatorWeakMapTracker)).ToValue(new MediatorWeakMapTracker());
		}

		[TearDown]
		public void TearDown()
		{
			mediatorMap = null;
			mediatorManager = null;
			mediatorTracker = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/
		/*
		[Test]
		public void Mediators_Are_Released_And_Not_Held_In_Memory()
		{
			mediatorMap.Map(typeof(SupportView).ToMediator(SupportMediator);
			mediatorMap.map(Sprite).toMediator(ExampleMediator2);

			const view:Sprite = new Sprite();

			mediatorMap.mediate(view);

			const trackedMediators:Dictionary = mediatorTracker.trackedMediators;

			var expectedNumberOfKeys:uint = 2;
			assertEquals(expectedNumberOfKeys, dictionaryLength(trackedMediators));

			mediatorMap.unmediate(view);
			System.gc();

			expectedNumberOfKeys = 0;

			assertAfterDelay(function():void {
				assertEquals(expectedNumberOfKeys, dictionaryLength(trackedMediators));
			}, 50);
		}

		[Test]
		public void Mediated_View_Is_Released_And_Not_Held_In_Memory()
		{
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(SupportMediator));
			mediatorMap.Map(typeof(SupportView)).ToMediator(typeof(SupportMediator2));

			List<SupportView> views = new List<SupportView>();
			views.Add new Sprite();

			const trackedViews:Dictionary = new Dictionary(true);
			trackedViews[views[0]] = true;

			mediatorMap.mediate(views[0]);

			var expectedNumberOfKeys:uint = 1;
			assertEquals(expectedNumberOfKeys, dictionaryLength(trackedViews));

			mediatorMap.unmediate(views[0]);
			views[0] = new Sprite();
			System.gc();

			expectedNumberOfKeys = 0;

			assertAfterDelay(function():void {
				assertEquals(expectedNumberOfKeys, dictionaryLength(trackedViews));
			}, 50);
		}
		//*/
	}

	class ExampleMediator : IMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public MediatorWeakMapTracker mediatorTracker {get;set;}

		[Inject]
		public SupportView view  {get;set;}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Initialize()
		{
			mediatorTracker.TrackMediator(this);
		}

		public void Destroy()
		{
		}
	}

	class ExampleMediator2 : IMediator
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public MediatorWeakMapTracker mediatorTracker  {get;set;}
		
		[Inject]
		public SupportView view {get;set;}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Initialize()
		{
			mediatorTracker.TrackMediator(this);
		}

		public void Destroy()
		{
		}
	}
}