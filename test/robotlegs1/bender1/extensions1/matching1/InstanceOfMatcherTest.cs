//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using NUnit.Framework;

namespace Robotlegs.Bender.Extensions.Matching
{
	[TestFixture]
	public class InstanceOfMatcherTest
	{

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void matches_type()
		{
			Assert.That(new InstanceOfMatcher(typeof(float)).Matches(5f), Is.True);
		}

		[Test]
		public void does_not_match_wrong_type()
		{
			Assert.That(new InstanceOfMatcher(typeof(string)).Matches(5f), Is.False);
		}

		[Test]
		public void does_not_match_wrong_type_int_float()
		{
			Assert.That(new InstanceOfMatcher(typeof(int)).Matches(5f), Is.False);
		}
	}
}
