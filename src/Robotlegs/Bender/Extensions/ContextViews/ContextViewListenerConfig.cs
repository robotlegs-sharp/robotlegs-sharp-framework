//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextViews.API;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Framework.API;


namespace Robotlegs.Bender.Extensions.ContextViews
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

