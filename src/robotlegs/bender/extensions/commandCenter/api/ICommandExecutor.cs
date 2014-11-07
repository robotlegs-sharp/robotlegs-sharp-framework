using System.Collections.Generic;

namespace robotlegs.bender.extensions.commandCenter.api
{
	/// <summary>
	/// Optional Command interface.
	///
	/// <p>Note, you do not need to implement this interface,
	/// any class with an execute method can be used.</p>
	/// </summary>
	public interface ICommandExecutor
	{
		/// <summary>
		/// Execute a command for a given mapping
		/// </summary>
		/// <param name="mapping">The Command Mapping</param>
		/// <param name="payload">The Command Payload</param>
		void ExecuteCommand(ICommandMapping mapping, CommandPayload payload);

		/// <summary>
		/// Execute a list of commands for a given list of mappings
		/// </summary>
		/// <param name="mapping">The Command Mappings</param>
		/// <param name="payload">The Command Payload</param>
		void ExecuteCommands(List<ICommandMapping> mapping, CommandPayload payload);
	}
}