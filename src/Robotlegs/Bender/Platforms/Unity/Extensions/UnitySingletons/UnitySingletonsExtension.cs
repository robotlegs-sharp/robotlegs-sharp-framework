//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextViews.API;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Platforms.Unity.Extensions.UnitySingletons.Impl;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.UnitySingletons
{
	public class UnitySingletonsExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;
		
		private SingletonFactory _singletonFactory;

		private UnitySingletonsDisplay unitySingletons;
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			_injector = context.injector;

			_singletonFactory = new SingletonFactory (_injector);

			context.BeforeInitializing (BeforeInitializing);
			context.BeforeDestroying(BeforeDestroying);
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void BeforeInitializing()
		{
			if (_injector.HasDirectMapping(typeof(IContextView)))
			{
				IContextView contextView = _injector.GetInstance(typeof(IContextView)) as IContextView;
				unitySingletons = (contextView.view as Transform).gameObject.AddComponent<UnitySingletonsDisplay>();
				unitySingletons.SetFactory(_singletonFactory);
			}
		}

		private void BeforeDestroying()
		{
			if (unitySingletons != null) 
			{
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(unitySingletons);
                else
				    GameObject.Destroy(unitySingletons);
			}
			_singletonFactory.Destroy();
		}
	}
}