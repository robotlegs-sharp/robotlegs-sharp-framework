//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using NUnit.Framework;
using System;
using System.Collections.Generic;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.eventDispatcher.api;

namespace robotlegs.bender.extensions.matching
{
	[TestFixture]
	public class TypeFilterTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private static readonly List<Type> ALL_OF = new List<Type>{typeof(uint), typeof(float)};

		private static readonly List<Type> ANY_OF = new List<Type>{typeof(Context), typeof(IEventDispatcher)};

		private static readonly List<Type> NONE_OF = new List<Type>{typeof(String), typeof(Exception)};

		private TypeFilter instance;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void setUp()
		{
			instance = new TypeFilter(ALL_OF, ANY_OF, NONE_OF);
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
			Assert.That (instance, Is.InstanceOf<TypeFilter> ());
		}

		[Test]
		public void get_allOfTypes()
		{
			Assert.That(instance.AllOfTypes, Is.EqualTo(ALL_OF).AsCollection);
		}

		[Test]
		public void get_anyOfTypes()
		{
			Assert.That(instance.AnyOfTypes, Is.EqualTo(ANY_OF).AsCollection);
		}

		[Test]
		public void get_descriptor_returns_alphabetised_readable_list()
		{
			string expected = "all of: System.Single, System.UInt32, any of: robotlegs.bender.extensions.eventDispatcher.api.IEventDispatcher, robotlegs.bender.framework.impl.Context, none of: System.Exception, System.String";
			Console.WriteLine (instance.Descriptor);
			Assert.That(instance.Descriptor, Is.EqualTo(expected));
		}

		[Test]
		public void get_noneOfTypes()
		{
			Assert.That(instance.NoneOfTypes, Is.EqualTo(NONE_OF).AsCollection);
		}

		[Test]
		public void implements_ITypeFilter_interface()
		{
			Assert.That(instance, Is.AssignableTo<ITypeFilter>());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void initialising_with_allOf_null_throws_error()
		{
			new TypeFilter(null, ANY_OF, NONE_OF);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void initialising_with_anyOf_null_throws_error()
		{
			new TypeFilter(ALL_OF, null, NONE_OF);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void initialising_with_noneOf_null_throws_error()
		{
			new TypeFilter(ALL_OF, ANY_OF, null);
		}
	}
}
