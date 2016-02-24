//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
﻿
using Robotlegs.Bender.Extensions.DirectCommand.API;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.DirectCommand
{
	public class DirectCommandMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IDirectCommandMap)).ToType(typeof(Impl.DirectCommandMap));
		}
	}
}

