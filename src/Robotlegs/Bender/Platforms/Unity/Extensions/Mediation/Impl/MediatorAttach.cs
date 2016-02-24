//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.Mediation.Impl
{
	public class MediatorAttach : MonoBehaviour
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> AddedMediator
		{
			add
			{
				_addedMediator += value;
			}
			remove 
			{
				_addedMediator -= value;
			}
		}

		public event Action<object> RemovedMediator
		{
			add
			{
				_removedMediator += value;
			}
			remove 
			{
				_removedMediator -= value;
			}
		}

		public object[] Mediators
		{
			get
			{
				return _mediators;
			}
		}

		public object View
		{
			get
			{
				return _view;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<object> _addedMediator;

		private Action<object> _removedMediator;

		private object[] _mediators = new object[0];
		
		private object _view;

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		public void SetView(object view)
		{
			_view = view;
		}

		public void AddMediator(object mediator)
		{
			List<object> newMediators = new List<object> (_mediators);
			newMediators.Add (mediator);
			_mediators = newMediators.ToArray();
			if (_addedMediator != null) 
			{
				_addedMediator(mediator);
			}
		}

		public void RemoveMediator(object mediator)
		{
			List<object> newMediators = new List<object> (_mediators);
			if (newMediators.Remove (mediator)) 
			{
				_mediators = newMediators.ToArray();
				if (_removedMediator != null) 
				{
					_removedMediator(mediator);
				}
			}
		}
	}
}

