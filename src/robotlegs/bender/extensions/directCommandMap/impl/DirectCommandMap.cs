using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.commandCenter.api;
using robotlegs.bender.extensions.commandCenter.impl;
using strange.extensions.injector.api;

namespace robotlegs.bender.extensions.directCommandMap.api
{
	public class DirectCommandMap : IDirectCommandMap
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<CommandMappingList.Processor> _mappingProcessors = new List<CommandMappingList.Processor>();

		private IContext _context;

		private ICommandExecutor _executor;

		private CommandMappingList _mappings;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
	
		public DirectCommandMap(IContext context)
		{
			_context = context;
			IInjectionBinder sandboxedInjector = context.injectionBinder.CreateChild();
			// allow access to this specific instance in the commands
			//sandboxedInjector.map(IDirectCommandMap).toValue(this);
			sandboxedInjector.Bind (typeof(IDirectCommandMap)).To (this);
			_mappings = new CommandMappingList(
				new NullCommandTrigger(), _mappingProcessors, context.GetLogger(this));
			_executor = new CommandExecutor(sandboxedInjector, _mappings.RemoveMapping);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IDirectCommandConfigurator Map (Type commandClass)
		{
			return new DirectCommandMapper(_executor, _mappings, commandClass);
		}

		public void Detain (object command)
		{
			_context.Detain (command);
		}

		public void Release (object command)
		{
			_context.Release (command);
		}

		public void Execute (robotlegs.bender.extensions.commandCenter.api.CommandPayload payload)
		{
			_executor.ExecuteCommands (_mappings.GetList(), payload);
		}

		public IDirectCommandMap AddMappingProcessor (CommandMappingList.Processor handler)
		{
			if (!_mappingProcessors.Contains (handler))
				_mappingProcessors.Add (handler);
			return this;
		}
	}
}

