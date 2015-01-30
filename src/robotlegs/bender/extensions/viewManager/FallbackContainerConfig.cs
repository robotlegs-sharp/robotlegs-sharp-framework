using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.viewManager
{
	public class FallbackContainerConfig : IConfig
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public ContainerRegistry registry;

		[Inject]
		public IViewManager viewManager;

		[Inject]
		public ILogger logger;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Configure ()
		{
			if (registry.FallbackBinding == null)
			{
				registry.SetFallbackContainer (new object ());
			}
			else
			{
				logger.Warn ("The fallback container can only be set once for the registry");
			}
			viewManager.AddContainer (registry.FallbackBinding.Container);
		}
	}
}

