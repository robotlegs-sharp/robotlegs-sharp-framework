//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using robotlegs.bender.framework.api;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	public class Hooks
	{
		public static void Apply(IInjector injector, params object[] hooks) 
		{
			Apply (injector, hooks as IEnumerable<object>);
		}

		public static void Apply(IInjector injector, IEnumerable<object> hooks)
		{
			object hookInstance;

			foreach (object hook in hooks)
			{
				if (hook is Action)
				{
					(hook as Action)();
					continue;
				}
				if (hook is Type)
				{
					if (injector != null)
						hookInstance = injector.InstantiateUnmapped(hook as Type);
					else
						hookInstance = Activator.CreateInstance(hook as Type);
				}
				else
					hookInstance = hook;

				MethodInfo hookMethod = hookInstance.GetType().GetMethod("Hook");
				if (hookMethod != null)
					hookMethod.Invoke (hookInstance, null);
				else
					throw new Exception ("Invalid hook to apply");
			}
		}

		public static void Apply(params object[] hooks)
		{
			Apply (hooks as IEnumerable<object>);
		}

		public static void Apply(IEnumerable<object> hooks)
		{
			Apply (null, hooks);
		}

		public static void Apply(IInjector injector, params Action[] hooks)
		{
			Apply (injector, hooks as IEnumerable<Action>);
		}

		public static void Apply(IInjector injector, IEnumerable<Action> hooks)
		{
			Apply (injector, hooks as IEnumerable<object>);
		}

		public static void Apply(params Action[] hooks)
		{
			Apply (null, hooks as IEnumerable<Action>);
		}

		public static void Apply(IEnumerable<Action> hooks)
		{
			Apply (null, hooks);
		}
	}
}

