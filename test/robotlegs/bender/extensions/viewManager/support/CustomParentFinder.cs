using System;

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

		public object FindParent (object childView, System.Collections.Generic.Dictionary<object, robotlegs.bender.extensions.viewManager.impl.ContainerBinding> containers)
		{
			return _parent;
		}
	}
}

