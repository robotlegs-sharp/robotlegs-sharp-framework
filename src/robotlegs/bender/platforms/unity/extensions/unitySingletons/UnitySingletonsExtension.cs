using UnityEngine;
using System.Collections;
using robotlegs.bender.framework.api;
using swiftsuspenders.mapping;
using robotlegs.bender.extensions.modularity.impl;
using System;
using swiftsuspenders.dependencyproviders;
using System.Collections.Generic;
using robotlegs.bender.platforms.unity.extensions.unitySingletons.impl;
using robotlegs.bender.extensions.contextview.api;

namespace robotlegs.bender.platforms.unity.extensions.unitySingletons
{
	public class UnitySingletonsExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;
		
		private SingletonFactory _singletonFactory;

		private UnitySingletons unitySingletons;
		
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
				unitySingletons = (contextView.view as Transform).gameObject.AddComponent<UnitySingletons>();
				unitySingletons.SetFactory(_singletonFactory);
			}
		}

		private void BeforeDestroying()
		{
			if (unitySingletons != null) 
			{
				GameObject.Destroy(unitySingletons);
			}
			_singletonFactory.Destroy();
		}
	}
}