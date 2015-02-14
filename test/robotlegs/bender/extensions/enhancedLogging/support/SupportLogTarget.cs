//------------------------------------------------------------------------------
//  Copyright (c) 2011 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using robotlegs.bender.framework.api;
using System;
using robotlegs.bender.framework.impl.loggingSupport;

namespace robotlegs.bender.extensions.enhancedLogging.support
{

	public class SupportLogTarget : ILogTarget
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<LogParams> _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public SupportLogTarget(Action<LogParams> callback)
		{
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Log (object source, robotlegs.bender.framework.impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			_callback(new LogParams (source, level, timestamp, message, messageParameters));
		}
	}
}
