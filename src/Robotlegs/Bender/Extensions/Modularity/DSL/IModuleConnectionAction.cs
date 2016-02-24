//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;

namespace Robotlegs.Bender.Extensions.Modularity.DSL
{
	public interface IModuleConnectionAction
	{

		IModuleConnectionAction RelayEvent(Enum eventType);

		IModuleConnectionAction ReceiveEvent(Enum eventType);

		void Suspend();

		void Resume();
	}
}