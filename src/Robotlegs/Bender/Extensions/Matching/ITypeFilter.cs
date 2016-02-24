//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.Matching
{
	public interface ITypeFilter : IMatcher
	{
		List<Type> AllOfTypes {get;}
		List<Type> AnyOfTypes {get;}
		List<Type> NoneOfTypes {get;}
		string Descriptor {get;}
	}
}