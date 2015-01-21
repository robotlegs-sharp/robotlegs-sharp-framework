using System;
using robotlegs.bender.framework.api;
using swiftsuspenders.mapping;

namespace robotlegs.bender.extensions.enhancedLogging.impl
{
	public class InjectorListener
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/**
		* Creates an Injector Listener
		* @param injector Injector
		* @param logger Logger
		*/
		public InjectorListener(IInjector injector, ILogger logger)
		{
			_injector = injector;
			_logger = logger;
			AddListeners();
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/**
		 * Destroys this listener
		 */
		public void Destroy()
		{
			_injector.POST_CONSTRUCT -= OnPostConstruct;
			_injector.POST_INSTANTIATE -= OnPostInstantiate;
			_injector.PRE_CONSTRUCT -= OnPreConstruct;

			_injector.MAPPING_OVERRIDE -= OnMappingOverride;
			_injector.POST_MAPPING_CHANGE -= OnPostMappingChange;
			_injector.POST_MAPPING_CREATE -= OnPostMappingCreate;
			_injector.POST_MAPPING_REMOVE -= OnPostMappingRemove;
			_injector.PRE_MAPPING_CHANGE -= OnPreMappingChange;
			_injector.PRE_MAPPING_CREATE -= OnPreMappingCreate;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AddListeners()
		{
			_injector.POST_CONSTRUCT += OnPostConstruct;
			_injector.POST_INSTANTIATE += OnPostInstantiate;
			_injector.PRE_CONSTRUCT += OnPreConstruct;

			_injector.MAPPING_OVERRIDE += OnMappingOverride;
			_injector.POST_MAPPING_CHANGE += OnPostMappingChange;
			_injector.POST_MAPPING_CREATE += OnPostMappingCreate;
			_injector.POST_MAPPING_REMOVE += OnPostMappingRemove;
			_injector.PRE_MAPPING_CHANGE += OnPreMappingChange;
			_injector.PRE_MAPPING_CREATE += OnPreMappingCreate;
		}

		private void OnPreConstruct (object instance, Type instanceType)
		{
			_logger.Debug ("Injection event PRE_CONSTRUCT. Instance: {1}. Instance type: {2}", instance, instanceType);
		}

		private void OnPostConstruct (object instance, Type instanceType)
		{
			_logger.Debug ("Injection event POST_CONSTRUCT. Instance: {1}. Instance type: {2}", instance, instanceType);
		}

		private void OnPostInstantiate (object instance, Type instanceType)
		{
			_logger.Debug ("Injection event POST_INSTANTIATE. Instance: {1}. Instance type: {2}", instance, instanceType);
		}

		private void OnMappingOverride(MappingId mappingId, InjectionMapping instanceType)
		{
			_logger.Debug("Mapping event MAPPING_OVERRIDE. Mapped type: {1}. Mapped name: {2}",
				mappingId.type, mappingId.key);
		}

		private void OnPostMappingChange(MappingId mappingId, InjectionMapping instanceType)
		{
			_logger.Debug("Mapping event POST_MAPPING_CHANGE. Mapped type: {1}. Mapped name: {2}",
				mappingId.type, mappingId.key);
		}

		private void OnPostMappingCreate(MappingId mappingId, InjectionMapping instanceType)
		{
			_logger.Debug("Mapping event POST_MAPPING_CREATE. Mapped type: {1}. Mapped name: {2}",
				mappingId.type, mappingId.key);
		}

		private void OnPostMappingRemove (MappingId mappingId)
		{
			_logger.Debug("Mapping event MAPPING_OVERRIDE. Mapped type: {1}. Mapped name: {2}",
				mappingId.type, mappingId.key);
		}

		private void OnPreMappingChange(MappingId mappingId, InjectionMapping instanceType)
		{
			_logger.Debug("Mapping event PRE_MAPPING_CHANGE. Mapped type: {1}. Mapped name: {2}",
				mappingId.type, mappingId.key);
		}

		private void OnPreMappingCreate (MappingId mappingId)
		{
			_logger.Debug("Mapping event MAPPING_OVERRIDE. Mapped type: {1}. Mapped name: {2}",
				mappingId.type, mappingId.key);
		}
	}
}

