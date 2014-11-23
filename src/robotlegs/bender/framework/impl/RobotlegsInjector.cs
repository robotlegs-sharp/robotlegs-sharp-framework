using System;
using swiftsuspenders;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class RobotlegsInjector : Injector, IInjector
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public IInjector parent
		{
			set
			{
				this.parentInjector = value as Injector;
			}
			get 
			{
				return this.parentInjector as RobotlegsInjector;
			}
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IInjector CreateChild(/*applicationDomain:ApplicationDomain = null*/)
		{
			IInjector childInjector = new RobotlegsInjector();
//			childInjector.applicationDomain = applicationDomain || this.applicationDomain;
			childInjector.parent = this;
			return childInjector;
		}
	}
}

