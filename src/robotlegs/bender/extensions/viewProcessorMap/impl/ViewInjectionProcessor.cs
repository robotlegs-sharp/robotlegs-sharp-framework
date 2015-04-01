//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	/// <summary>
	/// Default View Injection Processor implementation
	/// </summary>
	public class ViewInjectionProcessor
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		/*
			Dictionary param = weak keys.
			AS Docs on weak keys:
				Instructs the Dictionary object to use "weak" references on object keys. If the only reference to an object is in the specified Dictionary object, the key is eligible for garbage collection and is removed from the table when the object is collected. Note that the Dictionary never removes weak String keys from the table. Specifically in the case of String keys, the weak reference will never get removed from the key table, and the Dictionary will keep holding a strong reference to the respective values.

			TODO: Matt: Look into a C# weak dictionary
			C# equivilent = ConditionalWeakTable<TKey, TValue> ?
		*/
		//private _Dictionary<object, bool> _injectedObjects = new Dictionary<object, bool>(true);
		private Dictionary<object, bool> _injectedObjects = new Dictionary<object, bool>();

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Process(object view, Type type, IInjector injector)
		{
			if(!_injectedObjects.ContainsKey(view))
			{
				InjectAndRemember(view, injector);
			}
		}


		public void Unprocess(object view, Type type, IInjector injector)
		{
			// assumption is that teardown is not wanted.
			// if you *do* want teardown, copy this class
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void InjectAndRemember(object view, IInjector injector)
		{
			injector.InjectInto(view);
			_injectedObjects[view] = true;
		}
	}
}