//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager.Impl
{
	public class UnityViewStateWatcher : MonoBehaviour, IViewStateWatcher
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> added
		{
			add
			{
				_added += value;
			}
			remove 
			{
				_added -= value;
			}
		}

		public event Action<object> removed
		{
			add
			{
				_removed += value;
			}
			remove 
			{
				_removed -= value;
			}
		}

		public event Action<object> disabled
		{
			add
			{
				_disabled += value;
			}
			remove 
			{
				_disabled -= value;
			}
		}

		#pragma warning disable 0108
		public event Action<object> enabled
		{
			add
			{
				_enabled += value;
			}
			remove 
			{
				_enabled -= value;
			}
		}
		#pragma warning restore 0108

		public object target;

		public bool isAdded 
		{ 
			get 
			{ 
				return _isAdded; 
			} 
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<object> _added;

		private Action<object> _removed;

		private Action<object> _disabled;

		private Action<object> _enabled;

		private bool _isAdded;

		private bool _hasBeenDisabled;

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected virtual void Start()
		{
			_isAdded = true;
			if (_added != null)
			{
				_added (target);
			}
		}

		protected virtual void OnEnable()
		{
			if(_hasBeenDisabled)
			{
				_hasBeenDisabled = false;
				if (_enabled != null)
				{
					_enabled (target);
				}
			}
		}

		protected virtual void OnDisable()
		{
			_hasBeenDisabled = true;
			if (_disabled != null)
			{
				_disabled (target);
			}
		}

		protected virtual void OnDestroy()
		{
			_isAdded = false;
			if (_removed != null)
			{
				_removed (target);
			}
		}
	}
}

