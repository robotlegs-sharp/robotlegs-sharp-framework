using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;
using robotlegs.bender.extensions.viewManager.api;

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

