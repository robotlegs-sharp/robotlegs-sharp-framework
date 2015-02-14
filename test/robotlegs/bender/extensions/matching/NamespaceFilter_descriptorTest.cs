//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using NUnit.Framework;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.matching
{
	public class NamespaceFilter_DescriptorTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private string REQUIRE = "k.j";

		private List<string> ANY_OF = new List<string>{"b.c.d", "a.b.c"};

		private List<string> EMPTY_VECTOR = new List<string>();

		private List<string> NONE_OF = new List<string>{"f.g.h", "c.d.e"};

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void can_be_instantiated()
		{
			NamespaceFilter instance = new NamespaceFilter(REQUIRE, EMPTY_VECTOR, EMPTY_VECTOR);
			Assert.That (instance, Is.InstanceOf<NamespaceFilter> ());
		}

		[Test]
		public void descriptor_produced_as_alphabetised_readable_list()
		{
			NamespaceFilter filter = new NamespaceFilter(REQUIRE, ANY_OF, NONE_OF);
			string expected = "require: k.j, any of: a.b.c,b.c.d, none of: c.d.e,f.g.h";
			Assert.That(filter.Descriptor, Is.EqualTo(expected));
		}
	}
}
