using UnityEngine;
using System;
using robotlegs.bender.extensions.viewManager.api;


namespace robotlegs.bender.platforms.unity.extensions.viewManager.impl
{
	public class UnityViewStateWatcher : MonoBehaviour, IViewStateWatcher
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> added;

		public event Action<object> removed;

		public event Action<object> disabled;

		#pragma warning disable 0108
		public event Action<object> enabled;
		#pragma warning restore 0108

		public GameObject target;

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

		private bool _isAdded;

		private bool _hasBeenDisabled;

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected virtual void Start()
		{
			_isAdded = true;
			if (this.added != null)
			{
				this.added (target);
			}
		}

		protected virtual void OnEnable()
		{
			if(_hasBeenDisabled)
			{
				_hasBeenDisabled = false;
				if (this.enabled != null)
				{
					this.enabled (target);
				}
			}
		}

		protected virtual void OnDisable()
		{
			_hasBeenDisabled = true;
			if (this.disabled != null)
			{
				this.disabled (target);
			}
		}

		protected virtual void OnDestroy()
		{
			_isAdded = false;
			if (this.removed != null)
			{
				this.removed (target);
			}
		}
	}
}

