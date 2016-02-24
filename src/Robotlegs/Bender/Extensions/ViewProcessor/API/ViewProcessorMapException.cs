//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace Robotlegs.Bender.Extensions.ViewProcessor.API
{
	public class ViewProcessorMapException : Exception
	{
		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Creates a View Processor Map Error
		/// </summary>
		/// <param name="message">The error message</param>
		public ViewProcessorMapException(string message) : base(message)
		{

		}
	}
}

