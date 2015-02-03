using System;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.viewManager.impl
{
	public class BlankParentFinder : IParentFinder
	{
		public bool Contains (object parentContainer, object childContainer)
		{
			return false;
		}

		public object FindParent (object childView, List<ContainerBinding> containers)
		{
			return null;
		}

		public object FindParent (object childView, Dictionary<object, ContainerBinding> containers)
		{
			return null;
		}
	}
}

