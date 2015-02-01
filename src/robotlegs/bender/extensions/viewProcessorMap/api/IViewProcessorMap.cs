using System;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.api
{
	/// <summary>
	/// The View Processor Map allows you to bind views to processors
	/// </summary>
	public interface IViewProcessorMap
	{
	
		/// <summary>
		/// <p>Maps a matcher that will be tested against incoming items to be handled.</p>
		/// </summary>
		/// <returns>The mapper so that you can continue the mapping.</returns>
		/// <param name="matcher">The type or package matcher specifying the rules for matching.</param>
		IViewProcessorMapper MapMatcher(ITypeMatcher matcher);

		/// <summary>
		/// <p>Maps a type that will be tested against incoming items to be handled.</p>
		/// <p>Under the hood this will create a TypeMatcher for this type.</p>
		/// <returns>The mapper so that you can continue the mapping.</returns>
		/// <param name ="type">The type to be matched against.</param>
		IViewProcessorMapper Map(Type type);

		/// <summary>
		/// <p>Removes a mapping that was made against a matcher.</p>
		/// <p>No error will be thrown if there isn't a mapping to remove.</p>
		/// <returns>The unmapper so that you can continue the unmapping.</returns>
		/// <param name ="type">The type or package matcher specifying the rules for matching</param>
		IViewProcessorUnmapper UnmapMatcher(ITypeMatcher matcher);

		/// <summary>
		/// <p>Removes a mapping that was made against a type.</p>
		/// <p>No error will be thrown if there isn't a mapping to remove.</p>
		/// </summary>
		/// <returns>The unmapper so that you can continue the unmapping.</returns>
		/// <param name="type">The Type to be matched against</param>
		IViewProcessorUnmapper Unmap(Type type);

		/// <summary>
		/// <p>Processes an item directly. If the item matches any mapped matchers or types then it will be processed according to those mappings.</p>
		/// </summary>
		/// <param name="item">The item to process.</param>
		void Process(object item);

		/// <summary>
		/// Runs unprocess on relevant processors for an item if there are any.
		/// </summary>
		/// <param name="item">The item to unprocess.</param>
		void Unprocess(object item);
	}
}

