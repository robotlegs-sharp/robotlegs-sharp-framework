using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.viewManager.impl;


namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportParentFinderExtension : IExtension
	{
		private IInjector _injector;

		private IParentFinder _parentFinder;

		public void Extend (IContext context)
		{
			_injector = context.injector;

			_parentFinder = new SupportParentFinder();
			_injector.Map(typeof(IParentFinder)).ToValue(_parentFinder);
			context.BeforeInitializing(BeforeInitializing);
		}

		private void BeforeInitializing()
		{
			if (_injector.HasDirectMapping (typeof(ContainerRegistry)))
			{
				ContainerRegistry registry = _injector.GetInstance (typeof(ContainerRegistry)) as ContainerRegistry;
				registry.SetParentFinder(_parentFinder);
			}
		}
	}
}

