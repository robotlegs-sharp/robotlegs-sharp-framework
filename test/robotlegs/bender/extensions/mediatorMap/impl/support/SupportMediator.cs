using System;
using robotlegs.bender.extensions.viewManager.support;

namespace robotlegs.bender.extensions.mediatorMap.impl.support
{
	public class SupportMediator
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public MediatorWatcher mediatorWatcher {get;set;}

		[Inject]
		public SupportView view {get;set;}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Initialize()
		{
			mediatorWatcher.Notify("SupportMediator");
		}

		public void Destroy()
		{
			mediatorWatcher.Notify("SupportMediator destroy");
		}
	}
}

