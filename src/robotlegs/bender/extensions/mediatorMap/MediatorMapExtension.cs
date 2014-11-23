//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.mediatorMap.impl;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.mediatorMap
{
	public class MediatorMapExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private IInjector _injector;
		
		private MediatorMap _mediatorMap;
		
		private IViewManager _viewManager;
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			_injector = context.injector;

			// TODO: Make the injection binder work on the next line
//			_injector.Bind<IMediatorMap>().To<MediatorMap>().ToSingleton();
//			UnityEngine.Debug.Log(_injector.GetBinding<IMediatorMap>());

			_mediatorMap = new MediatorMap(context);
			_injector.Map(typeof(IMediatorMap)).ToValue(_mediatorMap);
			
			//TODO: Add when destroying to Context
			context.AddPreInitializedCallback(BeforeInitializing)
				.AddPreDestroyCallback(BeforeDestroying);
			//.AddWhenDestroyingCallback(WhenDestroying);
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void BeforeInitializing()
		{
//			_mediatorMap = _injector.GetInstance<IMediatorMap>() as MediatorMap;
			_viewManager = _injector.GetInstance(typeof(IViewManager)) as IViewManager;
			if (_viewManager != null)
				_viewManager.AddViewHandler(_mediatorMap);
		}
		
		private void BeforeDestroying()
		{
			//TODO: Satify directly
			// if (_injector.satisfiesDirectly(IViewManager)
			_mediatorMap.UnmediateAll();
			if (_viewManager != null)
				_viewManager.RemoveViewHandler(_mediatorMap);
		}
		
		private void WhenDestroying()
		{
			//TODO: Satify directly
			// if (_injector.satisfiesDirectly(IMediatorMap)
			_injector.Unmap(typeof(IMediatorMap));
		}
	}
}

