//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Robotlegs.Bender.Extensions.EventManagement.API;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.Modularity.API;
using Robotlegs.Bender.Extensions.Modularity.DSL;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.Modularity.Impl
{
	public class ModuleConnector : IModuleConnector
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _rootInjector;

		private IEventDispatcher _localDispatcher;

		private Dictionary<object, ModuleConnectionConfigurator> _configuratorsByChannel = new Dictionary<object, ModuleConnectionConfigurator>();

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/**
		 * @private
		 */
		public ModuleConnector(IContext context)
		{
			IInjector injector= context.injector;
			_rootInjector = GetRootInjector(injector);
			_localDispatcher = injector.GetInstance(typeof(IEventDispatcher)) as IEventDispatcher;
			context.WhenDestroying(Destroy);
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IModuleConnectionAction OnChannel(object channelId)
		{
			return GetOrCreateConfigurator(channelId);
		}

		public IModuleConnectionAction OnDefaultChannel()
		{
			return GetOrCreateConfigurator(ModuleChannels.Global);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void Destroy()
		{
			object[] configuratorsByChannelKeys = new object[_configuratorsByChannel.Keys.Count];
			_configuratorsByChannel.Keys.CopyTo(configuratorsByChannelKeys, 0);

			foreach (object channelId in configuratorsByChannelKeys)
			{
				ModuleConnectionConfigurator configurator = _configuratorsByChannel[channelId];
				configurator.Destroy();
				_configuratorsByChannel.Remove(channelId);
			}

			_configuratorsByChannel = null;
			_localDispatcher = null;
			_rootInjector = null;
		}

		private ModuleConnectionConfigurator GetOrCreateConfigurator(object channelId)
		{
			ModuleConnectionConfigurator moduleConnectionConfigurator = null;
			if (!_configuratorsByChannel.TryGetValue (channelId, out moduleConnectionConfigurator))
			{
				moduleConnectionConfigurator = _configuratorsByChannel[channelId] = CreateConfigurator(channelId);
			}
			return moduleConnectionConfigurator;
		}

		private ModuleConnectionConfigurator CreateConfigurator(object channelId)
		{
			if (!_rootInjector.HasMapping(typeof(IEventDispatcher), channelId))
			{
				_rootInjector.Map(typeof(IEventDispatcher), channelId)
					.ToValue(new EventDispatcher());
			}
			return new ModuleConnectionConfigurator(_localDispatcher, _rootInjector.GetInstance(typeof(IEventDispatcher), channelId) as IEventDispatcher);
		}

		private IInjector GetRootInjector(IInjector injector)
		{
			while (injector.parent != null)
			{
				injector = injector.parent;
			}
			return injector;
		}
	}
}
