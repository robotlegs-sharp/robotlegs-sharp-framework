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
			if (registry.FallbackBinding != null) 
			{
				logger.Warn ("The fallback container has already been set in the registry");
			}

			viewManager.SetFallbackContainer (new object ());
		}
	}
}

