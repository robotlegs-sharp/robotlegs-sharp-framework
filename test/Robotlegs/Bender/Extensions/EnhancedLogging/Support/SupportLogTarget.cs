//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using Robotlegs.Bender.Framework.API;
using System;
using Robotlegs.Bender.Framework.Impl.LoggingSupport;

namespace Robotlegs.Bender.Extensions.EnhancedLogging.Support
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

		public void Log (object source, Robotlegs.Bender.Framework.Impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			_callback(new LogParams (source, level, timestamp, message, messageParameters));
		}
	}
}
