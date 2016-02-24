//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Extensions.ViewProcessor.DSL
{
	public interface IViewProcessorMappingConfig
	{
		/// <summary>
		/// A list of guards to consult before allowing a view to be processed
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="guards">A list of guards.</param>
		IViewProcessorMappingConfig WithGuards(params object[] guards);

		/// <summary>
		/// A list of hooks to run before processing a view
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="hooks">A list of hooks.</param>
		IViewProcessorMappingConfig WithHooks(params object[] hooks);
	}
}

