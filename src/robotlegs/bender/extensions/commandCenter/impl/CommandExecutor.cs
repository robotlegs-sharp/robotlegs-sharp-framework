using System;
using robotlegs.bender.extensions.commandCenter.api;
using System.Collections.Generic;
using robotlegs.bender.framework.impl;
using System.Reflection;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.commandCenter.impl
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

		public void ExecuteCommands(List<ICommandMapping> mappings, CommandPayload payload = null)
		{
			int length = mappings.Count;
			for (int i = 0; i < length; i++)
			{
				ExecuteCommand(mappings[i], payload);
			}
		}

		public void ExecuteCommand(ICommandMapping mapping, CommandPayload payload = null)
		{
			bool hasPayload = payload !=null && payload.HasPayload();
			bool injectionEnabled = hasPayload && mapping.PayloadInjectionEnabled;
			object command = null;

			if (injectionEnabled)
				MapPayload(payload);
				
			if (mapping.Guards.Count == 0 || Guards.Approve(_injector, mapping.Guards.ToArray()))
			{
				Type commandClass = mapping.CommandClass;
				if (mapping.FireOnce && _removeMapping != null)
					_removeMapping (mapping);
				command = _injector.GetOrCreateNewInstance(commandClass);
				if (mapping.Hooks.Count > 0)
				{
					_injector.Map(commandClass).ToValue(command);
					Hooks.Apply(_injector, mapping.Hooks.ToArray());
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