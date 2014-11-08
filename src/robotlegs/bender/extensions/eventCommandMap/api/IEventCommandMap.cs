using System;
using robotlegs.bender.extensions.commandCenter.dsl;

namespace robotlegs.bender.extensions.eventCommandMap.api
{
	public interface IEventCommandMap
	{
		/// <summary>
		/// Creates a mapping for an Event based trigger
		/// </summary>
		/// <param name="type">The Event type</param>
		/// <param name="eventClass">The concrete Event class</param>
		ICommandMapper map(string type, Type eventClass = null);

		/// <summary>
		/// Unmaps an Event based trigger from a Command
		/// </summary>
		/// <param name="type">The Event type</param>
		/// <param name="eventClass">The concrete Event class</param>
		ICommandUnmapper unmap(string type, Type eventClass = null);
	}
}

