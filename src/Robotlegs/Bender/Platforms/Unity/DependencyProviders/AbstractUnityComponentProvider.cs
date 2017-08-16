using Robotlegs.Bender.Extensions.ContextViews.API;
using SwiftSuspenders;
using SwiftSuspenders.DependencyProviders;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Robotlegs.Bender.DependencyProviders
{
	public abstract class AbstractUnityComponentProvider
	{	
		public event Action<DependencyProvider, object> PostApply
		{
			add
			{
				_postApply += value;
			}
			remove
			{
				_postApply -= value;
			}
		}

		public event Action<DependencyProvider, object> PreDestroy
		{
			add
			{
				_preDestroy += value;
			}
			remove
			{
				_preDestroy -= value;
			}
		}

		protected Type componentType;

		protected Transform parentObject;

		protected Component component;

		protected Action<DependencyProvider, object> _postApply;

		protected Action<DependencyProvider, object> _preDestroy;

		private bool destroyGameObjectWhenComplete;

		public AbstractUnityComponentProvider(Type componentType) : this(componentType, (Transform) null) { }

		public AbstractUnityComponentProvider(Type componentType, GameObject parentObject) : this(componentType, parentObject.transform) { }

		public AbstractUnityComponentProvider(Type componentType, Transform parentObject)
		{
			if (!componentType.IsSubclassOf(typeof(Component)))
				throw (new Exception(string.Format("The componentType {0} must be a Unity Component", componentType)));
			else
			{
				this.componentType = componentType;
				this.parentObject = parentObject == null ? null : parentObject.transform;
			}
		}

		public Component AddComponent(Injector activeInjector, Type componentType)
		{
			if (parentObject == null)
			{
				if (activeInjector.HasMapping(typeof(IContextView)))
				{
					IContextView contextViewObj = (IContextView) activeInjector.GetInstance(typeof(IContextView));
					object contextView = contextViewObj.view;
					if(contextView != null)
					{
						if (contextView is Transform) parentObject = (Transform) contextView;
						else if (contextView is GameObject) parentObject = ((GameObject) contextView).transform;
					}
				}
			}
			if (parentObject == null)
			{
				destroyGameObjectWhenComplete = true;
				parentObject = new GameObject("Unity Component Provider").transform;
			}
			component = parentObject.gameObject.AddComponent(componentType);
			return component;
		}

		public void RemoveComponent()
		{
			if (destroyGameObjectWhenComplete && parentObject != null)
			{
				GameObject.Destroy(parentObject.gameObject);
			}
			else
			{
                if(!Application.isPlaying)
                    Component.DestroyImmediate(component);
                else
                    Component.Destroy(component);
			}
			parentObject = null;
			component = null;
		}
	}
}
