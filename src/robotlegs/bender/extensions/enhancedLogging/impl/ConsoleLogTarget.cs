//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.enhancedLogging.impl
{
	public class ConsoleLogTarget : ILogTarget
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private IContext _context;
		
		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		
		/**
		 * Creates a Console Log Target
		 * @param context Context
		 */
		
		public ConsoleLogTarget (IContext context)
		{
			_context = context;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Log (object source, robotlegs.bender.framework.impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			Console.WriteLine(timestamp.ToLongTimeString()
			      + " " + level.ToString()
			      + " " + _context
			      + " " + source
			      + " " + message, messageParameters);
		}
	}
}

