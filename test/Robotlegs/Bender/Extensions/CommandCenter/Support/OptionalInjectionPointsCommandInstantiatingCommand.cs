//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.CommandCenter.Support
{
	public class OptionalInjectionPointsCommandInstantiatingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IInjector injector;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			OptionalInjectionPointsCommand command = injector.InstantiateUnmapped<OptionalInjectionPointsCommand>();
			command.Execute();
		}
	}
}

