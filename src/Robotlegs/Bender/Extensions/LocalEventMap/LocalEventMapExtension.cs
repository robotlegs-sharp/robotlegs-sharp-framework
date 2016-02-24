//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.LocalEventMap.API;
using Robotlegs.Bender.Extensions.LocalEventMap.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.LocalEventMap
{
	/// <summary>
	/// An Event Map keeps track of listeners and provides the ability to
	/// unregister all the listeners which a single method call.
	/// </summary>
	public class LocalEventMapExtension : IExtension
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.injector.Map(typeof(IEventMap)).ToType((typeof(EventMap)));
		}
	}
}

