using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportView : SupportContainer, IView, IViewStateWatcher
	{
		public bool isAdded { get { return isAddedToStage; } }

		public event Action<object> added;
		public event Action<object> removed;
		public event Action<object> disabled;
		public event Action<object> enabled;
		public event Action<IView> RemoveView;
		public bool isAddedToStage;

		private bool registered = false;

		public void AddThisView()
		{
			Register ();
			isAddedToStage = true;
			if (added != null)
			{
				added(this);
			}
		}

		public void RemoveChild (SupportView child)
		{
			if (child.Parent == this)
			{
				child.RemoveThisView();
			}
		}

		public void RemoveThisView()
		{
			isAddedToStage = false;
			Parent = null;
			if (RemoveView != null)
			{
				RemoveView (this);
			}
			if (removed != null)
			{
				removed(this);
			}
		}

		public override void AddChild(SupportContainer child)
		{
			base.AddChild (child);
			if (child is SupportView) (child as SupportView).AddThisView();
		}

		public void Enable()
		{
			if (enabled != null)
			{
				enabled (this);
			}
		}

		public void Disable()
		{
			if (disabled != null)
			{
				disabled (this);
			}
		}

		public void Register()
		{
			if (registered)
				return;

			registered = true;
			ViewNotifier.RegisterView (this);
		}
			
		public override SupportContainer Parent
		{
			get
			{
				return base.Parent;
			}
			protected set
			{
				base.Parent = value;
				if (value != null)
				{
					Register ();
				}
			}
		}
	}
}

