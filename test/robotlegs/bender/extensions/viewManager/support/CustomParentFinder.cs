using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class CustomParentFinder : IParentFinder
	{
		private bool _contains;

		private object _parent;

		public CustomParentFinder(bool contains, object parent)
		{
			_contains = contains;
			_parent = parent;
		}

		public bool Contains (object parentContainer, object childContainer)
		{
			return _contains;
		}

		public object FindParent (object childView, Dictionary<object, ContainerBinding> containers)
		{
			return _parent;
		}

		public object FindParent (object childView, List<ContainerBinding> containers)
		{
			return _parent;
		}
	}
}

