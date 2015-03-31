//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.mediatorMap.dsl;


namespace robotlegs.bender.extensions.mediatorMap.impl
{

	public class MediatorMapTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private MediatorMap mediatorMap;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void SetUp()
		{
			Context context = new Context();
			mediatorMap = new MediatorMap(context);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void MapMatcher_Creates_Mapper()
		{
			TypeMatcher matcher = new TypeMatcher().AllOf(typeof(SupportView));

			Assert.That (mediatorMap.MapMatcher (matcher), Is.InstanceOf (typeof(IMediatorMapper)));
		}

		[Test]
		public void MapMatcher_To_Matching_TypeMatcher_Returns_Same_Mapper()
		{
			TypeMatcher matcher1 = new TypeMatcher().AllOf(typeof(SupportView));
			TypeMatcher matcher2 = new TypeMatcher().AllOf(typeof(SupportView));
			object mapper1 = mediatorMap.MapMatcher(matcher1);
			object mapper2 = mediatorMap.MapMatcher(matcher2);

			Assert.That (mapper1, Is.EqualTo (mapper2));
		}

		[Test]
		public void MapMatcher_To_Differing_TypeMatcher_Returns_Different_Mapper()
		{
			TypeMatcher matcher1 = new TypeMatcher().AllOf(typeof(SupportView));
			TypeMatcher matcher2 = new TypeMatcher().AllOf(typeof(Event));
			object mapper1 = mediatorMap.MapMatcher(matcher1);
			object mapper2 = mediatorMap.MapMatcher(matcher2);
			Assert.That(mapper1, Is.Not.EqualTo(mapper2));
		}

		[Test]
		public void Unmap_Returns_Mapper()
		{
			object mapper = mediatorMap.MapMatcher(new TypeMatcher().AllOf(typeof(SupportView)));
			Assert.That(mediatorMap.UnmapMatcher(new TypeMatcher().AllOf(typeof(SupportView))), Is.EqualTo(mapper));
		}

		[Test]
		public void Robust_To_Unmapping_Non_Existent_Mappings()
		{
			mediatorMap.UnmapMatcher(new TypeMatcher().AllOf(typeof(SupportView))).FromAll();
		}
	}
}
