//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Framework.API
{
	/**
	 * The Robotlegs logger contract
	 */
	public interface ILogging
	{
		/**
		 * Logs a message for debug purposes
		 * @param message Message to log
		 * @param params Message parameters
		 */
		//
		void Debug(object message, params object[] parameters);
		
		/**
		 * Logs a message for notification purposes
		 * @param message Message to log
		 * @param params Message parameters
		 */
		void Info(object message, params object[] parameters);
		
		/**
		 * Logs a warning message
		 * @param message Message to log
		 * @param params Message parameters
		 */
		void Warn(object message, params object[] parameters);
		
		/**
		 * Logs an error message
		 * @param message Message to log
		 * @param params Message parameters
		 */
		void Error(object message, params object[] parameters);
		
		/**
		 * Logs a fatal error message
		 * @param message Message to log
		 * @param params Message parameters
		 */
		void Fatal(object message, params object[] parameters);
	}
}