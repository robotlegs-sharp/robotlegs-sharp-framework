using System;
using robotlegs.bender.framework.api;
using strange.extensions.command.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.framework.context.api;

namespace strange.extensions.command
{
	public class CommandBinderDispatchConfig : IConfig
	{
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher { get;set; }

		[Inject]
		public ICommandBinder commandBinder { get;set; }

		public void Configure()
		{
			if (commandBinder is ITriggerable && dispatcher is ITriggerProvider) 
			{
				(dispatcher as ITriggerProvider).AddTriggerable(commandBinder as ITriggerable);
			}
		}
	}
}

