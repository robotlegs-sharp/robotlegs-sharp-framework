using System;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportContainer
	{
		public virtual SupportContainer Parent
		{
			get
			{
				return _parent;
			}
			protected set
			{
				_parent = value;
			}
		}

		protected SupportContainer _parent;

		public virtual void AddChild(SupportContainer child)
		{
			child.Parent = this;
		}
	}
}

