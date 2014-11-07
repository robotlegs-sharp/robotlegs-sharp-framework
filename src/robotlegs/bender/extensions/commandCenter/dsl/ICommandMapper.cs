using System;

namespace robotlegs.bender.extensions.commandCenter.dsl
{
	public interface ICommandMapper
	{
		/// <summary>
		/// Creates a command mapping
		/// </summary>
		/// <returns>Mapping configurator</returns>
		/// <param name="commandType">The Command Class to map</param>
		ICommandConfigurator ToCommand(Type commandType);
	}
}