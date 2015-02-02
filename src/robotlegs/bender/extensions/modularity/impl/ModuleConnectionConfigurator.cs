using robotlegs.bender.extensions.eventDispatcher.impl;
using robotlegs.bender.extensions.modularity.dsl;
using robotlegs.bender.extensions.eventDispatcher.api;
using System;

namespace robotlegs.bender.extensions.modularity.impl
{
	public class ModuleConnectionConfigurator : IModuleConnectionAction
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private EventRelay _channelToLocalRelay;

		private EventRelay _localToChannelRelay;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ModuleConnectionConfigurator(
			IEventDispatcher localDispatcher,
			IEventDispatcher channelDispatcher)
		{
			_localToChannelRelay = new EventRelay(localDispatcher, channelDispatcher).Start();
			_channelToLocalRelay = new EventRelay(channelDispatcher, localDispatcher).Start();
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IModuleConnectionAction RelayEvent(Enum eventType)
		{
			_localToChannelRelay.AddType(eventType);
			return this;
		}
			
		public IModuleConnectionAction ReceiveEvent(Enum eventType)
		{
			_channelToLocalRelay.AddType(eventType);
			return this;
		}

		public void Suspend()
		{
			_channelToLocalRelay.Stop();
			_localToChannelRelay.Stop();
		}
			
		public void Resume()
		{
			_channelToLocalRelay.Start();
			_localToChannelRelay.Start();
		}

		public void Destroy()
		{
			_localToChannelRelay.Stop();
			_localToChannelRelay = null;
			_channelToLocalRelay.Stop();
			_channelToLocalRelay = null;
		}
	}
}
