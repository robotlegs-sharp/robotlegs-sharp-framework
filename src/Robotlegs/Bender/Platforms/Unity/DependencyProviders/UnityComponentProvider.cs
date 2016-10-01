using Robotlegs.Bender.Extensions.ContextViews.API;
using swiftsuspenders;
using swiftsuspenders.dependencyproviders;
using System;
using System.Collections.Generic;
using UnityEngine;
using swiftsuspenders.errors;

namespace Robotlegs.Bender.DependencyProviders
{
	public class UnityComponentProvider : AbstractUnityComponentProvider, DependencyProvider
	{
		public UnityComponentProvider(Type componentType) : base(componentType) { }

		public UnityComponentProvider(Type componentType, GameObject parentObject) : base(componentType, parentObject) { }

		public UnityComponentProvider(Type componentType, Transform parentObject) : base(componentType, parentObject) { }

		public object Apply(Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
		{
			AddComponent(activeInjector, componentType);

			activeInjector.InjectInto(component);

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
			RemoveComponent();
		}
	}
}