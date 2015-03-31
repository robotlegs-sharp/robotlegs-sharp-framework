//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
namespace robotlegs.bender.extensions.mediatorMap.dsl
{
	public interface IMediatorConfigurator
	{
		/// <summary>
		/// Guards to check before allowing a mediator to be created
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="guards">Guards</param>
		IMediatorConfigurator WithGuards(params object[] guards);
		IMediatorConfigurator WithGuards<T>();
		IMediatorConfigurator WithGuards<T1, T2>();
		IMediatorConfigurator WithGuards<T1, T2, T3>();
		IMediatorConfigurator WithGuards<T1, T2, T3, T4>();
		IMediatorConfigurator WithGuards<T1, T2, T3, T4, T5>();


		/// <summary>
		/// Hooks to run before a mediator is created
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="hooks">Hooks</param>
		IMediatorConfigurator WithHooks(params object[] hooks);
		IMediatorConfigurator WithHooks<T>();
		IMediatorConfigurator WithHooks<T1, T2>();
		IMediatorConfigurator WithHooks<T1, T2, T3>();
		IMediatorConfigurator WithHooks<T1, T2, T3, T4>();
		IMediatorConfigurator WithHooks<T1, T2, T3, T4, T5>();
		
		/// <summary>
		/// Should the mediator be removed when the mediated item looses scope?
		///
		/// <p>Usually this would be when the mediated item is a Display Object
		/// and it leaves the stage.</p>
		/// </summary>
		/// <returns>Self</returns>
		IMediatorConfigurator AutoRemove(bool value);
	}
}

