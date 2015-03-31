//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.contextview.api;


namespace robotlegs.bender.extensions.contextview
{
	public class ContextViewListenerConfig : IConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IContextView contextView {get;set;}
		
		[Inject]
		public IViewManager viewManager {get;set;}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure ()
		{
			viewManager.AddContainer(contextView.view);
		}
	}
}

