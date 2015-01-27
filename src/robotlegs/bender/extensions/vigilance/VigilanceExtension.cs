using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using swiftsuspenders.mapping;
using swiftsuspenders.errors;

namespace robotlegs.bender.extensions.vigilance
{
	public class VigilanceExtension : IExtension, ILogTarget
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.AddLogTarget (this);
			context.injector.MappingOverride += MappingOverrideHandler;
		}

		public void Log (object source, robotlegs.bender.framework.impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			if (level <= LogLevel.WARN)
			{
				throw new VigilanceException(string.Format(message.ToString(), messageParameters));
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		void MappingOverrideHandler (MappingId mappingId, InjectionMapping instanceType)
		{
			throw new InjectorException("Injector mapping override for type " +
				mappingId.type + " with name " + mappingId.key);
		}
	}
}

