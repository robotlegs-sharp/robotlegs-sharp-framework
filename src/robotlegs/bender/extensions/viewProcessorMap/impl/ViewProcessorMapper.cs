using System;
using robotlegs.bender.extensions.viewProcessorMap.dsl;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.matching;
using System.Collections.Generic;

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

		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ViewProcessorMapper(ITypeFilter matcher, IViewProcessorViewHandler handler, ILogger logger = null)
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
			IViewProcessorMapping mapping = _mappings[processClassOrInstance];
			return mapping != null
				? OverwriteMapping(mapping, processClassOrInstance)
					: CreateMapping(processClassOrInstance);
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
			IViewProcessorMapping mapping = _mappings[processorClassOrInstance];
			if(mapping != null)
			{
				DeleteMapping(mapping);
			}
		}

		public void FromAll()
		{
			foreach (Object processor in _mappings)
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
			_mappings[processor] = mapping;
			if(_logger != null)
			{
				_logger.Debug("{0} mapped to {1}", _matcher, mapping);
			}
			return mapping;
		}

		private void DeleteMapping(IViewProcessorMapping mapping)
		{
			_handler.RemoveMapping(mapping);
			_mappings.Remove (mapping.Processor);
			//delete _mappings[mapping.Processor];
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

