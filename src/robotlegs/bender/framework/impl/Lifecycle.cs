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

		public event Action<Exception> ERROR;

		public event Action STATE_CHANGE;

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
			if (!Uninitialized)
				ReportError(LifecycleException.LATE_HANDLER_ERROR_MESSAGE);
			_initialize.AddBeforeHandler(callback);
			return this;
		}

		public ILifecycle WhenInitializing (Action handler)
		{
			if (Initialized)
				ReportError(LifecycleException.LATE_HANDLER_ERROR_MESSAGE);
			_initialize.AddWhenHandler (handler, true);
			return this;
		}

		public ILifecycle AfterInitializing (Action handler)
		{
			if (Initialized)
				ReportError(LifecycleException.LATE_HANDLER_ERROR_MESSAGE);
			_initialize.AddAfterHandler (handler, true);
			return this;
		}

		public ILifecycle BeforeSuspending (Action handler)
		{
			_suspend.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle WhenSuspending (Action handler)
		{
			_suspend.AddWhenHandler (handler, false);
			return this;
		}

		public ILifecycle AfterSuspending (Action handler)
		{
			_suspend.AddAfterHandler (handler, false);
			return this;
		}

		public ILifecycle BeforeResuming (Action handler)
		{
			_resume.AddBeforeHandler(handler);
			return this;
		}

		public ILifecycle WhenResuming (Action handler)
		{
			_resume.AddWhenHandler(handler, false);
			return this;
		}

		public ILifecycle AfterResuming (Action handler)
		{
			_resume.AddAfterHandler(handler, false);
			return this;
		}

		public ILifecycle BeforeDestroying (Action handler)
		{
			_destroy.AddBeforeHandler(handler);
			return this;
		}

		public ILifecycle WhenDestroying (Action handler)
		{
			_destroy.AddWhenHandler(handler, true);
			return this;
		}

		public ILifecycle AfterDestroying (Action handler)
		{
			_destroy.AddAfterHandler(handler, true);
			return this;
		}

		/*============================================================================*/
		/* Internal Functions                                                         */
		/*============================================================================*/

		internal void SetCurrentState(LifecycleState state)
		{
			if (_state == state)
				return;
			_state = state;
			if (STATE_CHANGE != null)
				STATE_CHANGE ();
		}

		internal bool HasErrorSubscriber()
		{
			return ERROR != null;
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

			_suspend = new LifecycleTransition("LifecycleEvent.PRE_SUSPEND", this)
				.FromStates(LifecycleState.ACTIVE)
				.ToStates(LifecycleState.SUSPENDING, LifecycleState.SUSPENDED)
				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize)
//				.WithEvents(LifecycleEvent.PRE_SUSPEND, LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND)
				.InReverse();

			_resume = new LifecycleTransition("LifecycleEvent.PRE_RESUME", this)
				.FromStates(LifecycleState.SUSPENDED)
				.ToStates(LifecycleState.RESUMING, LifecycleState.ACTIVE)
				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize);
//				.WithEvents(LifecycleEvent.PRE_RESUME, LifecycleEvent.RESUME, LifecycleEvent.POST_RESUME);

			_destroy = new LifecycleTransition("LifecycleEvent.PRE_DESTROY", this)
				.FromStates(LifecycleState.SUSPENDED, LifecycleState.ACTIVE)
				.ToStates(LifecycleState.DESTROYING, LifecycleState.DESTROYED)
				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize)
//				.WithEvents(LifecycleEvent.PRE_DESTROY, LifecycleEvent.DESTROY, LifecycleEvent.POST_DESTROY)
				.InReverse();
		}

		public void ReportError(Exception error)
		{
			if (HasErrorSubscriber ())
				ERROR (error);
			else
				throw error;
		}

		public void ReportError(string message)
		{
			ReportError (new LifecycleException (message));
		}
	}
}

