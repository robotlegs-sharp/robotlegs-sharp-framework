using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager.impl;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.contextview.api;

namespace robotlegs.bender.extensions.viewManager
{
	public class StageCrawlerExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ILogger _logger;

		private IInjector _injector;

		private ContainerRegistry _containerRegistry;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_injector = context.injector;
			_logger = context.GetLogger(this);
			context.AfterInitializing(AfterInitializing);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AfterInitializing()
		{
			_containerRegistry = _injector.GetInstance(typeof(ContainerRegistry)) as ContainerRegistry;
			if (!_injector.HasDirectMapping (typeof(IStageCrawler)))
			{
				_logger.Warn ("No CrawlerConfig configured. Make sure to configure a platform specific stage crawler config, or don't install the StageCrawler extension");
				return;
			}
			if(_injector.HasDirectMapping(typeof(IViewManager)))
			{
				ScanViewManagedContainers ();
			}
			else
			{
				ScanContextView();
			}
		}

		private void ScanViewManagedContainers()
		{
			_logger.Debug("ViewManager is installed. Checking for managed containers...");
			IViewManager viewManager = _injector.GetInstance (typeof(IViewManager)) as IViewManager;
			foreach (object container in viewManager.Containers)
			{
				// How to check if the view is on the stage?? Use a View watcher?
				//container.stage && ScanContainer(container);
				ScanContainer(container);
			}
		}


		private void ScanContextView()
		{
			_logger.Debug("ViewManager is not installed. Checking the ContextView...");
			IContextView contextView = _injector.GetInstance(typeof(IContextView)) as IContextView;
			// How to check if the contextView is on the stage?? Use a View watcher?
			//contextView.view.stage && ScanContainer(contextView.view);
			ScanContainer(contextView.view);
		}

		private void ScanContainer(object container)
		{
			ContainerBinding binding = _containerRegistry.GetBinding(container);
			_logger.Debug("StageCrawler scanning container {0} ...", container);
			IStageCrawler stageCrawler = _injector.GetInstance(typeof(IStageCrawler)) as IStageCrawler;
			stageCrawler.Binding = binding;
			stageCrawler.Scan (container);
			_logger.Debug("StageCrawler finished scanning {0}", container);
		}

	}
}

