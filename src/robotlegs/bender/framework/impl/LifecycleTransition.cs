//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class LifecycleTransition
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Action preTransition;

		public Action transition;

		public Action postTransition;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<LifecycleState> _fromStates = new List<LifecycleState>();

		private MessageDispatcher _dispatcher = new MessageDispatcher();

		private List<Action> _whenCallbacks = new List<Action>();

		private List<Action> _postCallbacks = new List<Action>();

		private List<Delegate> _callbacks = new List<Delegate>();

		private object _name;

		private Lifecycle _lifecycle;

		private LifecycleState _transitionState = LifecycleState.DESTROYED;

		private LifecycleState _finalState = LifecycleState.DESTROYED;

		private bool _reverse;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LifecycleTransition (object name, Lifecycle lifecycle)
		{
			_name = name;
			_lifecycle = lifecycle;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/**
		* States that this transition is allowed to enter from
		* @param states Allowed states
		* @return Self
		*/
		public LifecycleTransition FromStates(params LifecycleState[] states)
		{
			foreach (LifecycleState state in states)
				_fromStates.Add(state);
			return this;
		}

		/**
		 * The states that this transition applies
		 * @param transitionState The state that the target is put into during the transition
		 * @param finalState The state that the target is put into after the transition
		 * @return
		 */
		public LifecycleTransition ToStates(LifecycleState transitionState, LifecycleState finalState)
		{
			_transitionState = transitionState;
			_finalState = finalState;
			return this;
		}

		/**
		 * Reverse the dispatch order of this transition
		 * @return Self
		 */
		public LifecycleTransition InReverse()
		{
			_reverse = true;
			return this;
		}

		/**
		 * A handler to run before the transition runs
		 * @param handler Possibly asynchronous before handler
		 * @return Self
		 */
		public LifecycleTransition AddBeforeHandler(Action handler)
		{
			_dispatcher.AddMessageHandler(_name, handler);
			return this;
		}
		public LifecycleTransition AddBeforeHandler(HandlerMessageDelegate handler)
		{
			_dispatcher.AddMessageHandler (_name, handler);
			return this;
		}
		public LifecycleTransition AddBeforeHandler(HandlerMessageCallbackDelegate handler)
		{
			_dispatcher.AddMessageHandler (_name, handler);
			return this;
		}

		public LifecycleTransition AddWhenHandler(Action handler, bool once = false)
		{
			if (once) 
				AddOnceHandler (_whenCallbacks, handler);
			else
				_whenCallbacks.Add (handler);
			return this;
		}

		public LifecycleTransition RemoveWhenHandler(Action handler)
		{
			_whenCallbacks.Remove (handler);
			return this;
		}

		public LifecycleTransition AddAfterHandler(Action handler, bool once = false)
		{
			if (once) 
				AddOnceHandler (_postCallbacks, handler);
			else
				_postCallbacks.Add (handler);
			return this;
		}

		public LifecycleTransition RemoveAfterHandler(Action handler)
		{
			_postCallbacks.Remove (handler);
			return this;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private bool InvalidTransition()
		{
			return _fromStates.Count > 0
				&& !_fromStates.Contains(_lifecycle.state);
		}

		private void SetState(LifecycleState state)
		{
//			state && _lifecycle.SetCurrentState(state);
			_lifecycle.SetCurrentState(state);
		}

		private void ReportError(object message, List<Delegate> callbacks = null)
		{
			// turn message into Error
			Exception exception = message is Exception
				? message as Exception
				: new Exception(message.ToString());

			// Report error throws an exception if we don't have a ERROR subscriber and if so
			// the callbacks will not get called
			_lifecycle.ReportError (exception); 

			if (callbacks != null)
				CallCallbacks (callbacks, exception);
		}

		/**
		 * Attempts to enter the transition
		 * @param callback Completion callback
		 */
		public void Enter()
		{
			Enter (null as Delegate);
		}

		public void Enter(Action callback)
		{
			Enter (callback as Delegate);
		}

		public void Enter(Action<Exception> callback)
		{
			Enter (callback as Delegate);
		}

		private void Enter(Delegate callback = null)
		{
			// immediately call back if we have already transitioned, and exit
			if (_lifecycle.state == _finalState)
			{
				CallCallback (callback);
				return;
			}

			// queue this callback if we are mid transition, and exit
			if (_lifecycle.state == _transitionState)
			{
				if (callback != null)
					_callbacks.Add(callback);
				return;
			}

			// report invalid transition, and exit
			if (InvalidTransition())
			{
				ReportError("Invalid transition", new List<Delegate>{callback});
				return;
			}

			// store the initial lifecycle state in case we need to roll back
			LifecycleState initialState = _lifecycle.state;

			// queue the first callback
			if (callback != null)
				_callbacks.Add(callback);

			// put lifecycle into transition state
			SetState(_transitionState);

			// run before handlers
			_dispatcher.DispatchMessage(_name, delegate(object error)
				{
					// revert state, report error, and exit
					if (error != null)
					{
						SetState(initialState);
						ReportError(error, _callbacks);
						return;
					}

					// dispatch pre transition and transition events
					if (preTransition != null)
						preTransition();

					ProcessCallbacks(_whenCallbacks);

					if (transition != null)
						transition();

					// put lifecycle into final state
					SetState(_finalState);

					// process callback queue (dup and trash for safety)
					CallCallbacks(_callbacks.ToArray());
					_callbacks.Clear();

					ProcessCallbacks(_postCallbacks);

					if (postTransition != null)
						postTransition();

				}, _reverse);
		}

		private void ProcessCallbacks(List<Action> callbacksList)
		{
			Action[] callbacksArray = callbacksList.ToArray();
			if (_reverse)
				Array.Reverse (callbacksArray);
			foreach (Action callback in callbacksArray)
				callback();
		}

		private void AddOnceHandler(List<Action> handlerList, Action handler)
		{
			Action onceHandler = null;
			onceHandler = delegate() {
				handlerList.Remove(onceHandler);
				handler();
			};
			handlerList.Add (onceHandler);
		}

		private void CallCallbacks(ICollection<Delegate> delegates, Exception exception = null)
		{
			foreach (Delegate del in delegates)
			{
				CallCallback (del, exception);
			}
		}

		private void CallCallback(Delegate del, Exception exception = null)
		{
			if (del == null)
				return;

			if (del is Action)
				(del as Action) ();
			else if (del is Action<Exception>)
				(del as Action<Exception>)(exception);
		}
	}
}

