//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.EventCommand.API;
using Robotlegs.Bender.Extensions.EventCommand.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.EventCommand
{
	/// <summary>
	/// The Event Command Map allows you to bind Events to Commands
	/// </summary>

	public class EventCommandMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IEventCommandMap)).ToSingleton(typeof(Impl.EventCommandMap));
		}
	}
}

