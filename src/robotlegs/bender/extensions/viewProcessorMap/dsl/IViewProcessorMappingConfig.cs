using System;

namespace robotlegs.bender.extensions.viewProcessorMap.dsl
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

