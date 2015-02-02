using System;

namespace robotlegs.bender.extensions.modularity.dsl
{
	public interface IModuleConnectionAction
	{

		IModuleConnectionAction RelayEvent(Enum eventType);

		IModuleConnectionAction ReceiveEvent(Enum eventType);

		void Suspend();

		void Resume();
	}
}