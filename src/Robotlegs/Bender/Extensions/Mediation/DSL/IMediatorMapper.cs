//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;

namespace Robotlegs.Bender.Extensions.Mediation.DSL
{
	/// <summary>
	/// Maps a matcher to a concrete Mediator type
	/// </summary>
	public interface IMediatorMapper
	{
		/// <summary>
		/// Maps a matcher to a concrete Mediator type
		/// </summary>
		/// <returns>Mapping configurator</returns>
		/// <param name="mediatorType">The concrete mediator type</param>
		IMediatorConfigurator ToMediator(Type mediatorType);
		IMediatorConfigurator ToMediator<T>();

	}
}

