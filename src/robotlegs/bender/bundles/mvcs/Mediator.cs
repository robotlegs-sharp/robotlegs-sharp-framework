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
			eventMap.UnmapListeners ();
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected void AddViewListener(Enum type, Delegate listener)
		{
			eventMap.MapListener(_viewComponent as IEventDispatcher, type, listener);
		}

		protected void AddContextListener(Enum type, Delegate listener)
		{
			eventMap.MapListener(eventDispatcher, type, listener);
		}

		protected void RemoveViewListener(Enum type, Delegate listener)
		{
			eventMap.UnmapListener(_viewComponent as IEventDispatcher, type, listener);
		}

		protected void RemoveContextListener(Enum type, Delegate listener)
		{
			eventMap.UnmapListener(eventDispatcher, type, listener);
		}

		protected void Dispatch(IEvent evt)
		{
			if (eventDispatcher.HasEventListener(evt.type))
				eventDispatcher.Dispatch(evt);
		}
	}
}

