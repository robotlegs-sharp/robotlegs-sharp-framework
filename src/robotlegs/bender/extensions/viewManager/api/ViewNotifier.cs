using System;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.viewManager.api
{
	public static class ViewNotifier
	{
		private static ContainerRegistry _registry;

		public static void SetRegistry(ContainerRegistry containerRegistry)
		{
			_registry = containerRegistry;
		}

		public static void RegisterView(object view, Type type)
		{
			if (_registry == null)
				return;

			ContainerBinding binding = _registry.FindParentBinding(view);
			while (binding != null)
			{
				binding.HandleView(view, type);
				binding = binding.Parent;
			}
		}
	}
}

