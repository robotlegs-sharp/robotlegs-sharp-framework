//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.extensions.localEventMap.impl;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.localEventMap
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

