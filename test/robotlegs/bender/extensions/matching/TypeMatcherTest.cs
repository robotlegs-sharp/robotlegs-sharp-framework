//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.framework.impl;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.matching
{
	[TestFixture]
	public class TypeMatcherTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private static readonly List<Type> ALL_OF = new List<Type>{typeof(uint), typeof(float)};

		private static readonly List<Type> ALL_OF_2 = new List<Type>{typeof(object), typeof(IConfig)};

		private static readonly List<Type> ANY_OF = new List<Type>{typeof(Context), typeof(IEventDispatcher)};

		private static readonly List<Type> ANY_OF_2 = new List<Type>{typeof(SupportContainer), typeof(SupportView)};

		private static readonly List<Type> NONE_OF = new List<Type>{typeof(byte[]), typeof(string)};

		private static readonly List<Type> NONE_OF_2 = new List<Type>{typeof(ITypeFilter), typeof(ITypeMatcher)};

		private static readonly List<Type> EMPTY_CLASS_VECTOR = new List<Type>{};

		private TypeMatcher instance;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void setUp()
		{
			instance = new TypeMatcher();
		}

		[TearDown]
		public void tearDown()
		{
			instance = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void can_be_instantiated()
		{
			Assert.That(instance, Is.InstanceOf<TypeMatcher>());
		}

		[Test]
		public void implements_ITypeMatcher()
		{
			Assert.That(instance, Is.InstanceOf<ITypeMatcher>());
		}

		[Test]
		public void not_supplying_allOf_causes_no_errors()
		{
			TypeFilter expectedFilter = new TypeFilter(new List<Type>{}, ANY_OF, NONE_OF);

			instance.AnyOf(ANY_OF.ToArray()).NoneOf(NONE_OF.ToArray());
			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void not_supplying_anyOf_causes_no_errors()
		{
			TypeFilter expectedFilter = new TypeFilter(ALL_OF, new List<Type>{}, NONE_OF);

			instance.AllOf(ALL_OF.ToArray()).NoneOf(NONE_OF.ToArray());
			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void not_supplying_noneOf_causes_no_errors()
		{
			TypeFilter expectedFilter = new TypeFilter(ALL_OF, ANY_OF, new List<Type>{});

			instance.AnyOf(ANY_OF.ToArray()).AllOf(ALL_OF.ToArray());
			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void supplying_all_any_and_none_in_different_order_populates_them_in_typeFilter()
		{

			TypeFilter expectedFilter = new TypeFilter(ALL_OF, ANY_OF, NONE_OF);

			instance.NoneOf(NONE_OF.ToArray()).AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray());

			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void supplying_all_any_and_none_populates_them_in_typeFilter()
		{

			TypeFilter expectedFilter = new TypeFilter(ALL_OF, ANY_OF, NONE_OF);

			instance.AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray()).NoneOf(NONE_OF.ToArray());

			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void supplying_multiple_all_values_includes_all_given_in_typeFilter()
		{
			TypeFilter expectedFilter = new TypeFilter(Concat(ALL_OF, ALL_OF_2) as List<Type>, ANY_OF, NONE_OF);

			instance.AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray()).NoneOf(NONE_OF.ToArray()).AllOf(ALL_OF_2.ToArray());
			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void supplying_multiple_any_values_includes_all_given_in_typeFilter()
		{

			TypeFilter expectedFilter = new TypeFilter(ALL_OF, Concat(ANY_OF, ANY_OF_2), NONE_OF);

			instance.AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray()).NoneOf(NONE_OF.ToArray()).AnyOf(ANY_OF_2.ToArray());
			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test]
		public void supplying_multiple_none_values_includes_all_given_in_typeFilter()
		{

			TypeFilter expectedFilter = new TypeFilter(ALL_OF, ANY_OF, Concat(NONE_OF, NONE_OF_2));

			instance.AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray()).NoneOf(NONE_OF.ToArray()).NoneOf(NONE_OF_2.ToArray());
			assertMatchesTypeFilter(expectedFilter, instance.CreateTypeFilter());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_allOf_changed_after_filter_requested()
		{
			instance.AnyOf(ANY_OF.ToArray());
			instance.CreateTypeFilter();

			instance.AllOf(ALL_OF.ToArray());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_allOf_changed_after_lock()
		{
			instance.AnyOf(ANY_OF.ToArray());
			instance.Lock();

			instance.AllOf(ALL_OF.ToArray());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_anyOf_changed_after_filter_requested()
		{
			instance.NoneOf(NONE_OF.ToArray());
			instance.CreateTypeFilter();

			instance.AnyOf(ALL_OF.ToArray());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_anyOf_changed_after_lock()
		{
			instance.NoneOf(NONE_OF.ToArray());
			instance.Lock();

			instance.AnyOf(ALL_OF.ToArray());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_noneOf_changed_after_filter_requested()
		{
			instance.AllOf(ALL_OF.ToArray());
			instance.CreateTypeFilter();

			instance.NoneOf(ALL_OF.ToArray());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_noneOf_changed_after_lock()
		{
			instance.AllOf(ALL_OF.ToArray());
			instance.Lock();

			instance.NoneOf(ALL_OF.ToArray());
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_conditions_empty_and_filter_requested()
		{
			TypeMatcher emptyInstance = new TypeMatcher();
			emptyInstance.AllOf(EMPTY_CLASS_VECTOR.ToArray()).AnyOf(EMPTY_CLASS_VECTOR.ToArray()).NoneOf(EMPTY_CLASS_VECTOR.ToArray());

			emptyInstance.CreateTypeFilter();
		}

		[Test, ExpectedException(typeof(TypeMatcherException))]
		public void throws_TypeMatcherError_if_empty_and_filter_requested()
		{
			TypeMatcher emptyInstance = new TypeMatcher();
			emptyInstance.CreateTypeFilter();
		}

		[Test]
		public void clone_returns_open_copy_when_not_locked()
		{
			instance.AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray());
			TypeMatcher clone = instance.Clone();
			clone.NoneOf(NONE_OF.ToArray());
			TypeFilter expectedFilter = new TypeFilter(ALL_OF, ANY_OF, NONE_OF);
			assertMatchesTypeFilter(expectedFilter, clone.CreateTypeFilter());
		}

		[Test]
		public void clone_returns_open_copy_when_locked()
		{
			instance.AllOf(ALL_OF.ToArray()).AnyOf(ANY_OF.ToArray());
			instance.Lock();
			TypeMatcher clone = instance.Clone();
			clone.NoneOf(NONE_OF.ToArray());
			TypeFilter expectedFilter = new TypeFilter(ALL_OF, ANY_OF, NONE_OF);
			assertMatchesTypeFilter(expectedFilter, clone.CreateTypeFilter());
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected void assertMatchesTypeFilter(ITypeFilter expected, ITypeFilter actual)
		{
			Assert.That (actual.AllOfTypes, Is.EquivalentTo (expected.AllOfTypes), "Expected AllOfTypes to match");
			Assert.That (actual.AnyOfTypes, Is.EquivalentTo (expected.AnyOfTypes), "Expected AnyOfTypes to match");
			Assert.That (actual.NoneOfTypes, Is.EquivalentTo (expected.NoneOfTypes), "Expected NoneOfTypes to match");
		}

		private static List<Type> Concat(List<Type> list1, List<Type> list2)
		{
			List<Type> returnList = new List<Type> ();
			foreach (Type t in list1)
				returnList.Add(t);
			foreach (Type t in list2)
				returnList.Add(t);
			return returnList;
		}

		private static List<Type> ShallowClone(List<Type> list1)
		{
			List<Type> returnList = new List<Type> ();
			foreach (Type t in list1)
				returnList.Add(t);
			return returnList;
		}
	}
}
