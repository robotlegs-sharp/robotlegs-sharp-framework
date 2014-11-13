using System;
using robotlegs.bender.extensions.commandCenter.dsl;
using robotlegs.bender.extensions.commandCenter.impl;

namespace robotlegs.bender.extensions.eventCommandMap.api
{
	public interface IEventCommandMap
	{
		/// <summary>
		/// Creates a mapping for an Event based trigger
		/// </summary>
		/// <param name="type">The Event type</param>
		/// <param name="eventClass">The concrete Event class</param>
		ICommandMapper Map(Enum type, Type eventClass = null);

		/// <summary>
		/// Unmaps an Event based trigger from a Command
		/// </summary>
		/// <param name="type">The Event type</param>
		/// <param name="eventClass">The concrete Event class</param>
		ICommandUnmapper Unmap(Enum type, Type eventClass = null);

		/// <summary>
		/// Adds a handler to a process mappings
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="handler">Function that accepts a mapping</param>
		IEventCommandMap AddMappingProcessor(CommandMappingList.Processor handler);
	}
}

