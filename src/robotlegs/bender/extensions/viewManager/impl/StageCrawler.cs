using System;
using robotlegs.bender.extensions.viewManager.api;


namespace robotlegs.bender.extensions.viewManager.impl
{
	public abstract class StageCrawler : IStageCrawler
	{
		private ContainerBinding _binding;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/
		public ContainerBinding Binding
		{
			set
			{
				_binding = value;
			}
		}

		public void Scan(object view)
		{
			ScanContainer(view);
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected abstract void ScanContainer(object container);

		protected void ProcessView(object view)
		{
			_binding.HandleView(view, view.GetType());
		}
	}
}

