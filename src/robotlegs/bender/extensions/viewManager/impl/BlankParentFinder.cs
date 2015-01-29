using System;

namespace robotlegs.bender.extensions.viewManager.impl
{
	public class BlankParentFinder : IParentFinder
	{
		public bool Contains (object parentContainer, object childContainer)
		{
			return false;
		}

		public object FindParent (object childView, System.Collections.Generic.Dictionary<object, ContainerBinding> containers)
		{
			return null;
		}
	}
}

