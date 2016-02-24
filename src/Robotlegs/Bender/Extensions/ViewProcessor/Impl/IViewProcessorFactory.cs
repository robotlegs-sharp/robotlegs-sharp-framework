//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.ViewProcessor.DSL;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public interface IViewProcessorFactory
	{
		void RunProcessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings);

		void RunUnprocessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings);

		void RunAllUnprocessors();
	}
}

