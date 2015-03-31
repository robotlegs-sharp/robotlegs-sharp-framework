//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System;

namespace robotlegs.bender.extensions.viewProcessorMap.support
{
	public class Processor
	{
		[Inject("timingTracker")]
		public List<Type> timingTracker;

		public void Process(object view, Type type, object injector)
		{
			timingTracker.Add(typeof(Processor));
		}

		public void Unprocess(object view, Type type, object injector)
		{

		}
	}
}