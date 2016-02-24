//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;

namespace Robotlegs.Bender.Extensions.Mediation.Support
{

	public class CallbackMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject(true, "callback")]
		public Action<object> callback;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		[PostConstruct]
		public void Init()
		{
			if (callback != null)
			{
				callback(this);
			}
		}
	}
}
