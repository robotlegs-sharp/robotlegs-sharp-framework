using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class OptionalInjectionPointsCommandInstantiatingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IInjector injector;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			OptionalInjectionPointsCommand command = injector.InstantiateUnmapped<OptionalInjectionPointsCommand>();
			command.Execute();
		}
	}
}

