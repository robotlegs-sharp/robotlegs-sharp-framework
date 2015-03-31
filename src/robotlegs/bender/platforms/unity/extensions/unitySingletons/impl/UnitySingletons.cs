//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using UnityEngine;
using swiftsuspenders.mapping;

namespace robotlegs.bender.platforms.unity.extensions.unitySingletons.impl
{
	public class UnitySingletons : MonoBehaviour
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/
		
		public event Action<MappingId, object> AddedSingleton
		{
			add
			{
				_addedSingleton += value;
			}
			remove 
			{
				_addedSingleton -= value;
			}
		}
		
		public event Action<MappingId> RemovedSingleton
		{
			add
			{
				_removedSingeton += value;
			}
			remove 
			{
				_removedSingeton -= value;
			}
		}

		public SingletonFactory Factory
		{
			get
			{
				return _factory;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<MappingId, object> _addedSingleton;

		private Action<MappingId> _removedSingeton;

		private SingletonFactory _factory;
		
		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public void SetFactory(SingletonFactory factory)
		{
			if (_factory != null) 
			{
				_factory.AddedSingleton -= OnAddedSingleton;
				_factory.RemovedSingleton -= OnRemovedSingleton;
			}

			_factory = factory;
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void OnAddedSingleton(MappingId mappingId, object singleton)
		{
			if (_addedSingleton != null) 
			{
				_addedSingleton (mappingId, singleton);
			}
		}

		private void OnRemovedSingleton(MappingId mappingId)
		{
			if (_removedSingeton != null) 
			{
				_removedSingeton (mappingId);
			}
		}
	}
}
