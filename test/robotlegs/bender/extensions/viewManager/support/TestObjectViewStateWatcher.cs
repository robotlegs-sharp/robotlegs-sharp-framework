using System;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class TestObjectViewStateWatcher : IViewStateWatcher
	{

		public event Action<object> added;

		public event Action<object> removed;

		public event Action<object> disabled;

		public event Action<object> enabled;

		public bool isAdded 
		{ 
			get 
			{ 
				return _isAdded; 
			}
			set
			{
				_isAdded = value;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ObjectA _target;

		private bool _isAdded;

		/*============================================================================*/
		/* Constructor																  */
		/*============================================================================*/

		public TestObjectViewStateWatcher(object target)
		{
			_target = target as ObjectA;
			_isAdded = _target.isAddedToStage;
			_target.AddView += Added;
			_target.RemoveView += Removed;
			_target.EnableView += Enabled;
			_target.DisableView += Disabled;
		}

		/*============================================================================*/
		/* Public Functions                                                        	  */
		/*============================================================================*/

		public void Added(IView view)
		{
			_isAdded = true;
			if (this.added != null)
			{
				this.added(view);
			}
		}

		public void Disabled(IView view)
		{
			if (this.disabled != null)
			{
				this.disabled (view);
			}
		}

		public void Enabled(IView view)
		{
			if (this.enabled != null)
			{
				this.enabled(view);
			}
		}

		public void Removed(IView view)
		{
			_isAdded = false;
			if (this.removed != null)
			{
				this.removed (view);
			}
		}
	}
}

