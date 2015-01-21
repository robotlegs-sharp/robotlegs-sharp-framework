using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.localEventMap.api;
using robotlegs.bender.framework.api;
using System.Reflection;

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

		protected virtual void AddViewListener(Enum type, Action listener)
		{
			if(viewDispatcher != null) eventMap.MapListener(viewDispatcher, type, listener);
		}
		protected virtual void AddViewListener(Enum type, Action<IEvent> listener)
		{
			if(viewDispatcher != null) eventMap.MapListener(viewDispatcher, type, listener);
		}
		protected virtual void AddViewListener<T>(Enum type, Action<T> listener)
		{
			if(viewDispatcher != null) eventMap.MapListener(viewDispatcher, type, listener);
		}
		protected virtual void AddViewListener(Enum type, Delegate listener)
		{
			if(viewDispatcher != null) eventMap.MapListener(viewDispatcher, type, listener);
		}

		protected virtual void AddContextListener(Enum type, Delegate listener)
		{
			eventMap.MapListener(eventDispatcher, type, listener);
		}

		protected virtual void RemoveViewListener(Enum type, Delegate listener)
		{
			if(viewDispatcher != null) eventMap.UnmapListener(viewDispatcher, type, listener);
		}

		protected virtual void RemoveContextListener(Enum type, Delegate listener)
		{
			eventMap.UnmapListener(eventDispatcher, type, listener);
		}

		protected virtual void Dispatch(IEvent evt)
		{
			if (eventDispatcher.HasEventListener(evt.type))
				eventDispatcher.Dispatch(evt);
		}

		protected virtual IEventDispatcher GetViewDispatcher(object dispatcherObject)
		{
			if(dispatcherObject == null) return null;
			if(dispatcherObject is IEventDispatcher) return dispatcherObject as IEventDispatcher;

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
					if(viewDispatcherValue is IEventDispatcher) return viewDispatcherValue as IEventDispatcher;
				}
			}
			else
			{
				viewDispatcherValue = propertyInfo.GetValue(dispatcherObject, null);
				if(viewDispatcherValue is IEventDispatcher) return viewDispatcherValue as IEventDispatcher;
			}

			if (logger != null) logger.Warn("{0}: Can't add or remove view listeners because set {0} is not, and does not contain, an IEventDispatcher", this, dispatcherObject);
			return null;
		}
	}
}

