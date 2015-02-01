using System;
using robotlegs.bender.extensions.matching;

namespace robotlegs.bender.extensions.viewProcessorMap.dsl
{
	public interface IViewProcessorMapping
	{
		/// <summary>
		/// The matchher for this mapping
		/// </summary>
		ITypeFilter Matcher {get;}

		/// <summary>
		/// The processor object for this mapping
		/// </summary>
		object Processor { get; set; }

		/// <summary>
		/// The processor type for this mapping
		/// </summary>
		Type ProcessorClass { get; }

		/// <summary>
		/// Returns a list of guards to consult before allowing a view to be processed
		/// </summary>
		object[] Guards {get;}

		/// <summary>
		/// Returns a list of hooks to run before processing a view
		/// </summary>
		object[] Hooks { get; }
	}
}

