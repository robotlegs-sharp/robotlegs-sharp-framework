//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using NUnit.Framework;
using Robotlegs.Bender.Extensions.Matching.Support.A;
using Robotlegs.Bender.Extensions.Matching.Support.C;
using Robotlegs.Bender.Extensions.Matching.Support.B;
using System;

namespace Robotlegs.Bender.Extensions.Matching
{
	public class NamespaceMatchingTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private NamespaceMatcher instance;

		private string PACKAGE_A = "Robotlegs.Bender.Extensions.Matching.Support.A";

		private string PACKAGE_B = "Robotlegs.Bender.Extensions.Matching.Support.B";

		private string PACKAGE_C = "Robotlegs.Bender.Extensions.Matching.Support.C";

		private string PARENT_PACKAGE = "Robotlegs.Bender.Extensions.Matching.Support";

		private string REQUIRE;

		private string REQUIRE_2;

		private string[] ANY_OF;

		private string[] ANY_OF_2;

		private string[] EMPTY_VECTOR = new string[0];

		private string[] NONE_OF;

		private string[] NONE_OF_2;

		private PackagedTypeA ITEM_A = new PackagedTypeA();

		private PackagedTypeB ITEM_B = new PackagedTypeB();

		private PackagedTypeC ITEM_C = new PackagedTypeC();

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void setUp()
		{
			REQUIRE = PARENT_PACKAGE;
			REQUIRE_2 = PACKAGE_A;
			ANY_OF = new string[]{PACKAGE_A, PACKAGE_B};
			ANY_OF_2 = new string[]{PACKAGE_B, PACKAGE_C};
			NONE_OF = new string[]{PACKAGE_C};
			NONE_OF_2 = new string[]{PACKAGE_B};

			instance = new NamespaceMatcher();
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
			Assert.That (instance, Is.InstanceOf<NamespaceMatcher> ());
		}

		[Test]
		public void implements_ITypeMatcher()
		{
			Assert.That (instance, Is.InstanceOf<ITypeMatcher> ());
		}

		[Test]
		public void matches_based_on_anyOf()
		{
			instance.AnyOf(ANY_OF);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.That(typeFilter.Matches(ITEM_A), Is.True);
		}

		[Test]
		public void matches_based_on_noneOf()
		{
			instance.NoneOf(NONE_OF);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.True(typeFilter.Matches(ITEM_B));
		}

		[Test]
		public void doesnt_match_based_on_noneOf()
		{
			instance.NoneOf(NONE_OF);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.False(typeFilter.Matches(ITEM_C));
		}

		[Test]
		public void matches_based_on_noneOf_twice()
		{
			instance.NoneOf(NONE_OF);
			instance.NoneOf(NONE_OF_2);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.False(typeFilter.Matches(ITEM_B));
			Assert.False(typeFilter.Matches(ITEM_C));
		}

		[Test]
		public void matches_based_on_anyOf_twice()
		{
			instance.AnyOf(ANY_OF);
			instance.AnyOf(ANY_OF_2);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.True(typeFilter.Matches(ITEM_A));
			Assert.True(typeFilter.Matches(ITEM_B));
			Assert.True(typeFilter.Matches(ITEM_C));
		}

		[Test]
		public void matches_subpackage_a_based_on_required()
		{
			instance.Require(REQUIRE);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.True(typeFilter.Matches(ITEM_A));
		}

		[Test]
		public void matches_subpackage_b_based_on_required()
		{
			instance.Require(REQUIRE);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.True(typeFilter.Matches(ITEM_B));
		}

		[Test]
		public void doesnt_match_subpackage_c_based_on_required_and_noneOf()
		{
			instance.Require(REQUIRE).NoneOf(NONE_OF);
			ITypeFilter typeFilter = instance.CreateTypeFilter();
			Assert.False(typeFilter.Matches(ITEM_C));
		}

        [Test]
        public void throws_IllegalOperationError_if_require_changed_after_filter_requested()
        {
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.AnyOf(ANY_OF);
                instance.CreateTypeFilter();
                instance.Require(REQUIRE);
            }));
		}

		[Test]
		public void throws_IllegalOperationError_if_require_changed_after_lock()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.AnyOf(ANY_OF);
                instance.Lock();
                instance.Require(REQUIRE);
            }
            ));
		}

		[Test]
		public void throws_IllegalOperationError_if_anyOf_changed_after_filter_requested()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.NoneOf(NONE_OF);
                instance.CreateTypeFilter();
                instance.AnyOf(ANY_OF);
            }
            ));
		}

		[Test]
		public void throws_IllegalOperationError_if_anyOf_changed_after_lock()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.NoneOf(NONE_OF);
                instance.Lock();
                instance.AnyOf(ANY_OF);
            }
            ));
		}

		[Test]
		public void throws_IllegalOperationError_if_noneOf_changed_after_filter_requested()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.AnyOf(ANY_OF);
                instance.CreateTypeFilter();
                instance.NoneOf(NONE_OF);
            }
            ));
		}

		[Test]
		public void throws_IllegalOperationError_if_noneOf_changed_after_lock()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.AnyOf(ANY_OF);
                instance.Lock();
                instance.NoneOf(NONE_OF);
            }
            ));
		}

		[Test]
		public void throws_IllegalOperationError_if_require_called_twice()
		{
            Assert.Throws(typeof(Exception), new TestDelegate(() =>
            {
                instance.Require(REQUIRE);
                instance.Require(REQUIRE_2);
            }
            ));
		}

		[Test]
		public void throws_TypeMatcherError_if_conditions_empty_and_filter_requested()
		{
            Assert.Throws(typeof(TypeMatcherException), new TestDelegate(() =>
            {
                NamespaceMatcher emptyInstance = new NamespaceMatcher();
                emptyInstance.AnyOf(new string[0]).NoneOf(new string[0]);
                emptyInstance.CreateTypeFilter();
            }
            ));
		}

		[Test]
        public void throws_TypeMatcherError_if_empty_and_filter_requested()
        {
            Assert.Throws(typeof(TypeMatcherException), new TestDelegate(() =>
            {
                NamespaceMatcher emptyInstance = new NamespaceMatcher();
                emptyInstance.CreateTypeFilter();
            }
            ));
        }
	}
}
