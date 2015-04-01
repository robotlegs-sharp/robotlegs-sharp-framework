//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.framework.api;
using swiftsuspenders;

namespace robotlegs.bender.framework.impl
{
	public class RobotlegsInjector : Injector, IInjector
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public IInjector parent
		{
			set
			{
				this.parentInjector = value as Injector;
			}
			get 
			{
				return this.parentInjector as RobotlegsInjector;
			}
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IInjector CreateChild(/*applicationDomain:ApplicationDomain = null*/)
		{
			IInjector childInjector = new RobotlegsInjector();
//			childInjector.applicationDomain = applicationDomain || this.applicationDomain;
			childInjector.parent = this;
			return childInjector;
		}
	}
}

