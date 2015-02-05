using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class ExtensionInstaller
	{
		
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private Dictionary <Type, bool> _types = new Dictionary<Type, bool> ();

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

		public void Install(Type type)
		{
			if (_types.ContainsKey (type))
				return;

			IExtension extension = _context.injector.InstantiateUnmapped(type) as IExtension;
			Install(extension);
		}

		public void Install(IExtension extension)
		{
			Type type = extension.GetType();

			if (_types.ContainsKey(type))
				return;

			_logger.Debug("Installing extension {0}", extension);
			_types[type] = true;
			extension.Extend (_context);
		}

		public void Destroy()
		{
			_types.Clear();
		}
	}
}