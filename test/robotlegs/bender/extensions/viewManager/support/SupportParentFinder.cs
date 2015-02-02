using System;
using robotlegs.bender.extensions.viewManager;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportParentFinder : IParentFinder
	{
		public bool Contains (object parentContainer, object childContainer)
		{
			SupportContainer parentSupport = parentContainer as SupportContainer;
			SupportContainer childSupport = childContainer as SupportContainer;

			if (parentSupport == null || childContainer == null)
				return false;

			while (childSupport != null)
			{
				if (childSupport.Parent == parentSupport)
					return true;

				childSupport = childSupport.Parent;
			}
			return false;
		}

		public object FindParent (object childView, System.Collections.Generic.Dictionary<object, robotlegs.bender.extensions.viewManager.impl.ContainerBinding> containers)
		{
			foreach (object obj in containers.Keys)
			{
				Console.WriteLine (obj);
			}

			SupportContainer supportView = childView as SupportContainer;
			while (supportView.Parent != null)
			{
				foreach (object parent in containers.Keys)
				{
					if (parent == supportView.Parent)
					{
						return parent;
					}
				}
				supportView = supportView.Parent;
			}
			return null;
		}
	}
}

