//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewProcessor.API;
using Robotlegs.Bender.Extensions.ViewProcessor.DSL;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using swiftsuspenders.errors;


namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public class ViewProcessorFactory : IViewProcessorFactory
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		//private Dictionary<> _listenersByView = new Dictionary<>(true);
		private Dictionary<IView, List<Action<IView>>> _listenersByView = new Dictionary<IView, List<Action<IView>>>();

		private const string ProcessMethodName	= "Process";

		private const string UnProcessMethodName = "Unprocess";

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

		public void RunProcessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings)
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

		public void RunUnprocessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings)
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
			IView[] viewsInQuestion = new IView[_listenersByView.Keys.Count];
			_listenersByView.Keys.CopyTo(viewsInQuestion, 0);
			foreach (IView viewInQuestion in viewsInQuestion)
			{
				List<Action<IView>> removalHandlers = _listenersByView [viewInQuestion];
				int iLength = removalHandlers.Count;
				for (int i = 0; i < iLength; i++)
				{
					removalHandlers[i].Invoke(viewInQuestion);
				}
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void RunProcess(object view, Type type, IViewProcessorMapping mapping)
		{
			if (Guards.Approve(_injector, mapping.Guards))
			{
				if (mapping.Processor == null)
				{
					mapping.Processor = CreateProcessor(mapping.ProcessorClass);
				}
				Hooks.Apply(_injector, mapping.Hooks);

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

		private void CreateRemovedListener(IView view, Type type, IEnumerable<IViewProcessorMapping> processorMappings)
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

		private void RemoveHandlerFromView(IView view, Action<IView> handler)
		{
			if (_listenersByView.ContainsKey(view) && _listenersByView[view].Count > 0)
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

