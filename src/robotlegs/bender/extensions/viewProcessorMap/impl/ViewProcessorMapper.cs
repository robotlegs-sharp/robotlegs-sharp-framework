//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewProcessorMap.dsl;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class ViewProcessorMapper : IViewProcessorMapper, IViewProcessorUnmapper
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Dictionary<object, IViewProcessorMapping> _mappings = new Dictionary<object, IViewProcessorMapping>();

		private IViewProcessorViewHandler _handler;

		private ITypeFilter _matcher;

		private ILogging _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ViewProcessorMapper(ITypeFilter matcher, IViewProcessorViewHandler handler, ILogging logger = null)
		{
			_handler = handler;
			_matcher = matcher;
			_logger = logger;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IViewProcessorMappingConfig ToProcess(object processClassOrInstance)
		{
			if (_mappings.ContainsKey (processClassOrInstance))
				return OverwriteMapping (_mappings [processClassOrInstance], processClassOrInstance);
			else
				return CreateMapping(processClassOrInstance);
		}

		public IViewProcessorMappingConfig ToInjection()
		{
			return ToProcess(typeof(ViewInjectionProcessor));
		}

		public IViewProcessorMappingConfig ToNoProcess()
		{
			return ToProcess(typeof(NullProcessor));
		}

		public void FromProcess(object processorClassOrInstance)
		{
			if(_mappings.ContainsKey(processorClassOrInstance))
			{
				DeleteMapping(_mappings[processorClassOrInstance]);
			}
		}

		public void FromAll()
		{
			object[] mappingKeys = new object[_mappings.Keys.Count];
			_mappings.Keys.CopyTo(mappingKeys, 0);
			foreach (object processor in mappingKeys)
			{
				FromProcess(processor);
			}
		}

		public void FromNoProcess()
		{
			FromProcess(typeof(NullProcessor));
		}

		public void FromInjection()
		{
			FromProcess(typeof(ViewInjectionProcessor));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private ViewProcessorMapping CreateMapping(object processor)
		{
			ViewProcessorMapping mapping = new ViewProcessorMapping(_matcher, processor);
			_handler.AddMapping(mapping);
			_mappings[mapping.Processor == null ? mapping.ProcessorClass : mapping.Processor] = mapping;
			if(_logger != null)
			{
				_logger.Debug("{0} mapped to {1}", _matcher, mapping);
			}
			return mapping;
		}

		private void DeleteMapping(IViewProcessorMapping mapping)
		{
			_handler.RemoveMapping(mapping);
			_mappings.Remove(mapping.Processor == null ? mapping.ProcessorClass : mapping.Processor);
			if(_logger != null)
			{
				_logger.Debug("{0} unmapped from {1}", _matcher, mapping);
			}
		}

		private IViewProcessorMappingConfig OverwriteMapping(IViewProcessorMapping mapping, object processClassOrInstance)
		{
			if(_logger != null)
			{
				_logger.Warn("{0} is already mapped to {1}.\n" +
					"If you have overridden this mapping intentionally you can use 'unmap()' " +
					"prior to your replacement mapping in order to avoid seeing this message.\n",
					_matcher, mapping);
			}
			DeleteMapping(mapping);
			return CreateMapping(processClassOrInstance);
		}
	}
}

