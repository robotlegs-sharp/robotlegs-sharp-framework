//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager.Impl;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.ContextViews
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

