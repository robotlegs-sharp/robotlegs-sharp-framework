using System;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportContainer
	{
		public SupportContainer Parent
		{
			get
			{
				return _parent;
			}
		}

		private SupportContainer _parent;

		public void AddChild(SupportContainer child)
		{
			child._parent = this;
		}
	}
}

