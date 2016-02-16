using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender
{
	public class LogLevelTarget : ILogTarget
	{
		public event LogEventDelegate DEBUG;
		public event LogEventDelegate ERROR;
		public event LogEventDelegate FATAL;
		public event LogEventDelegate INFO;
		public event LogEventDelegate WARN;

		public delegate void LogEventDelegate(object source, object message, params object[] messageParameters);

		public void Log (object source, robotlegs.bender.framework.impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			switch (level)
			{
			case robotlegs.bender.framework.impl.LogLevel.DEBUG:
				if (DEBUG != null)
					DEBUG (source, message, messageParameters);
				break;
			case robotlegs.bender.framework.impl.LogLevel.ERROR:
				if (ERROR != null)
					ERROR (source, message, messageParameters);
				break;
			case robotlegs.bender.framework.impl.LogLevel.FATAL:
				if (FATAL != null)
					FATAL (source, message, messageParameters);
				break;
			case robotlegs.bender.framework.impl.LogLevel.INFO:
				if (INFO != null)
					INFO (source, message, messageParameters);
				break;
			case robotlegs.bender.framework.impl.LogLevel.WARN:
				if (WARN != null)
					WARN (source, message, messageParameters);
				break;
			}
		}
	}
}

