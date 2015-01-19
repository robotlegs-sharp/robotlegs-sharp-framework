﻿using System;
using robotlegs.bender.framework.api;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	public class LifecycleTransition
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<LifecycleState> _fromStates = new List<LifecycleState>();

		private MessageDispatcher _dispatcher = new MessageDispatcher();

		private List<Action> _whenCallbacks = new List<Action>();
		private List<Action> _postCallbacks = new List<Action>();

		private List<Action> _callbacks = new List<Action>();

		private object _name;

		private Lifecycle _lifecycle;

		private LifecycleState _transitionState = LifecycleState.UNINITIALIZED;

		private LifecycleState _finalState = LifecycleState.UNINITIALIZED;

		private Action _preTransitionEvent;

		private Action _transitionEvent;

		private Action _postTransitionEvent;

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
		 * The events that the lifecycle will dispatch
		 * @param preTransitionEvent
		 * @param transitionEvent
		 * @param postTransitionEvent
		 * @return Self
		 */
		public LifecycleTransition WithEvents(Action preTransitionEvent, Action transitionEvent, Action postTransitionEvent)
		{
			_preTransitionEvent = preTransitionEvent;
			_transitionEvent = transitionEvent;
			_postTransitionEvent = postTransitionEvent;
			if (_reverse)
				_lifecycle.AddReversedEventTypes(preTransitionEvent, transitionEvent, postTransitionEvent);
			return this;
		}

		/**
		 * Reverse the dispatch order of this transition
		 * @return Self
		 */
		public LifecycleTransition InReverse()
		{
			_reverse = true;
			_lifecycle.AddReversedEventTypes(_preTransitionEvent, _transitionEvent, _postTransitionEvent);
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
		public LifecycleTransition AddBeforeHandler(MessageDispatcher.HandlerMessageDelegate handler)
		{
			_dispatcher.AddMessageHandler (_name, handler);
			return this;
		}
		public LifecycleTransition AddBeforeHandler(MessageDispatcher.HandlerMessageCallbackDelegate handler)
		{
			_dispatcher.AddMessageHandler (_name, handler);
			return this;
		}

		public LifecycleTransition AddWhenHandler(Action handler, bool once)
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

		public LifecycleTransition AddAfterHandler(Action handler, bool once)
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

		private void Dispatch(Action callback)
		{
			if (callback != null)
				callback();
//			if (type && _lifecycle.hasEventListener(type))
//				_lifecycle.dispatchEvent(new LifecycleEvent(type));
		}

		private void ReportError(object message, List<Action> callbacks = null)
		{
			// turn message into Error

			Exception error = message is Exception
				? message as Exception
				: new Exception(message.ToString());

			/*
			// dispatch error event if a listener exists, or throw
			if (_lifecycle.hasEventListener(LifecycleEvent.ERROR))
			{
				const event:LifecycleEvent = new LifecycleEvent(LifecycleEvent.ERROR, error);
				_lifecycle.dispatchEvent(event);
				// process callback queue
				if (callbacks)
				{
					for each (var callback:Function in callbacks)
						callback && safelyCallBack(callback, error, _name);
					callbacks.length = 0;
				}
			}
			else
			{
				// explode!
				throw error;
			}
			*/
		}

		/**
		 * Attempts to enter the transition
		 * @param callback Completion callback
		 */
		public void Enter(Action callback = null)
		{
			// immediately call back if we have already transitioned, and exit
			if (_lifecycle.state == _finalState)
			{
				if (callback != null)
					callback ();
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
				ReportError("Invalid transition", new List<Action>{callback});
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
//			/*
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
					Dispatch(_preTransitionEvent);
					Dispatch(_transitionEvent);

					ProcessCallbacks(_whenCallbacks);

					// put lifecycle into final state
					SetState(_finalState);

					// process callback queue (dup and trash for safety)
					Action[] callbacks = _callbacks.ToArray();
					_callbacks.Clear();
					foreach (Action callback2 in callbacks)
					{
						callback2();
//						safelyCallBack(callback, null, _name);
					}

					// dispatch post transition event
					Dispatch(_postTransitionEvent);

					ProcessCallbacks(_postCallbacks);

				}, _reverse);
//			*/
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
	}
}
