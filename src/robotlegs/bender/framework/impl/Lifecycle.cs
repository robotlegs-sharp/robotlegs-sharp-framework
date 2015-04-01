//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class Lifecycle : ILifecycle
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<Exception> ERROR
		{
			add
			{
				_ERROR += value;
			}
			remove 
			{
				_ERROR -= value;
			}
		}

		public event Action STATE_CHANGE
		{
			add
			{
				_STATE_CHANGE += value;
			}
			remove 
			{
				_STATE_CHANGE -= value;
			}
		}

		public event Action<object> PRE_INITIALIZE
		{
			add
			{
				_PRE_INITIALIZE += value;
			}
			remove 
			{
				_PRE_INITIALIZE -= value;
			}
		}

		public event Action<object> INITIALIZE
		{
			add
			{
				_INITIALIZE += value;
			}
			remove 
			{
				_INITIALIZE -= value;
			}
		}

		public event Action<object> POST_INITIALIZE
		{
			add
			{
				_POST_INITIALIZE += value;
			}
			remove 
			{
				_POST_INITIALIZE -= value;
			}
		}

		public event Action<object> PRE_SUSPEND
		{
			add
			{
				_PRE_SUSPEND += value;
			}
			remove 
			{
				_PRE_SUSPEND -= value;
			}
		}

		public event Action<object> SUSPEND
		{
			add
			{
				_SUSPEND += value;
			}
			remove 
			{
				_SUSPEND -= value;
			}
		}

		public event Action<object> POST_SUSPEND
		{
			add
			{
				_POST_SUSPEND += value;
			}
			remove 
			{
				_POST_SUSPEND -= value;
			}
		}

		public event Action<object> PRE_RESUME
		{
			add
			{
				_PRE_RESUME += value;
			}
			remove 
			{
				_PRE_RESUME -= value;
			}
		}

		public event Action<object> RESUME
		{
			add
			{
				_RESUME += value;
			}
			remove 
			{
				_RESUME -= value;
			}
		}

		public event Action<object> POST_RESUME
		{
			add
			{
				_POST_RESUME += value;
			}
			remove 
			{
				_POST_RESUME -= value;
			}
		}

		public event Action<object> PRE_DESTROY
		{
			add
			{
				_PRE_DESTROY += value;
			}
			remove 
			{
				_PRE_DESTROY -= value;
			}
		}

		public event Action<object> DESTROY
		{
			add
			{
				_DESTROY += value;
			}
			remove 
			{
				_DESTROY -= value;
			}
		}

		public event Action<object> POST_DESTROY
		{
			add
			{
				_POST_DESTROY += value;
			}
			remove 
			{
				_POST_DESTROY -= value;
			}
		}

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

		private Action<Exception> _ERROR;

		private Action _STATE_CHANGE;

		private Action<object> _PRE_INITIALIZE;
		private Action<object> _INITIALIZE;
		private Action<object> _POST_INITIALIZE;

		private Action<object> _PRE_SUSPEND;
		private Action<object> _SUSPEND;
		private Action<object> _POST_SUSPEND;

		private Action<object> _PRE_RESUME;
		private Action<object> _RESUME;
		private Action<object> _POST_RESUME;

		private Action<object> _PRE_DESTROY;
		private Action<object> _DESTROY;
		private Action<object> _POST_DESTROY;

		private readonly object _target;

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

		public ILifecycle BeforeInitializing (HandlerMessageDelegate handler)
		{
			ReportIfNotUnitialized();
			_initialize.AddBeforeHandler(handler);
			return this;
		}

		public ILifecycle BeforeInitializing (HandlerMessageCallbackDelegate handler)
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

		public ILifecycle BeforeSuspending (HandlerMessageDelegate handler)
		{
			_suspend.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeSuspending (HandlerMessageCallbackDelegate handler)
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

		public ILifecycle BeforeResuming (HandlerMessageDelegate handler)
		{
			_resume.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeResuming (HandlerMessageCallbackDelegate handler)
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

		public ILifecycle BeforeDestroying (HandlerMessageDelegate handler)
		{
			_destroy.AddBeforeHandler (handler);
			return this;
		}

		public ILifecycle BeforeDestroying (HandlerMessageCallbackDelegate handler)
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
			if (_STATE_CHANGE != null)
				_STATE_CHANGE ();
		}

		internal bool HasErrorSubscriber()
		{
			return _ERROR != null;
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
				_ERROR (error);
			else
				throw error;
		}

		public void ReportError(string message)
		{
			ReportError (new LifecycleException (message));
		}

		private void DispatchPreInitialize()
		{
			if (_PRE_INITIALIZE != null)
				_PRE_INITIALIZE (_target);
		}

		private void DispatchInitialize()
		{
			if (_INITIALIZE != null)
				_INITIALIZE (_target);
		}

		private void DispatchPostInitialize()
		{
			if (_POST_INITIALIZE != null)
				_POST_INITIALIZE (_target);
		}

		private void DispatchPreSuspend()
		{
			if (_PRE_SUSPEND != null)
				_PRE_SUSPEND (_target);
		}

		private void DispatchSuspend()
		{
			if (_SUSPEND != null)
				_SUSPEND (_target);
		}

		private void DispatchPostSuspend()
		{
			if (_POST_SUSPEND != null)
				_POST_SUSPEND (_target);
		}

		private void DispatchPreResume()
		{
			if (_PRE_RESUME != null)
				_PRE_RESUME (_target);
		}

		private void DispatchResume()
		{
			if (_RESUME != null)
				_RESUME (_target);
		}

		private void DispatchPostResume()
		{
			if (_POST_RESUME != null)
				_POST_RESUME (_target);
		}

		private void DispatchPreDestroy()
		{
			if (_PRE_DESTROY != null)
				_PRE_DESTROY(_target);
		}

		private void DispatchDestroy()
		{
			if (_DESTROY != null)
				_DESTROY(_target);
		}

		private void DispatchPostDestroy()
		{
			if (_POST_DESTROY != null)
				_POST_DESTROY(_target);
		}
	}
}

