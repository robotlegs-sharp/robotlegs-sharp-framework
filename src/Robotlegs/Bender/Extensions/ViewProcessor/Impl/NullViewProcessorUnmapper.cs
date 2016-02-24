//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ViewProcessor.DSL;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public class NullViewProcessorUnmapper : IViewProcessorUnmapper
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void FromProcess(object processorClassOrInstance)
		{
		}

		public void FromAll()
		{
		}

		public void FromNoProcess()
		{
		}

		public void FromInjection()
		{
		}
	}
}

