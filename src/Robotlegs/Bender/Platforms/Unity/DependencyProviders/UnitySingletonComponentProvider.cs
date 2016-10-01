using Robotlegs.Bender.Extensions.ContextViews.API;
using swiftsuspenders;
using swiftsuspenders.dependencyproviders;
using System;
using System.Collections.Generic;
using UnityEngine;
using swiftsuspenders.errors;

namespace Robotlegs.Bender.DependencyProviders
{
	public class UnitySingletonComponentProvider : AbstractUnityComponentProvider, DependencyProvider
	{
		public UnitySingletonComponentProvider(Type componentType) : base(componentType) { }

		public UnitySingletonComponentProvider(Type componentType, GameObject parentObject) : base(componentType, parentObject) { }

		public UnitySingletonComponentProvider(Type componentType, Transform parentObject) : base(componentType, parentObject) { }

		private bool _destroyed = false;

		public object Apply(Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			if (_destroyed)
				throw new InjectorException("Forbidden usage of unmapped singleton provider for type "
					+ targetType.ToString());

			if (component == null)
			{
				AddComponent(activeInjector, componentType);

				activeInjector.InjectInto(component);
			}

			if (_postApply != null)
			{
				_postApply(this, component);
			}
			return component;
		}

		public void Destroy()
		{
			if (_preDestroy != null)
			{
				_preDestroy(this, component);
			}
			_destroyed = true;
			RemoveComponent();
		}
	}
}
