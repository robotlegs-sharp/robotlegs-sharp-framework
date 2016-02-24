//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	public class CommandExecutor : ICommandExecutor
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public delegate void RemoveMappingDelegate(ICommandMapping CommandMapping);

		public delegate void HandleResultDelegate(object result, object command, ICommandMapping CommandMapping);

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;

		private RemoveMappingDelegate _removeMapping;

		private HandleResultDelegate _handleResult;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Creates a Command Executor
		/// </summary>
		/// <param name="injector">The Injector to use. A child injector will be created from it.</param>
		/// <param name="removeMapping">removeMapping Remove mapping handler (optional)</param>
		/// <param name="handleResult">handleResult Result handler (optional)</param>
		public CommandExecutor (IInjector injector, RemoveMappingDelegate removeMapping = null, HandleResultDelegate handleResult = null)
		{
			_injector = injector.CreateChild();
			_removeMapping = removeMapping;
			_handleResult = handleResult;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void ExecuteCommands(IEnumerable<ICommandMapping> mappings, CommandPayload payload = null)
		{
			foreach (ICommandMapping mapping in mappings) 
			{
				ExecuteCommand(mapping, payload);
			}
		}

		public void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = null)
		{
			bool hasPayload = payload !=null && payload.HasPayload();
			bool injectionEnabled = hasPayload && mapping.PayloadInjectionEnabled;
			object command = null;

			if (injectionEnabled)
				MapPayload(payload);
				
			if (mapping.Guards.Count == 0 || Guards.Approve(_injector, mapping.Guards))
			{
				Type commandClass = mapping.CommandClass;
				if (mapping.FireOnce && _removeMapping != null)
					_removeMapping (mapping);
				command = _injector.GetOrCreateNewInstance(commandClass);
				if (mapping.Hooks.Count > 0)
				{
					_injector.Map(commandClass).ToValue(command);
					Hooks.Apply(_injector, mapping.Hooks);
					_injector.Unmap(commandClass);
				}
			}

			if (injectionEnabled)
				UnmapPayload(payload);

			if (command != null && mapping.ExecuteMethod != null) 
			{
				MethodInfo executeMethod = command.GetType().GetMethod (mapping.ExecuteMethod);
				object result = (hasPayload && executeMethod.GetParameters ().Length > 0)
					? executeMethod.Invoke (command, payload.Values.ToArray ())
					: executeMethod.Invoke (command, null);
				if (_handleResult != null)
					_handleResult.Invoke (result, command, mapping);
			}
		}	

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void MapPayload(CommandPayload payload)
		{
			int i = (int)payload.length;
			while (i-- > 0)
			{
				_injector.Map (payload.Classes [i]).ToValue (payload.Values [i]);
			}
		}

		private void UnmapPayload(CommandPayload payload)
		{
			int i = (int)payload.length;
			while (i-- > 0)
			{
				_injector.Unmap(payload.Classes [i]);
			}
		}
	}
}