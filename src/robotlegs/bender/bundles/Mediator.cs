using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.localEventMap.api;

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
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEventMap eventMap {get;set;}

		[Inject]
		public IEventDispatcher eventDispatcher {get;set;}

		private object _viewComponent;

		public object viewComponent
		{
			set
			{
				_viewComponent = value;
			}
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Initialize ()
		{

		}

		public void Destroy ()
		{

		}

		/// <summary>
		/// Runs after the mediator has been destroyed
		/// Cleans up listeners mapped through the local EventMap
		/// </summary>
		public void PostDestroy()
		{

		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

//		protected AddViewListener(eventString:String, listener:Function, eventClass:Class = null):void
//		{
//			eventMap.mapListener(IEventDispatcher(_viewComponent), eventString, listener, eventClass);
//		}
//
//		protected AddContextListener(eventString:String, listener:Function, eventClass:Class = null):void
//		{
//			eventMap.mapListener(eventDispatcher, eventString, listener, eventClass);
//		}
//
//		protected RemoveViewListener(eventString:String, listener:Function, eventClass:Class = null):void
//		{
//			eventMap.unmapListener(IEventDispatcher(_viewComponent), eventString, listener, eventClass);
//		}
//
//		protected RemoveContextListener(eventString:String, listener:Function, eventClass:Class = null):void
//		{
//			eventMap.unmapListener(eventDispatcher, eventString, listener, eventClass);
//		}
//
//		protected Dispatch(IEvent evt):void
//		{
//			if (evt.hasEventListener(event.type))
//				evt.dispatchEvent(event);
//		}
	}
}

