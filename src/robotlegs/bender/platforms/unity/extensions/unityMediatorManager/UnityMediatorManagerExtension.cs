using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.mediatorMap.dsl;
using robotlegs.bender.platforms.unity.extensions.unityMediatorManager.impl;

namespace robotlegs.bender.platforms.unity.extensions.unityMediatorManager
{
	public class UnityMediatorManagerExtension : IExtension
	{
		public void Extend (IContext context)
		{
			context.injector.Map (typeof(IMediatorManager)).ToSingleton (typeof(UnityMediatorManager));
		}
	}
}
