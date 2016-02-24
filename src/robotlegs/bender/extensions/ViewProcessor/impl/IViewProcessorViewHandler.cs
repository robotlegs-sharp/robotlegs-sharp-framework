//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.ViewProcessor.DSL;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public interface IViewProcessorViewHandler
	{
		void AddMapping (IViewProcessorMapping mapping);

		void RemoveMapping (IViewProcessorMapping mapping);

		void ProcessItem(object item, Type type);

		void UnprocessItem(object item, Type type);
	}
}

