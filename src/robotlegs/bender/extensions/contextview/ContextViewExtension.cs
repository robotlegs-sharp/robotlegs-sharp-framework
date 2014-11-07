using System;
using robotlegs.bender.framework.api;
using strange.extensions.injector.api;
using robotlegs.bender.extensions.contextview.impl;
using robotlegs.bender.extensions.contextview.api;
using strange.context.api;
using robotlegs.bender.extensions.matching;

namespace stange.extensions.contextview
{
	/// <summary>
	/// <p>This Extension waits for a ContextView to be added as a configuration
	/// and maps it into the context's injector.</p>
	///
	/// <p>It should be installed before context initialization.</p>
	/// </summary>
	public class ContextViewExtension : IExtension
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjectionBinder _injector;
		
		private ILogger _logger;
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			_injector = context.injectionBinder;
			_logger = context.GetLogger(this);
			context.AddPostInitializedCallback (BeforeInitializing);
			context.AddConfigHandler(new InstanceOfMatcher (typeof(ContextView)), AddContextView);
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		private void AddContextView(object contextViewObject)
		{
			ContextView contextView = contextViewObject as ContextView;
			if (!HasContextBinding ()) 
			{
				_logger.Debug("Mapping {0} as contextView", contextView.view);
				_injector.Bind<ContextView> ().To (contextView);
				_injector.Bind<IContextView> ().To (contextView);
			}
			else
				_logger.Warn("A contextView has already been installed, ignoring {0}", contextView.view);
		}

		private void BeforeInitializing()
		{
			if (!HasContextBinding ()) 
			{
				Console.WriteLine("Warning, you initilaized the context without a context view when the context view extension was installed");
			}
		}
		
		private bool HasContextBinding()
		{
			return _injector.GetBinding<ContextView> () != null;
		}
	}
}

