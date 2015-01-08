using System;
using robotlegs.bender.framework.api;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	public class Lifecycle : ILifecycle
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public LifecycleState state
		{
			get 
			{
				return _state;
			}
		}

		public object target {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool Uninitialized 
		{
			get 
			{
				return _state == LifecycleState.UNINITIALIZED;
			}
		}

		public bool Initialized 
		{
			get 
			{
				return _state != LifecycleState.UNINITIALIZED
					&& _state != LifecycleState.INITIALIZING;
			}
		}

		public bool Active 
		{
			get 
			{
				return _state == LifecycleState.ACTIVE;
			}
		}

		public bool Suspended 
		{
			get 
			{
				return _state == LifecycleState.SUSPENDED;
			}
		}

		public bool Destroyed 
		{
			get 
			{
				return _state == LifecycleState.DESTROYED;
			}
		}

		// James test event
		public event Action LifecycleEventPreInitialized;
		public event Action LifecycleEventInitialize;
		public event Action LifecycleEventPostInitialize;

		private void CallPreInitalized()
		{
			if (LifecycleEventPreInitialized != null)
				LifecycleEventPreInitialized ();
		}

		private void CallInitialize()
		{
			if (LifecycleEventInitialize != null)
				LifecycleEventInitialize ();
		}

		private void CallPostInitialize()
		{
			if (LifecycleEventPostInitialize != null)
				LifecycleEventPostInitialize ();
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private LifecycleState _state = LifecycleState.UNINITIALIZED;

		private Dictionary<Action, bool> _reversedEventTypes = new Dictionary<Action, bool> ();

		private int _reversePriority;

		private LifecycleTransition _initialize;

		private LifecycleTransition _suspend;

		private LifecycleTransition _resume;

		private LifecycleTransition _destroy;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/**
		* Creates a lifecycle for a given target object
			* @param target The target object
			*/
		public Lifecycle()
		{
//			_target = target;
//			_dispatcher = target as IEventDispatcher || new EventDispatcher(this);
			ConfigureTransitions();
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Initialize (Action callback = null)
		{
			_initialize.Enter (callback);
		}

		public void Suspend (Action callback = null)
		{
			_suspend.Enter (callback);
		}

		public void Resume (Action callback = null)
		{
			_resume.Enter (callback);
		}

		public void Destroy (Action callback = null)
		{
			_destroy.Enter (callback);
		}

		public ILifecycle BeforeInitializing (Action callback)
		{
//			if (!uninitialized)
//				reportError(LifecycleError.LATE_HANDLER_ERROR_MESSAGE);
			_initialize.AddBeforeHandler(callback);
			return this;
		}

		public ILifecycle WhenInitializing (Action handler)
		{
//			if (initialized)
//				reportError(LifecycleError.LATE_HANDLER_ERROR_MESSAGE);
			LifecycleEventInitialize += CreateSyncLifecycleListener (handler, true);
//			addEventListener(LifecycleEvent.INITIALIZE, createSyncLifecycleListener(handler, true));
			return this;
		}

		public ILifecycle AfterInitializing (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle BeforeSuspending (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle WhenSuspending (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle AfterSuspending (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle BeforeResuming (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle WhenResuming (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle AfterResuming (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle BeforeDestroying (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle WhenDestroying (Action handler)
		{
			throw new NotImplementedException ();
		}

		public ILifecycle AfterDestroying (Action handler)
		{
			throw new NotImplementedException ();
		}

		/*============================================================================*/
		/* Internal Functions                                                         */
		/*============================================================================*/

		public void SetCurrentState(LifecycleState state)
		{
			if (_state == state)
				return;
			_state = state;
//			dispatchEvent(new LifecycleEvent(LifecycleEvent.STATE_CHANGE));
		}

		public void AddReversedEventTypes(params Action[] types)
		{
			foreach (Action type in types)
			{
				_reversedEventTypes[type] = true;
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void ConfigureTransitions()
		{
			_initialize = new LifecycleTransition("LifecycleEvent.PRE_INITIALIZE", this)
				.FromStates(LifecycleState.UNINITIALIZED)
				.ToStates(LifecycleState.INITIALIZING, LifecycleState.ACTIVE)
				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize);

			/*
			_suspend = new LifecycleTransition(LifecycleEvent.PRE_SUSPEND, this)
				.FromStates(LifecycleState.ACTIVE)
				.ToStates(LifecycleState.SUSPENDING, LifecycleState.SUSPENDED)
				.WithEvents(LifecycleEvent.PRE_SUSPEND, LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND)
				.InReverse();

			_resume = new LifecycleTransition(LifecycleEvent.PRE_RESUME, this)
				.FromStates(LifecycleState.SUSPENDED)
				.ToStates(LifecycleState.RESUMING, LifecycleState.ACTIVE)
				.WithEvents(LifecycleEvent.PRE_RESUME, LifecycleEvent.RESUME, LifecycleEvent.POST_RESUME);

			_destroy = new LifecycleTransition(LifecycleEvent.PRE_DESTROY, this)
				.FromStates(LifecycleState.SUSPENDED, LifecycleState.ACTIVE)
				.ToStates(LifecycleState.DESTROYING, LifecycleState.DESTROYED)
				.WithEvents(LifecycleEvent.PRE_DESTROY, LifecycleEvent.DESTROY, LifecycleEvent.POST_DESTROY)
				.InReverse();
				*/
		}

		/*
		private int flipPriority(type:String, priority:int):int
		{
			return (priority == 0 && _reversedEventTypes[type])
				? _reversePriority++
					: priority;
		}
		*/

		private Action CreateSyncLifecycleListener(Action handler, bool once)
		{
			return delegate()
			{
				
//				once && IEventDispatcher(event.target)
//					.removeEventListener(event.type, arguments.callee);
				handler();
			};
		}

		/*
		private function reportError(message:String):void
		{
			const error:LifecycleError = new LifecycleError(message);
			if (hasEventListener(LifecycleEvent.ERROR))
			{
				const event:LifecycleEvent = new LifecycleEvent(LifecycleEvent.ERROR, error);
				dispatchEvent(event);
			}
			else
			{
				throw error;
			}
		}
		*/
	}
}

