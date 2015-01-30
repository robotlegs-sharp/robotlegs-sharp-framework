using robotlegs.bender.framework.api;
using System.Collections.Generic;
using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewProcessorMap.dsl;
using System.Reflection;
using robotlegs.bender.framework.impl;
using swiftsuspenders.errors;
using robotlegs.bender.extensions.viewProcessorMap.api;


namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class ViewProcessorFactory : IViewProcessorFactory
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		//private Dictionary<> _listenersByView = new Dictionary<>(true);
		private Dictionary<object, List<Action<IView>>> _listenersByView = new Dictionary<object, List<Action<IView>>>();

		private string ProcessMethodName	= "process";

		private string UnProcessMethodName	= "Unprocess";

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

		public void RunProcessors(object view, Type type, IViewProcessorMapping[] processorMappings)
		{
			if (view is IView)
			{
				CreateRemovedListener(view as IView, type, processorMappings);
			}

			ITypeFilter filter;

			foreach (IViewProcessorMapping mapping in processorMappings)
			{
				filter = mapping.Matcher;
				MapTypeForFilterBinding(filter, type, view);
				RunProcess(view, type, mapping);
				UnmapTypeForFilterBinding(filter, type, view);
			}
		}

		public void RunUnprocessors(object view, Type type, IViewProcessorMapping[] processorMappings)
		{
			foreach (IViewProcessorMapping mapping in processorMappings)
			{
				if (mapping.Processor == null)
				{
					mapping.Processor = CreateProcessor(mapping.ProcessorClass);
				}
				MethodInfo unProcessMethod = mapping.Processor.GetType().GetMethod(UnProcessMethodName);
				if (unProcessMethod != null && unProcessMethod.GetParameters().Length == 3)
				{
					unProcessMethod.Invoke(mapping.Processor, new object[3] {view, type, _injector});
				}
			}
		}

		public void RunAllUnprocessors()
		{
			foreach (List<Action<IView>> removalHandlers in _listenersByView.Values)
			{
				int iLength = removalHandlers.Count;
				for (int i = 0; i < iLength; i++)
				{
					removalHandlers[i].Invoke(null);
				}
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void RunProcess(object view, Type type, IViewProcessorMapping mapping)
		{
			if (Guards.Approve(mapping.Guards, _injector))
			{
				if (mapping.Processor == null)
				{
					mapping.Processor = CreateProcessor(mapping.ProcessorClass);
				}
				Hooks.Apply(mapping.Hooks, _injector);

				MethodInfo processMethod = mapping.Processor.GetType().GetMethod(ProcessMethodName);
				if (processMethod != null && processMethod.GetParameters().Length == 3)
				{
					processMethod.Invoke(mapping.Processor, new object[3] {view, type, _injector});
				}
			}
		}

		private object CreateProcessor(Type processorClass)
		{
			if (!_injector.HasMapping(processorClass))
			{
				_injector.Map(processorClass).AsSingleton();
			}

			try
			{
				return _injector.GetInstance(processorClass);
			}
			catch (InjectorInterfaceConstructionException exception)
			{
				String errorMsg = "The view processor "
				                  	+ processorClass.ToString ()
				                  	+ " has not been mapped in the injector, "
				                  	+ "and it is not possible to instantiate an interface. "
				                  	+ "Please map a concrete type against this interface."
									+ "Triggered from InjectorInterfaceConstructionException."
									+ exception.Message;
				throw(new ViewProcessorMapException(errorMsg));
			}
		}

		private void MapTypeForFilterBinding(ITypeFilter filter, Type type, object view)
		{
			List<Type> requiredTypes = RequiredTypesFor(filter, type);

			foreach (Type requiredType in requiredTypes)
			{
				_injector.Map(requiredType).ToValue(view);
			}
		}

		private void UnmapTypeForFilterBinding(ITypeFilter filter, Type type, object view)
		{
			List<Type> requiredTypes = RequiredTypesFor(filter, type);

			foreach (Type requiredType in requiredTypes)
			{
				if (_injector.HasDirectMapping(requiredType))
					_injector.Unmap(requiredType);
			}
		}

		private List<Type> RequiredTypesFor(ITypeFilter filter, Type type)
		{
			List<Type> requiredTypes = new List<Type> ();
			requiredTypes.AddRange (filter.AllOfTypes);
			requiredTypes.AddRange (filter.AnyOfTypes);

			if (requiredTypes.IndexOf(type) == -1)
				requiredTypes.Add(type);

			return requiredTypes;
		}

		private void CreateRemovedListener(IView view, Type type, IViewProcessorMapping[] processorMappings)
		{
			if (!_listenersByView.ContainsKey(view) || _listenersByView [view] == null)
			{
				_listenersByView[view] = new List<Action<IView>>();
			}

			Action<IView> handler = null;
			handler = delegate(IView iView)
			{
				RunUnprocessors(view, type, processorMappings);
				view.RemoveView -= handler;
				RemoveHandlerFromView(iView, handler);
			};

			_listenersByView[view].Add(handler);
			view.RemoveView += handler;
		}

		private void RemoveHandlerFromView(object view, Action<IView> handler)
		{
			if (_listenersByView[view] != null && _listenersByView[view].Count > 0)
			{
				_listenersByView[view].Remove(handler);
				if (_listenersByView[view].Count == 0)
				{
					_listenersByView.Remove(view);
				}
			}
		}
	}
}

