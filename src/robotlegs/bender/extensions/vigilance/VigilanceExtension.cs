using System;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;

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
			// TODO: Add mapping override to injector
			//context.injector.addEventListener (MappingEvent.MAPPING_OVERRIDE, mappingOverrideHandler);
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


	}
}

