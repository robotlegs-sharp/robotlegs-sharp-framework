//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.commandCenter.impl;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class PriorityMapping : CommandMapping
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public int priority;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public PriorityMapping (Type command, int priority) : base(command)
		{
			this.priority = priority;
		}
	}
}

