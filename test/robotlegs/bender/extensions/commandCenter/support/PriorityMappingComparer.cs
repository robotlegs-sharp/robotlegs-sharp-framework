//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.CommandCenter.API;

namespace Robotlegs.Bender.Extensions.CommandCenter.Support
{
	public class PriorityMappingComparer : IComparer<ICommandMapping>
	{
		public int Compare (ICommandMapping a, ICommandMapping b)
		{
			int aPriority = (a is PriorityMapping) ? (a as PriorityMapping).priority : 0;
			int bPriority = (b is PriorityMapping) ? (b as PriorityMapping).priority : 0;
			if (aPriority == bPriority)
				return 0;
			return aPriority > bPriority ? 1 : -1;
		}
	}
}

