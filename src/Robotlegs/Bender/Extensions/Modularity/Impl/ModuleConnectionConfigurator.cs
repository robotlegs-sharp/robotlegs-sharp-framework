//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.Modularity.DSL;

namespace Robotlegs.Bender.Extensions.Modularity.Impl
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
