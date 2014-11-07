using System;
using System.Collections.Generic;
using strange.extensions.injector.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class ExtensionInstaller
	{
		
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		public Dictionary <Type, IExtension> _extensions = new Dictionary<Type, IExtension> ();

		public Context _context;

		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ExtensionInstaller (Context context)
		{
			_context = context;
			_logger = _context.GetLogger(this);
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Install<T>() where T : IExtension
		{
			Install (typeof(T));
		}

		public void Install(IExtension extension)
		{
			Type extensionType = extension.GetType();
			if (!_extensions.ContainsKey(extensionType)) 
			{
				_extensions.Add (extensionType, extension);
				_logger.Debug("Installing extension {0}", extension);
				extension.Extend (_context);
			}
		}

		public void Install(object obj)
		{
			_context.injectionBinder.Bind<IExtension>().To(obj);
			IExtension extension = _context.injectionBinder.GetInstance<IExtension> ();
			_context.injectionBinder.Unbind<IExtension> ();
			Install(extension);
		}

		public void Destroy()
		{
			_extensions.Clear();
		}
	}
}