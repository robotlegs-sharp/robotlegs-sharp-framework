using robotlegs.bender.extensions.modularity.dsl;

namespace robotlegs.bender.extensions.modularity.api
{
	/// <summary>
	/// Create event relays between modules
	/// </summary>
	public interface IModuleConnector
	{
		/// <summary>
		/// Connects to a specified channel.
		/// </summary>
		/// <param name="channelId">The channel Id.</param>
		IModuleConnectionAction OnChannel(object channelId);

		/// <summary>
		/// Connects to the default channel
		/// </summary>
		IModuleConnectionAction OnDefaultChannel();
	}
}