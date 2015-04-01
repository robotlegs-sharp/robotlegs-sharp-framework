//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Reflection;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.bundles.mvcs
{
	/// <summary>
	/// Classic Robotlegs mediator implementation
	/// 
	/// <p>Override initialize and destroy to hook into the mediator lifecycle.</p>
	/// </summary>
	public class Mediator : IMediator
	{
		/*============================================================================*/
		/* Private Properties                                                          */
		/*============================================================================*/
		
		private const string VIEW_DISPATCHER_NAME = "dispatcher";
		private IEventDispatcher viewDispatcher;
		private object _viewComponent;

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEventMap eventMap {get;set;}
		
		[Inject]
		public ILogger logger {get;set;}

		[Inject]
		public IEventDispatcher eventDispatcher {get;set;}

		public object viewComponent
		{
			set
			{
				_viewComponent = value;
				viewDispatcher = GetViewDispatcher(_viewComponent);
			}
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual void Initialize ()
		{

		}

		public virtual void Destroy ()
		{

		}

		/// <summary>
		/// Runs after the mediator has been destroyed
		/// Cleans up listeners mapped through the local EventMap
		/// </summary>
		public virtual void PostDestroy()
		{
			eventMap.UnmapListeners ();
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected virtual void AddViewListener(Enum type, Action listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.MapListener(viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void AddViewListener(Enum type, Action<IEvent> listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.MapListener(viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void AddViewListener<T>(Enum type, Action<T> listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.MapListener(viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void AddViewListener(Enum type, Delegate listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.MapListener (viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void AddContextListener(Enum type, Action listener, Type eventClass = null)
		{
			eventMap.MapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void AddContextListener(Enum type, Action<IEvent> listener, Type eventClass = null)
		{
			eventMap.MapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void AddContextListener<T>(Enum type, Action<T> listener, Type eventClass = null)
		{
			eventMap.MapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void RemoveViewListener(Enum type, Action listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.UnmapListener(viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void RemoveViewListener(Enum type, Action<IEvent> listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.UnmapListener(viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void RemoveViewListener<T>(Enum type, Action<T> listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.UnmapListener(viewDispatcher, type, listener, eventClass);
			}
		}

		protected virtual void RemoveViewListener(Enum type, Delegate listener, Type eventClass = null)
		{
			if (viewDispatcher == null)
			{
				TriggerViewDispatcherError ();
			}
			else
			{
				eventMap.UnmapListener(viewDispatcher, type, listener, eventClass);
			}
		}
			
		protected virtual void RemoveContextListener(Enum type, Action listener, Type eventClass = null)
		{
			eventMap.UnmapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void RemoveContextListener(Enum type, Action<IEvent> listener, Type eventClass = null)
		{
			eventMap.UnmapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void RemoveContextListener<T>(Enum type, Action<T> listener, Type eventClass = null)
		{
			eventMap.UnmapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void RemoveContextListener(Enum type, Delegate listener, Type eventClass = null)
		{
			eventMap.UnmapListener(eventDispatcher, type, listener, eventClass);
		}

		protected virtual void Dispatch(IEvent evt)
		{
			if (eventDispatcher.HasEventListener(evt.type))
			{
				eventDispatcher.Dispatch(evt);
			}
		}

		protected virtual IEventDispatcher GetViewDispatcher(object dispatcherObject)
		{
			if(dispatcherObject == null)
				return null;
			if(dispatcherObject is IEventDispatcher)
				return dispatcherObject as IEventDispatcher;

				// User reflection to find an IEventDispatcher property called 'dispatcher'
			object viewDispatcherValue;
			Type dispatcherObjectType = dispatcherObject.GetType();
			
			PropertyInfo propertyInfo = dispatcherObjectType.GetProperty(VIEW_DISPATCHER_NAME);
			if(propertyInfo == null)
			{
				FieldInfo fieldInfo = dispatcherObjectType.GetField(VIEW_DISPATCHER_NAME);
				if(fieldInfo != null)
				{
					viewDispatcherValue = fieldInfo.GetValue(dispatcherObject);
					if(viewDispatcherValue is IEventDispatcher)
						return viewDispatcherValue as IEventDispatcher;
				}
			}
			else
			{
				viewDispatcherValue = propertyInfo.GetValue(dispatcherObject, null);
				if(viewDispatcherValue is IEventDispatcher)
					return viewDispatcherValue as IEventDispatcher;
			}
			// View doesn't have a, and is not, a dispatcher. Lets continue without a view dispatcher for now.
			return null;
		}
		
		/*============================================================================*/
		/* Private Functions                                                        */
		/*============================================================================*/

		private void TriggerViewDispatcherError()
		{
			if(logger == null) return;
			if (_viewComponent == null) logger.Warn("{0}: Can't add or remove view listeners because the viewComponent has not been set.", this);
			else logger.Warn("{0}: Can't add or remove view listeners because {1} is not, and does not contain, an IEventDispatcher", this, _viewComponent);
		}
	}
}

