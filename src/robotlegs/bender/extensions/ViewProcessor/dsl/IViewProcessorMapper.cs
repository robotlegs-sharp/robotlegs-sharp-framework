//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Extensions.ViewProcessor.DSL
{
	public interface IViewProcessorMapper
	{
		/**
		 * @param processClassOrInstance 
		 */
		/// <summary>
		/// Specifies the process to be mapped against the type or matcher. 
		/// </summary>
		/// <returns>The mapping config so that you can specify further details.</returns>
		/// <param name="processClassOrInstance"><p>An instance of a class, or a class implementing the following methods:</p>
		/// <p>process(view:ISkinnable, class:Class, injector:Injector):void;</p>
		/// <p>unprocess(view:ISkinnable, class:Class, injector:Injector):void;</p></param>
		IViewProcessorMappingConfig ToProcess(object processClassOrInstance);
		//TODO:Matt Maybe overload with the possible Action signatures? See code hinting docs above.

		/// <summary>
		/// Maps the type of matcher for injection
		/// </summary>
		/// <returns>The mapping config so that you can specify further details.</returns>
		IViewProcessorMappingConfig ToInjection();

		/// <summary>
		/// Maps the type of matcher to a nothing-happens process, so that you can make use of guards and hooks.
		/// </summary>
		/// <returns>The mapping config so that you can specify further details.</returns>
		IViewProcessorMappingConfig ToNoProcess();
	}
}

