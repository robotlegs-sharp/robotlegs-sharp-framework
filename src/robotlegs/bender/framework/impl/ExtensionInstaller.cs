//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;
using System.Reflection;

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

			object extension = CreateInstance (type);
			Install (extension);
		}

		public void Install(object extension)
		{
			Type type = extension.GetType();

			if (_types.ContainsKey(type))
				return;

			_logger.Debug("Installing extension {0}", extension);
			_types[type] = true;

			if (extension is IExtension)
			{
				// This check is to prevent TargetInvokationExecptions when an extension extend method errors
				(extension as IExtension).Extend (_context);
			}
			else
			{
				MethodInfo method = type.GetMethod ("Extend");
				method.Invoke (extension, new object[]{ (_context) });
			}
		}

		public void Destroy()
		{
			_types.Clear();
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private object CreateInstance(Type type)
		{
			// Get the constructor with the least amount of parameters
			int maxParameters = int.MaxValue;
			ConstructorInfo[] constructors = type.GetConstructors ();
			ConstructorInfo constructorToInject = null;
			foreach (ConstructorInfo constructor in constructors)
			{
				int paramsLength = constructor.GetParameters ().Length;
				if (paramsLength < maxParameters)
				{
					constructorToInject = constructor;
					maxParameters = paramsLength;
				}
			}

			ParameterInfo[] parameters = constructorToInject.GetParameters ();
			int parametersLength = parameters.Length;
			object[] args = new object[parametersLength];
			for (int i = 0; i < parametersLength; i++)
			{
				args [i] = parameters [i].DefaultValue;
			}

			return constructorToInject.Invoke (args);
		}
	}
}