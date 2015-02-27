using System;
using swiftsuspenders.dependencyproviders;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.enhancedLogging.impl
{
	public class LoggerProvider : DependencyProvider
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<DependencyProvider, object> PostApply;

		public event Action<DependencyProvider, object> PreDestroy;

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
			object logger = _context.GetLogger(targetType);;
			if (PostApply != null)
			{
				PostApply(this, logger);
			}
			return logger;
		}

		public void Destroy ()
		{
			if (PreDestroy != null) 
			{
				PreDestroy(this, null);
			}
		}
	}
}

