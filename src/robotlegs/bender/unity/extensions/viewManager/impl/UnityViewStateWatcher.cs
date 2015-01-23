using UnityEngine;
using System;
using robotlegs.bender.extensions.viewManager.api;


namespace robotlegs.bender.unity.extensions.viewManager.impl
{
	public class UnityViewStateWatcher : MonoBehaviour, IViewStateWatcher
	{
		private bool _isAdded;
		private bool _hasBeenDisabled;
		public event Action<object> added;
		public event Action<object> removed;
		public event Action<object> disabled;
		#pragma warning disable 0108
		public event Action<object> enabled;
		#pragma warning restore 0108
		public GameObject target;
		
		public bool isAdded { get { return _isAdded; } }

		void Start()
		{
			_isAdded = true;
			if(this.added != null) this.added(target);
		}

		void OnEnable()
		{
			if(_hasBeenDisabled)
			{
				_hasBeenDisabled = false;
				if(this.enabled != null) this.enabled(target);
			}
		}

		void OnDisable()
		{
			_hasBeenDisabled = true;
			if(this.disabled != null) this.disabled(target);
		}

		void OnDestroy()
		{
			_isAdded = false;
			if(this.removed != null) this.removed(target);
		}
	}
}

