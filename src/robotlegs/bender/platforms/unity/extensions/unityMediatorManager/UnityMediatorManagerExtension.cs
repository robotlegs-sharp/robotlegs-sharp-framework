using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.mediatorMap.dsl;

public class UnityMediatorManagerExtension : IExtension
{
	public void Extend (IContext context)
	{
		context.injector.Map (typeof(IMediatorManager)).ToSingleton (typeof(UnityMediatorManager));
	}
}
