//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Framework.Impl;

namespace Robotlegs.Bender.Framework.API
{
	public interface ILogTarget
	{
		/// <summary>
		/// Captures a log message
		/// </summary>
		/// <param name="source">The source of the log message</param>
		/// <param name="level">The log level of the message</param>
		/// <param name="timestamp">message timestamp</param>
		/// <param name="message">The log message</param>
		/// <param name="messageParameters">Message parameters</param>
		void Log(object source, LogLevel level, DateTime timestamp, object message, params object[] messageParameters);
	}
}

