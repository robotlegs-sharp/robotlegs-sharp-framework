//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.commandCenter.api;
using Moq;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class CommandMapStub
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual object KeyFactory(params object[] args)
		{
			string s = "";
			for (int i = 0; i < args.Length; i++)
			{
				s += args[i].ToString ();
				if (i < args.Length - 1)
					s += "::";
			}
			return s;
		}

		public virtual ICommandTrigger TriggerFactory(params object[] args)
		{
			return new Mock<ICommandTrigger> ().Object;
		}

		public virtual void Hook(params object[] args)
		{

		}
	}
}

