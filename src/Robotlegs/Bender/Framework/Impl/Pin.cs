//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Framework.Impl
{
	/// <summary>
	/// Pins objects in memory
	/// </summary>
	public class Pin: IPinEvent
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> Detained
		{
			add
			{
				_detained += value;
			}
			remove
			{
				_detained -= value;
			}
		}

		public event Action<object> Released
		{
			add
			{
				_released += value;
			}
			remove
			{
				_released -= value;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private Dictionary<object, bool>_instances = new Dictionary<object, bool>();

		private Action<object> _detained;

		private Action<object> _released;
//		private IEventDispatcher _dispatcher;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/// <summary>
		/// Pin an object in memory
		/// </summary>
		/// <param name="instance">Instance to pin</param>
		public void Detain(object instance)
		{
			if (!_instances.ContainsKey(instance))
			{
				_instances[instance] = true;
				if (_detained != null)
					_detained (instance);
			}
		}

		/// <summary>
		/// Unpins an object
		/// </summary>
		/// <param name="instance">Instance to unpin</param>
		public void Release(object instance)
		{
			if (_instances.ContainsKey(instance))
			{
				_instances.Remove(instance);
				if (_released != null)
					_released (instance);
			}
		}

		/// <summary>
		/// Removes all pins
		/// </summary>
		public void ReleaseAll()
		{
			object[] instancesKeys = new object[_instances.Keys.Count];
			_instances.Keys.CopyTo (instancesKeys, 0);
			foreach (object instance in instancesKeys)
			{
				Release(instance);
			}
		}
	}
}

