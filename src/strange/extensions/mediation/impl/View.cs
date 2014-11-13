/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

/**
 * @class strange.extensions.mediation.impl.View
 * 
 * Parent class for all your Views. Extends MonoBehaviour.
 * Bubbles its Awake, Start and OnDestroy events to the
 * ContextView, which allows the Context to know when these
 * critical moments occur in the View lifecycle.
 */

using UnityEngine;
using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.impl;

namespace strange.extensions.mediation.impl
{
	public class View : MonoBehaviour, IView
	{
		public event Action<IView> RemoveView;

		protected virtual void Start ()
		{
			ContainerRegistry.HandleView(this, this.GetType());
		}

		protected virtual void OnDestroy ()
		{
			if (RemoveView != null)
				RemoveView(this);
		}
	}
}

