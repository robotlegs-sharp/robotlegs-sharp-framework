//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.api;


namespace robotlegs.bender.extensions.mediatorMap.dsl
{
	public interface IMediatorFactory
	{
		object GetMediator (object item, IMediatorMapping mapping);
		
		List<object> CreateMediators (object item, Type type, IEnumerable<IMediatorMapping> mappings);
		
		void RemoveMediators (object item);
		
		void RemoveAllMediators();
	}
}

