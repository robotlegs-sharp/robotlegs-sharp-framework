//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Extensions.ViewProcessor.DSL
{
	/// <summary>
	/// Unmaps a view processor
	/// </summary>
	public interface IViewProcessorUnmapper
	{
		/// <summary>
		/// Unmaps a processor from a matcher
		/// </summary>
		/// <param name="processorClassOrInstance">Processor class or instance to remove from a matcher.</param>
		void FromProcess(object processorClassOrInstance);

		/// <summary>
		/// Unmaps a matcher
		/// </summary>
		void FromNoProcess();

		/// <summary>
		/// Unmaps an injection processor
		/// </summary>
		void FromInjection();

		/// <summary>
		/// Unmaps all processors from this matcher
		/// </summary>
		void FromAll();
	}
}

