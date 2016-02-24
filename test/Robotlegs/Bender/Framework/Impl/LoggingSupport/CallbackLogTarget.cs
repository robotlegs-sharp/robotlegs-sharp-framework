//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Framework.Impl.LoggingSupport
{
	public class CallbackLogTarget : ILogTarget
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public delegate void LogParamDelegate(LogParams logParams);

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private LogParamDelegate _callback;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public CallbackLogTarget (LogParamDelegate callback)
		{
			_callback = callback;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Log (object source, LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			if (_callback != null)
				_callback(new LogParams(source, level, timestamp, message, messageParameters));
		}
	}
}

