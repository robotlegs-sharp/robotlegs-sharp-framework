using System;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.Modularity.Impl
{
	public class LogLevelTarget : ILogTarget
	{
		public event LogEventDelegate DEBUG;
		public event LogEventDelegate ERROR;
		public event LogEventDelegate FATAL;
		public event LogEventDelegate INFO;
		public event LogEventDelegate WARN;

		public delegate void LogEventDelegate(object source, object message, params object[] messageParameters);

		public void Log (object source, Robotlegs.Bender.Framework.Impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			switch (level)
			{
			case Robotlegs.Bender.Framework.Impl.LogLevel.DEBUG:
				if (DEBUG != null)
					DEBUG (source, message, messageParameters);
				break;
			case Robotlegs.Bender.Framework.Impl.LogLevel.ERROR:
				if (ERROR != null)
					ERROR (source, message, messageParameters);
				break;
			case Robotlegs.Bender.Framework.Impl.LogLevel.FATAL:
				if (FATAL != null)
					FATAL (source, message, messageParameters);
				break;
			case Robotlegs.Bender.Framework.Impl.LogLevel.INFO:
				if (INFO != null)
					INFO (source, message, messageParameters);
				break;
			case Robotlegs.Bender.Framework.Impl.LogLevel.WARN:
				if (WARN != null)
					WARN (source, message, messageParameters);
				break;
			}
		}
	}
}

