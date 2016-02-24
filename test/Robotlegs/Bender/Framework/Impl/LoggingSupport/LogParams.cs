//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace Robotlegs.Bender.Framework.Impl.LoggingSupport
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

