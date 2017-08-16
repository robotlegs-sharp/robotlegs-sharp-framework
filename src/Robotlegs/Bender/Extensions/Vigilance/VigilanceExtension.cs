//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using SwiftSuspenders.Errors;
using SwiftSuspenders.Mapping;

namespace Robotlegs.Bender.Extensions.Vigilance
{
	public class VigilanceExtension : IExtension, ILogTarget
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend (IContext context)
		{
			context.AddLogTarget (this);
			context.injector.MappingOverride += MappingOverrideHandler;
		}

		public void Log (object source, Robotlegs.Bender.Framework.Impl.LogLevel level, DateTime timestamp, object message, params object[] messageParameters)
		{
			if (level <= LogLevel.WARN)
			{
				throw new VigilantException(string.Format(message.ToString(), messageParameters));
			}
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		void MappingOverrideHandler (MappingId mappingId, InjectionMapping instanceType)
		{
			throw new InjectorException("Injector mapping override for type " +
				mappingId.type + " with name " + mappingId.key);
		}
	}
}

