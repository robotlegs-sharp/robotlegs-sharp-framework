//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	/// <summary>
	/// The log manager creates loggers and is itself a log target
	/// </summary>
	public class LogManager : ILogTarget
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private List<ILogTarget> _targets = new List<ILogTarget>();

		/*============================================================================*/
		/* Public Properties                                                        */
		/*============================================================================*/

		public LogLevel logLevel = LogLevel.DEBUG;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/// <summary>
		/// Retrieves a logger for a given source
		/// </summary>
		/// <returns>Logger</returns>
		/// <param name="source">Logging source</param>
		public ILogging GetLogger(object source)
		{
			return new Logger(source, this);
		}

		/// <summary>
		/// Adds a custom log target
		/// </summary>
		/// <param name="target">Log target</param>
		public void AddLogTarget(ILogTarget target)
		{
			_targets.Add(target);
		}

		public void Log (object source, LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			if (level > logLevel)
				return;

			foreach (ILogTarget target in _targets)
			{
				target.Log(source, level, timestamp, message, messageParameters);
			}
		}

		public void RemoveAllTargets()
		{
			_targets.Clear();
		}
	}
}

