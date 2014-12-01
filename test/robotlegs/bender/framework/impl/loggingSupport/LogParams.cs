using System;

namespace robotlegs.bender.framework.impl.loggingSupport
{
	public struct LogParams
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public object source;

		public LogLevel level;

		public DateTime timestamp;

		public object message;

		public object[] messageParameters;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LogParams(object source, LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			this.source = source;
			this.level = level;
			this.timestamp = timestamp;
			this.message = message;
			this.messageParameters = messageParameters;
		}
	}
}

