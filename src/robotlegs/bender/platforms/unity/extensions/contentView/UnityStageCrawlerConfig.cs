//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.framework.api;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;

namespace robotlegs.bender.platforms.unity.extensions.contextview
{
	public class UnityStageCrawlerConfig : IConfig
	{
		[Inject]
		public IInjector injector {get;set;}

		public void Configure ()
		{
			injector.Map (typeof(IStageCrawler)).ToType (typeof(UnityStageCrawler));
		}

	}
}

