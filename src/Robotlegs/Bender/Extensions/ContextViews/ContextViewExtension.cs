//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextViews.API;
using Robotlegs.Bender.Extensions.Matching;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ContextViews
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

		private IInjector _injector;
		
		private ILogging _logger;
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Extend(IContext context)
		{
			_injector = context.injector;
			_logger = context.GetLogger(this);
			context.AfterInitializing (BeforeInitializing);
			context.AddConfigHandler(new InstanceOfMatcher (typeof(IContextView)), AddContextView);
		}
		
		/*============================================================================*/
		/* Private Functions                                                           */
		/*============================================================================*/

		private void AddContextView(object contextViewObject)
		{
			IContextView contextView = contextViewObject as IContextView;
			if (!HasContextBinding ())
			{
				_logger.Debug("Mapping {0} as contextView", contextView.view);
				_injector.Map(typeof(IContextView)).ToValue(contextView);
			}
			else
				_logger.Warn("A contextView has already been installed, ignoring {0}", contextView.view);
		}

		private void BeforeInitializing()
		{
			if (!HasContextBinding ()) 
			{
				_logger.Error("Warning, you initilaized the context without a context view when the context view extension was installed");
			}
		}
		
		private bool HasContextBinding()
		{
			return _injector.HasDirectMapping(typeof(IContextView));
		}
	}
}

