using System;
using robotlegs.bender.framework.api;

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
//			var type:String;
//			for each (type in INJECTION_TYPES)
//			{
//				_injector.removeEventListener(type, onInjectionEvent);
//			}
//			for each (type in MAPPING_TYPES)
//			{
//				_injector.removeEventListener(type, onMappingEvent);
//			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AddListeners()
		{
//			var type:String;
//			for each (type in INJECTION_TYPES)
//			{
//				_injector.addEventListener(type, onInjectionEvent);
//			}
//			for each (type in MAPPING_TYPES)
//			{
//				_injector.addEventListener(type, onMappingEvent);
//			}
		}

//		private function onInjectionEvent(event:InjectionEvent):void
//		{
//			_logger.debug("Injection event of type {0}. Instance: {1}. Instance type: {2}",
//				[event.type, event.instance, event.instanceType]);
//		}
//
//		private function onMappingEvent(event:MappingEvent):void
//		{
//			_logger.debug("Mapping event of type {0}. Mapped type: {1}. Mapped name: {2}",
//				[event.type, event.mappedType, event.mappedName]);
//		}
	}
}

