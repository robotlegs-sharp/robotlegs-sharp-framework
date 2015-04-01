//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
﻿
using robotlegs.bender.extensions.directCommandMap.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.directCommandMap
{
	public class DirectCommandMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IDirectCommandMap)).ToType(typeof(DirectCommandMap));
		}
	}
}

