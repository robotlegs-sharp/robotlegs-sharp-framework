//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Framework.Impl
{
	public class Guards
	{
		public static bool Approve(IInjector injector, IEnumerable<object> guards)
		{
			object guardInstance;

			foreach (object guard in guards)
			{
				if (guard is Func<bool>)
				{
					if ((guard as Func<bool>)())
						continue;
					return false;
				}
				if (guard is Type)
				{
					if (injector != null)
						guardInstance = injector.InstantiateUnmapped(guard as Type);
					else
						guardInstance = Activator.CreateInstance(guard as Type);
				}
				else
					guardInstance = guard;

				MethodInfo approveMethod = guardInstance.GetType().GetMethod("Approve");
				if (approveMethod != null)
				{
					//Before we invoke Approve, inject any needed values
					if (injector != null && guardInstance.GetType().IsClass)
						injector.InjectInto(guardInstance);

					if ((bool)approveMethod.Invoke (guardInstance, null) == false)
						return false;
				}
				else
				{
					throw(new Exception (String.Format("Guard {0} is not a valid guard. It doesn't have the method 'Approve'", guardInstance)));
				}
			}
			return true;
		}

		public static bool Approve(IInjector injector, params object[] guards)
		{
			return Approve (injector, guards as IEnumerable<object>);
		}

		public static bool Approve(params object[] guards)
		{
			return Approve (null, guards as IEnumerable<object>);
		}

		public static bool Approve(IEnumerable<object> guards)
		{
			return Approve (null, guards);
		}

		public static bool Approve(params Func<bool>[] guards)
		{
			return Approve (null, guards as IEnumerable<object>);
		}

		public static bool Approve(IInjector injector, params Func<bool>[] guards)
		{
			return Approve (injector, guards as IEnumerable<object>);
		}

		public static bool Approve(IInjector injector, IEnumerable<Func<bool>> guards)
		{
			return Approve (injector, guards as IEnumerable<object>);
		}
	}
}

