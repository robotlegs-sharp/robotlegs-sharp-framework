//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ViewManagement
{
	public class ViewManagerExtension : IExtension
	{
		/*============================================================================*/
		/* Private Static Properties                                                  */
		/*============================================================================*/

		// Really? Yes, there can be only one.
		private static ContainerRegistry _containerRegistry;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		private IViewManager _viewManager;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.WhenInitializing(WhenInitializing);
			context.WhenDestroying(WhenDestroying);

			_injector = context.injector;

			// Just one Container Registry
			if (_containerRegistry == null)
			{
				_containerRegistry = new ContainerRegistry ();
				ViewNotifier.SetRegistry (_containerRegistry);
			}
			_injector.Map(typeof(ContainerRegistry)).ToValue(_containerRegistry);
			if(_injector.HasDirectMapping(typeof(IParentFinder)))
			{
				_injector.Unmap (typeof(IParentFinder));
			}
			_injector.Map(typeof(IParentFinder)).ToValue(_containerRegistry);

			// But you get your own View Manager
			_injector.Map(typeof(IViewManager)).ToSingleton(typeof(Impl.ViewManager));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void WhenInitializing()
		{
			_viewManager = _injector.GetInstance(typeof(IViewManager)) as IViewManager;
		}

		private void WhenDestroying()
		{
			_viewManager.RemoveAllHandlers();
			_injector.Unmap(typeof(IViewManager));
		}
	}
}

