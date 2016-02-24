using Robotlegs.Bender.Extensions.ContextView.API;
using swiftsuspenders;
using swiftsuspenders.dependencyproviders;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Robotlegs.Bender.DependencyProviders
{
    public class UnityComponentProvider : DependencyProvider
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

        private Action<DependencyProvider, object> _postApply;

        private Action<DependencyProvider, object> _preDestroy;

        private bool destroyGameObjectWhenComplete;

        private Type componentType;

        private Transform parentObject;

        private Component component;

        public UnityComponentProvider(Type componentType) : this(componentType, (Transform) null) { }

        public UnityComponentProvider(Type componentType, GameObject parentObject) : this(componentType, parentObject.transform) { }

        public UnityComponentProvider(Type componentType, Transform parentObject)
        {
            if (!componentType.IsSubclassOf(typeof(Component)))
                throw (new Exception(string.Format("The componentType {0} must be a Unity Component", componentType)));
            else
            {
                this.componentType = componentType;
                this.parentObject = parentObject == null ? null : parentObject.transform;
            }
        }

        public object Apply(Type targetType, Injector activeInjector, Dictionary<string, object> injectParameters)
        {
            if (parentObject == null)
            {
                object contextViewObj = (IContextView) activeInjector.GetInstance(typeof(IContextView));
                if(contextViewObj != null)
                {
                    object contextView = ((IContextView) contextViewObj).view;
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

            //TODO: Make auto InjectInto configurable
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
            if (destroyGameObjectWhenComplete && parentObject != null)
            {
                GameObject.Destroy(parentObject);
            }
            else
            {
                Component.Destroy(component);
            }
            parentObject = null;
            component = null;
        }
    }
}
