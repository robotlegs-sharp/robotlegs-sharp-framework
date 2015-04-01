//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.matching
{
	public interface ITypeFilter : IMatcher
	{
		List<Type> AllOfTypes {get;}
		List<Type> AnyOfTypes {get;}
		List<Type> NoneOfTypes {get;}
		string Descriptor {get;}
	}
}