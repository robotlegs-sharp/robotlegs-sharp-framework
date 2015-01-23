using UnityEngine;
using robotlegs.bender.extensions.contextview.api;
using System;


namespace robotlegs.bender.unity.extensions.contextviewstatewatcher
{
	public class UnityContextViewStateWatcher : MonoBehaviour, IContextViewStateWatcher
	{
		private bool _isAddedToStage;
		private bool _hasBeenDisabled;
		public event Action<object> addedToStage;
		public event Action<object> removeFromStage;
		public event Action<object> suspended;
		public event Action<object> resumed;
		public GameObject target;
		
		public bool isAddedToStage { get { return _isAddedToStage; } }

		void Start()
		{
			_isAddedToStage = true;
			if(addedToStage != null) addedToStage(target);
		}

		void OnEnable()
		{
			if(_hasBeenDisabled)
			{
				_hasBeenDisabled = false;
				if(resumed != null) resumed(target);
			}
		}

		void OnDisable()
		{
			_hasBeenDisabled = true;
			if(suspended != null) suspended(target);
		}

		void OnDestroy()
		{
			_isAddedToStage = false;
			if(removeFromStage != null) removeFromStage(target);
		}
	}
}

