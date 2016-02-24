//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Extensions.Mediation.API;

namespace Robotlegs.Bender.Extensions.Mediation.DSL
{
	public interface IMediatorManager
	{
		event Action<object> ViewRemoved;

		void AddMediator (object mediator, object item, IMediatorMapping mapping);

		void RemoveMediator (object mediator, object item, IMediatorMapping mapping);
	}
}