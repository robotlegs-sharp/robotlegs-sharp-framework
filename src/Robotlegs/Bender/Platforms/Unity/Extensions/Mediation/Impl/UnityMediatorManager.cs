//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.Mediation.DSL;
using UnityEngine;


namespace Robotlegs.Bender.Platforms.Unity.Extensions.Mediation.Impl
{
	public class UnityMediatorManager : IMediatorManager
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

		private Dictionary<object, MediatorAttach> _viewMediatorAttachDictionary = new Dictionary<object, MediatorAttach> ();

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

			AddMediatorAttach (mediator, item);
		}
		
		public void RemoveMediator(object mediator, object item, IMediatorMapping mapping)
		{
			if (item is IView)
			{
				(item as IView).RemoveView -= HandleRemoveView;
			}

			RemoveMediatorAttach (mediator, item);

			DestroyMediator(mediator);
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void AddMediatorAttach(object mediator, object view)
		{
			if (view is Component) 
			{
				MediatorAttach mediatorAttach;
				if (!_viewMediatorAttachDictionary.ContainsKey(view))
				{
					mediatorAttach = (view as Component).gameObject.AddComponent<MediatorAttach>();
					mediatorAttach.SetView(view);
					_viewMediatorAttachDictionary.Add(view, mediatorAttach);
				}
				else
				{
					mediatorAttach = _viewMediatorAttachDictionary[view];
				}
				mediatorAttach.AddMediator(mediator);
			}
		}

		private void RemoveMediatorAttach(object mediator, object view)
		{
			if (!_viewMediatorAttachDictionary.ContainsKey (view)) 
				return;
			
			MediatorAttach mediatorAttach = _viewMediatorAttachDictionary[view];
			mediatorAttach.RemoveMediator (mediator);

			if (mediatorAttach.Mediators.Length == 0) 
			{
				if(!Application.isPlaying)
				    GameObject.DestroyImmediate (mediatorAttach);
                else
                    GameObject.Destroy (mediatorAttach);
				_viewMediatorAttachDictionary.Remove (view);
			}
		}
		
		private void HandleRemoveView (IView view)
		{
			if (_viewRemoved != null) 
			{
				_viewRemoved (view);
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
