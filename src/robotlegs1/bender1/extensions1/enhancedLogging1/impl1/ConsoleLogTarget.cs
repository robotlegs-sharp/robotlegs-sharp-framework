//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.EnhancedLogging.Impl
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

		public void Log (object source, Robotlegs.Bender.Framework.Impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			Console.WriteLine(timestamp.ToLongTimeString()
			      + " " + level.ToString()
			      + " " + _context
			      + " " + source
			      + " " + message, messageParameters);
		}
	}
}

