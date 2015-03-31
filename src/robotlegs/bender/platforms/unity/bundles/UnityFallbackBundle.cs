using UnityEngine;
using System.Collections;
using robotlegs.bender.framework.api;
using robotlegs.bender.platforms.unity.extensions.contextview;
using robotlegs.bender.extensions.viewManager;

namespace robotlegs.bender.bundles
{
	public class UnityFallbackBundle : IBundle
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.Configure(typeof(UnityFallbackStageCrawlerConfig));
			context.Configure(typeof(FallbackContainerConfig));
		}
	}
}