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
	/// Unmaps a Mediator
	/// </summary>
	public interface IMediatorUnmapper
	{
		/// <summary>
		/// Unmaps a mediator from this matcher
		/// </summary>
		/// <param name="mediatorType">Mediator to unmap</param>
		void FromMediator(Type mediatorType);

		/// <summary>
		/// Unmaps all mediator mappings for this matcher
		/// </summary>
		void FromAll();
	}
}

