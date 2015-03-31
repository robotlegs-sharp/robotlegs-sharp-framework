//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.viewProcessorMap.dsl;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public interface IViewProcessorFactory
	{
		void RunProcessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings);

		void RunUnprocessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings);

		void RunAllUnprocessors();
	}
}

