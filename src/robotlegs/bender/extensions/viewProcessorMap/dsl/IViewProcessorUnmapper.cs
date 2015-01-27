using System;

namespace robotlegs.bender.extensions.viewProcessorMap.dsl
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

