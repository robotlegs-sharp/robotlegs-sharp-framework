//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.matching
{
	public class InstanceOfMatcher : IMatcher
	{
		private Type _type;
		
		public InstanceOfMatcher(Type type)
		{
			_type = type;
		}
		
		public bool Matches(object item)
		{
			return _type.IsInstanceOfType(item);
		}
	}
}

