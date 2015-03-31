using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.platforms.unity.extensions.contextview
{
	public class UnityFallbackStageCrawlerConfig : IConfig
	{
		[Inject]
		public IInjector injector {get;set;}
		
		public void Configure ()
		{
			Type stageCrawlerInterface = typeof(IStageCrawler);
			if (injector.HasMapping (stageCrawlerInterface)) 
			{
				injector.Unmap (stageCrawlerInterface);
			}
			injector.Map (stageCrawlerInterface).ToType (typeof(UnityFallbackStageCrawler));
		}
		
	}
}

