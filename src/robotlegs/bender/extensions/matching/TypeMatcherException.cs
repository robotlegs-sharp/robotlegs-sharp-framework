//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;

namespace robotlegs.bender.extensions.matching
{
	public class TypeMatcherException : Exception
	{
		public const string EMPTY_MATCHER = "An empty matcher will create a filter which matches nothing. You should specify at least one condition for the filter.";

		public const string SEALED_MATCHER = "This matcher has been sealed and can no longer be configured.";

		public TypeMatcherException (string message) : base(message)
		{

		}
	}
}

