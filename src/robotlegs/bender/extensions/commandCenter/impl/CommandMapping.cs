//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.CommandCenter.API;


namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	public class CommandMapping : ICommandMapping
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/
		
		private Type _commandClass;
		
		/**
		 * @inheritDoc
		 */
		public Type CommandClass
		{
			get
			{
				return _commandClass;
			}
		}
		
		private string _executeMethod = "Execute";
		
		/**
		 * @inheritDoc
		 */
		public string ExecuteMethod
		{
			get
			{
				return _executeMethod;
			}
		}
		
		private List<object> _guards = new List<object>();
		
		/**
		 * @inheritDoc
		 */
		public List<object> Guards
		{
			get
			{
				return _guards;
			}
		}
		
		private List<object> _hooks = new List<object>();
		
		/**
		 * @inheritDoc
		 */
		public List<object> Hooks
		{
			get
			{
				return _hooks;
			}
		}
		
		private bool _fireOnce;
		
		/**
		 * @inheritDoc
		 */
		public bool FireOnce
		{
			get
			{
				return _fireOnce;
			}
		}
		
		private bool _payloadInjectionEnabled = true;
		
		/**
		 * @inheritDoc
		 */
		public bool PayloadInjectionEnabled
		{
			get
			{
				return _payloadInjectionEnabled;
			}
		}
		
		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		
		/**
		 * Creates a Command Mapping
		 * @param commandClass The concrete Command class
		 */
		public CommandMapping(Type commandClass)
		{
			_commandClass = commandClass;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/
		
		/**
		 * @inheritDoc
		 */
		public ICommandMapping SetExecuteMethod(string name)
		{
			_executeMethod = name;
			return this;
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandMapping AddGuards(params object[] guards)
		{
			_guards.AddRange(guards);
			return this;
		}

		public ICommandMapping AddGuards<T>()
		{
			return AddGuards (typeof(T));
		}

		public ICommandMapping AddGuards<T1, T2>()
		{
			return AddGuards (typeof(T1), typeof(T2));
		}

		public ICommandMapping AddGuards<T1, T2, T3>()
		{
			return AddGuards (typeof(T1), typeof(T2), typeof(T3));
		}

		public ICommandMapping AddGuards<T1, T2, T3, T4>()
		{
			return AddGuards (typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public ICommandMapping AddGuards<T1, T2, T3, T4, T5>()
		{
			return AddGuards (typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}

		public ICommandMapping AddHooks(params object[] hooks)
		{
			_hooks.AddRange(hooks);
			return this;
		}

		public ICommandMapping AddHooks<T>()
		{
			return AddHooks (typeof(T));
		}

		public ICommandMapping AddHooks<T1, T2>()
		{
			return AddHooks (typeof(T1), typeof(T2));
		}

		public ICommandMapping AddHooks<T1, T2, T3>()
		{
			return AddHooks (typeof(T1), typeof(T2), typeof(T3));
		}

		public ICommandMapping AddHooks<T1, T2, T3, T4>()
		{
			return AddHooks (typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public ICommandMapping AddHooks<T1, T2, T3, T4, T5>()
		{
			return AddHooks (typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandMapping SetFireOnce(bool value)
		{
			_fireOnce = value;
			return this;
		}
		
		/**
		 * @inheritDoc
		 */
		public ICommandMapping SetPayloadInjectionEnabled(bool value)
		{
			_payloadInjectionEnabled = value;
			return this;
		}
		
		public override string ToString()
		{
			return "Command " + _commandClass;
		}
	}
}

