using System;

namespace robotlegs.bender.extensions.directCommandMap.api
{
	public interface IDirectCommandMap : IDirectCommandMapper
	{
		/// <summary>
		/// Pins a command in memory
		/// </summary>
		/// <param name="command">The command instance to pin</param>
		void Detain(object command);

		/// <summary>
		/// Unpins a command instance from memory
		/// </summary>
		/// <param name="command">The command instance to unpin</param>
		void Release(object command);

		/// <summary>
		/// Adds a handler to the process mappings
		/// </summary>
		/// <returns>Self</returns>
		/// <param name="handler">Delegate that accepts a mapping</param>
		IDirectCommandMap AddMappingProcessor(robotlegs.bender.extensions.commandCenter.impl.CommandMappingList.Processor handler);
	}
}

