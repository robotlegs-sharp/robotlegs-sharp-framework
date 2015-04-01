//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.mediatorMap.dsl;

namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorViewHandler : IMediatorViewHandler
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private List<IMediatorMapping> _mappings = new List<IMediatorMapping>();
		
		private Dictionary<Type, List<IMediatorMapping>> _knownMappings = new Dictionary<Type, List<IMediatorMapping>>();
		
		private IMediatorFactory _factory;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public MediatorViewHandler (IMediatorFactory factory)
		{
			_factory = factory;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void AddMapping(IMediatorMapping mapping)
		{
			if (_mappings.Contains(mapping))
				return;

			_mappings.Add(mapping);
			FlushCache();
		}

		public void RemoveMapping(IMediatorMapping mapping)
		{
			int index = _mappings.IndexOf(mapping);
			if (index == -1)
				return;

			_mappings.RemoveAt(index);
			FlushCache();
		}
		
		public void HandleView(object view, Type type)
		{
			List<IMediatorMapping> interestedMappings = GetInterestedMappingsFor(view, type);
			if (interestedMappings != null)
			{
				_factory.CreateMediators (view, type, interestedMappings);
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void FlushCache()
		{
			_knownMappings.Clear();
		}
		
		private List<IMediatorMapping> GetInterestedMappingsFor(object item, Type type)
		{
			if (!_knownMappings.ContainsKey(type))
			{
				_knownMappings[type] = new List<IMediatorMapping>();

				foreach (IMediatorMapping mapping in _mappings)
				{
					if (mapping.Matcher.Matches(item))
					{
						_knownMappings[type].Add(mapping);
					}
				}

				if (_knownMappings [type].Count == 0)
				{
					_knownMappings [type] = null;
				}
			}

			return _knownMappings[type];
		}
	}
}

