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

		public event Action<DependencyProvider, object> PostApply
		{
			add
			{
				_postApply += value;
			}
			remove 
			{
				_postApply -= value;
			}
		}

		public event Action<DependencyProvider, object> PreDestroy
		{
			add
			{
				_preDestroy += value;
			}
			remove 
			{
				_preDestroy -= value;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<DependencyProvider, object> _postApply;

		private Action<DependencyProvider, object> _preDestroy;

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
			if (_postApply != null)
			{
				_postApply(this, logger);
			}
			return logger;
		}

		public void Destroy ()
		{
			if (_preDestroy != null) 
			{
				_preDestroy(this, null);
			}
		}
	}
}

