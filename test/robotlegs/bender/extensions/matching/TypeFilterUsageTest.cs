//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;
using robotlegs.bender.extensions.matching.support;

namespace robotlegs.bender.extensions.matching
{
	[TestFixture]
	public class TypeFilterUsageTest
	{

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void class_excluded_by_none()
		{
			TypeB12 subject = new TypeB12();

			ITypeFilter filter = new TypeFilter(new List<Type>{}, new List<Type>{}, new List<Type>{typeof(TypeB)});

			Assert.That (filter.Matches (subject), Is.False);
		}

		[Test]
		public void class_matched_by_all()
		{
			TypeA12 subject = new TypeA12();

			ITypeFilter filter = new TypeFilter(new List<Type>{typeof(TypeA), typeof(IType1), typeof(IType2)}, new List<Type>{}, new List<Type>{});

			Assert.That (filter.Matches(subject), Is.True);
		}

		[Test]
		public void class_matched_by_any()
		{
			TypeB12 subject = new TypeB12();

			ITypeFilter filter = new TypeFilter(new List<Type>{}, new List<Type>{typeof(TypeA), typeof(IType1)}, new List<Type>{});

			Assert.That (filter.Matches(subject), Is.True);
		}

		[Test]
		public void class_not_excluded_by_none()
		{
			TypeA12 subject = new TypeA12();

			ITypeFilter filter = new TypeFilter(new List<Type>{}, new List<Type>{}, new List<Type>{typeof(TypeB)});

			Assert.That (filter.Matches(subject), Is.True);
		}

		[Test]
		public void class_not_matched_by_all()
		{
			TypeA1 subject = new TypeA1();

			ITypeFilter filter = new TypeFilter(new List<Type>{typeof(TypeA), typeof(IType1), typeof(IType2)}, new List<Type>{}, new List<Type>{});

			Assert.That (filter.Matches(subject), Is.False);
		}

		[Test]
		public void class_not_matched_by_any()
		{
			TypeB subject = new TypeB();

			ITypeFilter filter = new TypeFilter(new List<Type>{}, new List<Type>{typeof(TypeA), typeof(IType1)}, new List<Type>{});

			Assert.That (filter.Matches(subject), Is.False);
		}

		[Test]
		public void default_behaviour_where_nothing_is_specified()
		{
			TypeB subject = new TypeB();

			ITypeFilter filter = new TypeFilter(new List<Type>{}, new List<Type>{}, new List<Type>{});

			Assert.That (filter.Matches(subject), Is.False);
		}
	}
}
