using System;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportContainer
	{
		protected int _childCount;

		public int NumChildren
		{
			get
			{
				return _childCount;
			}
		}

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
			_childCount++;
			child.Parent = this;
		}

		public virtual void RemoveChild(SupportContainer child)
		{
			if (child.Parent == this)
			{
				child.Parent = null;
			}
		}
	}
}

