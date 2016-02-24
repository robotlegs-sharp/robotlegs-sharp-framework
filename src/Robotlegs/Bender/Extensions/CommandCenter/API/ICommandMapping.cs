//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.CommandCenter.API
{
	public interface ICommandMapping
	{/**
		 * The concrete Command Class for this mapping
		 */
		Type CommandClass {get;}
		
		/**
		 * The "execute" method to invoke on the Command instance
		 */

		string ExecuteMethod {get;}
		
		/**
		 * A list of Guards to query before execution
		 */
		List<object> Guards {get;}
		
		/**
		 * A list of Hooks to run during execution
		 */
		List<object> Hooks {get;}
		
		/**
		 * Unmaps a Command after a successful execution
		 */
		bool FireOnce {get;}
		
		/**
		 * Supply the payload values via instance injection
		 */
		bool PayloadInjectionEnabled {get;}
		
		/**
		 * The "execute" method to invoke on the Command instance
		 */
		ICommandMapping SetExecuteMethod(string name);
		
		/**
		 * A list of Guards to query before execution
		 */
		ICommandMapping AddGuards(params object[] guards);
		ICommandMapping AddGuards<T> ();
		ICommandMapping AddGuards<T1, T2> ();
		ICommandMapping AddGuards<T1, T2, T3> ();
		ICommandMapping AddGuards<T1, T2, T3, T4> ();
		ICommandMapping AddGuards<T1, T2, T3, T4, T5> ();
		
		/**
		 * A list of Hooks to run during execution
		 */
		ICommandMapping AddHooks(params object[] hooks);
		ICommandMapping AddHooks<T> ();
		ICommandMapping AddHooks<T1, T2> ();
		ICommandMapping AddHooks<T1, T2, T3> ();
		ICommandMapping AddHooks<T1, T2, T3, T4> ();
		ICommandMapping AddHooks<T1, T2, T3, T4, T5> ();
		
		/**
		 * Unmaps a Command after a successful execution
		 */
		ICommandMapping SetFireOnce(bool value);
		
		/**
		 * Supply the payload values via instance injection
		 */
		ICommandMapping SetPayloadInjectionEnabled(bool value);
	}
}