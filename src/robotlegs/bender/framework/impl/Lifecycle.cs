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

		public event Action<object> PRE_INITIALIZE;
		public event Action<object> INITIALIZE;
		public event Action<object> POST_INITIALIZE;

		public event Action<object> PRE_SUSPEND;
		public event Action<object> SUSPEND;
		public event Action<object> POST_SUSPEND;

		public event Action<object> PRE_RESUME;
		public event Action<object> RESUME;
		public event Action<object> POST_RESUME;

		public event Action<object> PRE_DESTROY;
		public event Action<object> DESTROY;
		public event Action<object> POST_DESTROY;

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

		public object Target
		{
			get 
			{
				return _target;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private object _target;

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
		public Lifecycle(object target)
		{
			_target = target;
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

		public ILifecycle BeforeInitializing (Action handler)
		{
			ReportIfNotUnitialized();
			_initialize.AddBeforeHandler(handler);
			return this;
		}

		public ILifecycle BeforeInitializing (MessageDispatcher.HandlerMessageDelegate handler)
		{
			ReportIfNotUnitialized();
			_initialize.AddBeforeHandler(handler);
			return this;
		}

		public ILifecycle BeforeInitializing (MessageDispatcher.HandlerMessageCallbackDelegate handler)
		{
			ReportIfNotUnitialized();
			_initialize.AddBeforeHandler(handler);
			return this;
		}

		public ILifecycle WhenInitializing (Action handler)
		{
			ReportIfInitialized ();
			_initialize.AddWhenHandler (handler, true);
			return this;
		}

		public ILifecycle AfterInitializing (Action handler)
		{
			ReportIfInitialized ();
			_initialize.AddAfterHandler (handler, true);
			return this;
		}

		public ILifecycle BeforeSuspending (Action handler)
		{
			_suspend.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeSuspending (MessageDispatcher.HandlerMessageDelegate handler)
		{
			_suspend.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeSuspending (MessageDispatcher.HandlerMessageCallbackDelegate handler)
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

		public ILifecycle BeforeResuming (MessageDispatcher.HandlerMessageDelegate handler)
		{
			_resume.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeResuming (MessageDispatcher.HandlerMessageCallbackDelegate handler)
		{
			_resume.AddBeforeHandler (handler);
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

		public ILifecycle BeforeDestroying (MessageDispatcher.HandlerMessageDelegate handler)
		{
			_destroy.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeDestroying (MessageDispatcher.HandlerMessageCallbackDelegate handler)
		{
			_destroy.AddBeforeHandler (handler);
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
			_initialize = new LifecycleTransition ("LifecycleEvent.PRE_INITIALIZE", this)
				.FromStates (LifecycleState.UNINITIALIZED)
				.ToStates (LifecycleState.INITIALIZING, LifecycleState.ACTIVE);
			_initialize.preTransition += DispatchPreInitialize;
			_initialize.transition += DispatchInitialize;
			_initialize.postTransition += DispatchPostInitialize;

			_suspend = new LifecycleTransition ("LifecycleEvent.PRE_SUSPEND", this)
				.FromStates (LifecycleState.ACTIVE)
				.ToStates (LifecycleState.SUSPENDING, LifecycleState.SUSPENDED)
				.InReverse();
			_suspend.preTransition += DispatchPreSuspend;
			_suspend.transition += DispatchSuspend;
			_suspend.postTransition += DispatchPostSuspend;

			_resume = new LifecycleTransition("LifecycleEvent.PRE_RESUME", this)
				.FromStates(LifecycleState.SUSPENDED)
				.ToStates(LifecycleState.RESUMING, LifecycleState.ACTIVE);
			_resume.preTransition += DispatchPreResume;
			_resume.transition += DispatchResume;
			_resume.postTransition += DispatchPostResume;

			_destroy = new LifecycleTransition("LifecycleEvent.PRE_DESTROY", this)
				.FromStates(LifecycleState.SUSPENDED, LifecycleState.ACTIVE)
				.ToStates(LifecycleState.DESTROYING, LifecycleState.DESTROYED)
				.InReverse();
			_destroy.preTransition += DispatchPreDestroy;
			_destroy.transition += DispatchDestroy;
			_destroy.postTransition += DispatchPostDestroy;
		}

		private void ReportIfNotUnitialized()
		{
			if (!Uninitialized)
				ReportError(LifecycleException.LATE_HANDLER_ERROR_MESSAGE);
		}

		private void ReportIfInitialized()
		{
			if (Initialized)
				ReportError(LifecycleException.LATE_HANDLER_ERROR_MESSAGE);
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
				PRE_INITIALIZE (_target);
		}

		private void DispatchInitialize()
		{
			if (INITIALIZE != null)
				INITIALIZE (_target);
		}

		private void DispatchPostInitialize()
		{
			if (POST_INITIALIZE != null)
				POST_INITIALIZE (_target);
		}

		private void DispatchPreSuspend()
		{
			if (PRE_SUSPEND != null)
				PRE_SUSPEND (_target);
		}

		private void DispatchSuspend()
		{
			if (SUSPEND != null)
				SUSPEND (_target);
		}

		private void DispatchPostSuspend()
		{
			if (POST_SUSPEND != null)
				POST_SUSPEND (_target);
		}

		private void DispatchPreResume()
		{
			if (PRE_RESUME != null)
				PRE_RESUME (_target);
		}

		private void DispatchResume()
		{
			if (RESUME != null)
				RESUME (_target);
		}

		private void DispatchPostResume()
		{
			if (POST_RESUME != null)
				POST_RESUME (_target);
		}

		private void DispatchPreDestroy()
		{
			if (PRE_DESTROY != null)
				PRE_DESTROY(_target);
		}

		private void DispatchDestroy()
		{
			if (DESTROY != null)
				DESTROY(_target);
		}

		private void DispatchPostDestroy()
		{
			if (POST_DESTROY != null)
				POST_DESTROY(_target);
		}
	}
}

