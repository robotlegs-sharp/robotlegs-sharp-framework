//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.extensions.matching;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.mediatorMap.dsl;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorMapper : IMediatorMapper, IMediatorUnmapper
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private Dictionary<Type, IMediatorMapping> _mappings = new Dictionary<Type, IMediatorMapping>();
		
		private ITypeFilter _typeFilter;
		
		private IMediatorViewHandler _handler;
		
		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public MediatorMapper (ITypeFilter typeFilter, IMediatorViewHandler handler, ILogger logger)
		{
			_typeFilter = typeFilter;
			_handler = handler;
			_logger = logger;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IMediatorConfigurator ToMediator<T>()
		{
			return ToMediator (typeof(T));
		}
		
		public IMediatorConfigurator ToMediator(Type mediatorType)
		{
			if (_mappings.ContainsKey (mediatorType))
				return OverwriteMapping(_mappings[mediatorType]);
			return CreateMapping(mediatorType);
		}

		public void FromMediator<T>()
		{
			FromMediator (typeof(T));
		}
		
		public void FromMediator(Type mediatorType)
		{
			if (_mappings.ContainsKey (mediatorType))
			{
				DeleteMapping (_mappings [mediatorType]);
			}
		}
		 
		public void FromAll()
		{
			Dictionary<Type, IMediatorMapping>.ValueCollection values = _mappings.Values;
			IMediatorMapping[] mediatorMappings = new IMediatorMapping[values.Count];
			values.CopyTo(mediatorMappings, 0);
			foreach (IMediatorMapping mapping in mediatorMappings)
			{
				DeleteMapping(mapping);
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private MediatorMapping CreateMapping(Type mediatorType)
		{
			MediatorMapping mapping = new MediatorMapping (_typeFilter, mediatorType);
			_handler.AddMapping (mapping);
			_mappings [mediatorType] = mapping;
			if (_logger != null)
			{
				_logger.Debug ("{0} mapped to {1}", _typeFilter, mapping);
			}
			return mapping;
		}
		
		private void DeleteMapping(IMediatorMapping mapping)
		{
			_handler.RemoveMapping (mapping);
			_mappings.Remove (mapping.MediatorType);
			if (_logger != null)
			{
				_logger.Debug ("0} unmapped from {1}", _typeFilter, mapping);
			}
		}

		private IMediatorConfigurator OverwriteMapping(IMediatorMapping mapping)
		{
			if (_logger != null)
			{
				_logger.Warn ("{0} already mapped to {1}\n" +
				"If you have overridden this mapping intentionally you can use 'unmap()' " +
				"prior to your replacement mapping in order to avoid seeing this message.\n",
					_typeFilter, mapping);
			}
			DeleteMapping (mapping);
			return CreateMapping(mapping.MediatorType);
		}
	}
}

