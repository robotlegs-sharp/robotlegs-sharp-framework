using System;
using swiftsuspenders.dependencyproviders;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.enhancedLogging.impl
{
	public class LoggerProvider : DependencyProvider
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext _context;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LoggerProvider(IContext context)
		{
			_context = context;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public object Apply (Type targetType, swiftsuspenders.Injector activeInjector, System.Collections.Generic.Dictionary<string, object> injectParameters)
		{
			return _context.GetLogger(targetType);
		}

		public void Destroy ()
		{

		}
	}
}

