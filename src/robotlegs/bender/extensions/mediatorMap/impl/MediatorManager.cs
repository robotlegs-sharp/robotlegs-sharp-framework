//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Reflection;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.mediatorMap.dsl;

namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorManager : IMediatorManager
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> ViewRemoved
		{
			add
			{
				_viewRemoved += value;
			}
			remove 
			{
				_viewRemoved -= value;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<object> _viewRemoved;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/
		
		public void AddMediator(object mediator, object item, IMediatorMapping mapping)
		{
			if (item is IView && mapping.AutoRemoveEnabled)
			{
				(item as IView).RemoveView += HandleRemoveView;
			}

			InitializeMediator(mediator, item);
		}

		public void RemoveMediator(object mediator, object item, IMediatorMapping mapping)
		{
			if (item is IView)
			{
				(item as IView).RemoveView -= HandleRemoveView;
			}

			DestroyMediator(mediator);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/
		
		private void HandleRemoveView (IView view)
		{
			if (_viewRemoved != null)
			{
				_viewRemoved(view);
			}
		}

		private void InitializeMediator(object mediator, object mediatedItem)
		{
			Type mediatorType = mediator.GetType();

			CallMethod("PreInitialize", mediatorType, mediator);
			SetFieldOrProperty("viewComponent", mediatorType, mediator, mediatedItem);
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

