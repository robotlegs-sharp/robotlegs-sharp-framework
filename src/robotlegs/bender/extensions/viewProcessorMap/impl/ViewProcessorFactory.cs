using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class ViewProcessorFactory : IViewProcessorFactory
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		//private Dictionary<> _listenersByView = new Dictionary<>(true);
		private Dictionary<object, List<Action>> _listenersByView = new Dictionary<object, List<Action>>();

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ViewProcessorFactory(IInjector injector)
		{
			_injector = injector;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void RunProcessors(object view, Type type, object[] processorMappings):void
		{
			CreateRemovedListener(view, type, processorMappings);

			ITypeFilter filter;

			foreach (IViewProcessorMapping mapping in processorMappings)
			{
				filter = mapping.Matcher;
				MapTypeForFilterBinding(filter, type, view);
				RunProcess(view, type, mapping);
				unmapTypeForFilterBinding(filter, type, view);
			}
		}

		/**
		 * @inheritDoc
		 */
		public function runUnprocessors(view:Object, type:Class, processorMappings:Array):void
		{
			for each (var mapping:IViewProcessorMapping in processorMappings)
			{
				// ?? Is this correct - will assume that people are implementing something sensible in their processors.
				mapping.processor ||= createProcessor(mapping.processorClass);
				mapping.processor.unprocess(view, type, _injector);
			}
		}

		/**
		 * @inheritDoc
		 */
		public function runAllUnprocessors():void
		{
			for each (var removalHandlers:Array in _listenersByView)
			{
				const iLength:uint = removalHandlers.length;
				for (var i:uint = 0; i < iLength; i++)
				{
					removalHandlers[i](null);
				}
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private function runProcess(view:Object, type:Class, mapping:IViewProcessorMapping):void
		{
			if (guardsApprove(mapping.guards, _injector))
			{
				mapping.processor ||= createProcessor(mapping.processorClass);
				applyHooks(mapping.hooks, _injector);
				mapping.processor.process(view, type, _injector);
			}
		}

		private function createProcessor(processorClass:Class):Object
		{
			if (!_injector.hasMapping(processorClass))
			{
				_injector.map(processorClass).asSingleton();
			}

			try
			{
				return _injector.getInstance(processorClass);
			}
			catch (error:InjectorInterfaceConstructionError)
			{
				var errorMsg:String = "The view processor "
					+ processorClass
					+ " has not been mapped in the injector, "
					+ "and it is not possible to instantiate an interface. "
					+ "Please map a concrete type against this interface.";
				throw(new ViewProcessorMapError(errorMsg));
			}
			return null;
		}

		private function mapTypeForFilterBinding(filter:ITypeFilter, type:Class, view:Object):void
		{
			var requiredType:Class;
			const requiredTypes:Vector.<Class> = requiredTypesFor(filter, type);

			for each (requiredType in requiredTypes)
			{
				_injector.map(requiredType).toValue(view);
			}
		}

		private function unmapTypeForFilterBinding(filter:ITypeFilter, type:Class, view:Object):void
		{
			var requiredType:Class;
			const requiredTypes:Vector.<Class> = requiredTypesFor(filter, type);

			for each (requiredType in requiredTypes)
			{
				if (_injector.hasDirectMapping(requiredType))
					_injector.unmap(requiredType);
			}
		}

		private function requiredTypesFor(filter:ITypeFilter, type:Class):Vector.<Class>
		{
			const requiredTypes:Vector.<Class> = filter.allOfTypes.concat(filter.anyOfTypes);

			if (requiredTypes.indexOf(type) == -1)
				requiredTypes.push(type);

			return requiredTypes;
		}

		private function createRemovedListener(view:Object, type:Class, processorMappings:Array):void
		{
			if (view is DisplayObject)
			{
				_listenersByView[view] ||= [];

				const handler:Function = function(e:Event):void {
					runUnprocessors(view, type, processorMappings);
					(view as DisplayObject).removeEventListener(Event.REMOVED_FROM_STAGE, handler);
					removeHandlerFromView(view, handler);
				};

				_listenersByView[view].push(handler);
				(view as DisplayObject).addEventListener(Event.REMOVED_FROM_STAGE, handler, false, 0, true);
			}
		}

		private void RemoveHandlerFromView(object view, Action handler)
		{
			if (_listenersByView[view] && (_listenersByView[view].length > 0))
			{
				const handlerIndex:uint = _listenersByView[view].indexOf(handler);
				_listenersByView[view].splice(handlerIndex, 1);
				if (_listenersByView[view].length == 0)
				{
					delete _listenersByView[view];
				}
			}
		}
	}
}

