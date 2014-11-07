using System;
using robotlegs.bender.extensions.commandCenter.api;

namespace robotlegs.bender.extensions.directCommandMap.api
{
	public interface IDirectCommandMapper
	{
		/// <summary>
		/// Creates a mapping for a command class
		/// </summary>
		/// <returns>The command configurator</returns>
		/// <param name="commandClass">The concrete command class</param>
		IDirectCommandConfigurator Map(Type commandClass);

		/// <summary>
		/// Executes the configured command(s)
		/// </summary>
		/// <param name="payload">The command payload</param>
		void Execute(CommandPayload payload = null);
	}
}

