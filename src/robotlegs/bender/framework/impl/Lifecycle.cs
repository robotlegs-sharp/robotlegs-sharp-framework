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

		public event Action PRE_INITIALIZE;
		public event Action INITIALIZE;
		public event Action POST_INITIALIZE;

		public event Action PRE_SUSPEND;
		public event Action SUSPEND;
		public event Action POST_SUSPEND;

		public event Action PRE_RESUME;
		public event Action RESUME;
		public event Action POST_RESUME;

		public event Action PRE_DESTROY;
		public event Action DESTROY;
		public event Action POST_DESTROY;

		public LifecycleState state
		{
			get 
			{
				return _state;
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

		public void SetCurrentState(LifecycleState state)
		{
			if (_state == state)
				return;
			_state = state;
			if (STATE_CHANGE != null)
				STATE_CHANGE ();
		}

		public bool HasErrorSubscriber()
		{
			return ERROR != null;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void ConfigureTransitions()
		{
			_initialize = new LifecycleTransition ("LifecycleEvent.PRE_INITIALIZE", this)
				.FromStates (LifecycleState.UNINITIALIZED)
				.ToStates (LifecycleState.INITIALIZING, LifecycleState.ACTIVE);
//				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize);

			_initialize.preTransition += DispatchPreInitialize;
			_initialize.transition += DispatchInitialize;
			_initialize.postTransition += DispatchPostInitialize;

			_suspend = new LifecycleTransition ("LifecycleEvent.PRE_SUSPEND", this)
				.FromStates (LifecycleState.ACTIVE)
				.ToStates (LifecycleState.SUSPENDING, LifecycleState.SUSPENDED)
//				.WithEvents (CallPreInitalized, CallInitialize, CallPostInitialize)
//				.WithEvents(LifecycleEvent.PRE_SUSPEND, LifecycleEvent.SUSPEND, LifecycleEvent.POST_SUSPEND)
				.InReverse();

			_suspend.preTransition += DispatchPreSuspend;
			_suspend.transition += DispatchSuspend;
			_suspend.postTransition += DispatchPostSuspend;

//			_suspend.transition += 

			_resume = new LifecycleTransition("LifecycleEvent.PRE_RESUME", this)
				.FromStates(LifecycleState.SUSPENDED)
				.ToStates(LifecycleState.RESUMING, LifecycleState.ACTIVE);
//				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize);
//				.WithEvents(LifecycleEvent.PRE_RESUME, LifecycleEvent.RESUME, LifecycleEvent.POST_RESUME);


			_resume.preTransition += DispatchPreResume;
			_resume.transition += DispatchResume;
			_resume.postTransition += DispatchPostResume;

			_destroy = new LifecycleTransition("LifecycleEvent.PRE_DESTROY", this)
				.FromStates(LifecycleState.SUSPENDED, LifecycleState.ACTIVE)
				.ToStates(LifecycleState.DESTROYING, LifecycleState.DESTROYED)
//				.WithEvents(CallPreInitalized, CallInitialize, CallPostInitialize)
//				.WithEvents(LifecycleEvent.PRE_DESTROY, LifecycleEvent.DESTROY, LifecycleEvent.POST_DESTROY)
				.InReverse();

			_destroy.preTransition += DispatchPreDestroy;
			_destroy.transition += DispatchDestroy;
			_destroy.postTransition += DispatchPostDestroy;
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

		private void DispatchPreInitialize()
		{
			if (PRE_INITIALIZE != null)
				PRE_INITIALIZE ();
		}

		private void DispatchInitialize()
		{
			if (INITIALIZE != null)
				INITIALIZE ();
		}

		private void DispatchPostInitialize()
		{
			if (POST_INITIALIZE != null)
				POST_INITIALIZE ();
		}

		private void DispatchPreSuspend()
		{
			if (PRE_SUSPEND != null)
				PRE_SUSPEND ();
		}

		private void DispatchSuspend()
		{
			if (SUSPEND != null)
				SUSPEND ();
		}

		private void DispatchPostSuspend()
		{
			if (POST_SUSPEND != null)
				POST_SUSPEND ();
		}

		private void DispatchPreResume()
		{
			if (PRE_RESUME != null)
				PRE_RESUME ();
		}

		private void DispatchResume()
		{
			if (RESUME != null)
				RESUME ();
		}

		private void DispatchPostResume()
		{
			if (POST_RESUME != null)
				POST_RESUME ();
		}

		private void DispatchPreDestroy()
		{
			if (PRE_DESTROY != null)
				PRE_DESTROY();
		}

		private void DispatchDestroy()
		{
			if (DESTROY != null)
				DESTROY();
		}

		private void DispatchPostDestroy()
		{
			if (POST_DESTROY != null)
				POST_DESTROY();
		}
	}
}

