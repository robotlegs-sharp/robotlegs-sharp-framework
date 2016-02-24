//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.Mediation.API;

namespace Robotlegs.Bender.Extensions.Mediation.DSL
{
	public interface IMediatorFactory
	{
		object GetMediator (object item, IMediatorMapping mapping);
		
		List<object> CreateMediators (object item, Type type, IEnumerable<IMediatorMapping> mappings);
		
		void RemoveMediators (object item);
		
		void RemoveAllMediators();
	}
}

