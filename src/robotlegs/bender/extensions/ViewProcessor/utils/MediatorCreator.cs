//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Utils
{
	/// <summary>
	/// Simple mediator creation process
	/// </summary>
	public class MediatorCreator
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Type _mediatorClass;

//		private Dictionary<>_createdMediatorsByView = new Dictionary<>(true);
		private Dictionary<object, object>_createdMediatorsByView = new Dictionary<object, object>();

 		/*============================================================================*/
 		/* Constructor                                                                */
 		/*============================================================================*/

		/// <summary>
		/// <p>Mediator Creator Processor</p>
		/// </summary>
		/// <param name="">The mediator class to create</param>
		public MediatorCreator(Type mediatorClass)
		{
			_mediatorClass = mediatorClass;
		}

 		/*============================================================================*/
 		/* Public Functions                                                           */
 		/*============================================================================*/

		public void Process(object view, Type type, IInjector injector)
 		{
			if (_createdMediatorsByView.ContainsKey(view))
			{
				return;
			}
			object mediator = injector.InstantiateUnmapped(_mediatorClass);
			_createdMediatorsByView.Add(view, mediator);
			InitializeMediator(view, mediator);
 		}

		public void Unprocess(object view, Type type, IInjector injector)
 		{
			if (_createdMediatorsByView.ContainsKey(view))
			{
				DestroyMediator(_createdMediatorsByView[view]);
				_createdMediatorsByView.Remove(view);
			}
 		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void InitializeMediator(object view, object mediator)
		{
			Type mediatorType = mediator.GetType();

			CallMethod("PreInitialize", mediatorType, mediator);
			SetFieldOrProperty("viewComponent", mediatorType, mediator, view);
			CallMethod("Initialize", mediatorType, mediator);
			CallMethod("PostInitialize", mediatorType, mediator);
		}

		private void DestroyMediator(object mediator)
		{
			Type mediatorType = mediator.GetType();

			CallMethod("PreDestroy", mediatorType, mediator);
			CallMethod("Destroy", mediatorType, mediator);
			SetFieldOrProperty("viewComponent", mediatorType, mediator, null);
			CallMethod("PostDestroy", mediatorType, mediator);
		}

		//TODO: Matt to James: This code has been copied from the MediatorManager. Should we wrap this up into a Util class somewhere?
		private bool CallMethod(string methodName, Type mediatorType, object instance)
		{
			MethodInfo methodInfo = mediatorType.GetMethod(methodName);
			if (methodInfo == null)
				return false;

			methodInfo.Invoke(instance, null);
			return true;
		}

		private object SetFieldOrProperty(string fieldName, Type mediatorType, object instance, object fieldValue)
		{
			FieldInfo fieldInfo = mediatorType.GetField(fieldName);
			if (fieldInfo == null)
			{
				PropertyInfo propertyInfo = mediatorType.GetProperty (fieldName);
				if (propertyInfo == null)
					return false;
				else
				{
					propertyInfo.SetValue (instance, fieldValue, null);
				}
			}
			else
			{
				fieldInfo.SetValue (instance, fieldValue);
			}
			return true;
		}
	}
}