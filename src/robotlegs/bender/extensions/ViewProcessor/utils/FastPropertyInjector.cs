//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Utils
{
	/// <summary>
	/// Avoids view reflection by using a provided map 
	/// of property names to dependency types
	/// </summary>
	public class FastPropertyInjector
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Dictionary<string, Type> _fieldTypesByName;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		/// <summary>
		/// <p>Creates a Fast Property Injection Processor</p>
		/// <br/>
		/// <code>
		/// 	new FastPropertyInjector(
		/// 	new PropertyValueInjector(
		///			new Dictionary<string, Type> ()
		/// 		{
		///				{ "userService", typeof(IUserService) }, 
		///				{ "userPM", typeof(UserPM) }
		///			};
		/// 	);
		/// </code>
		/// </summary>
		/// <param name="propertyTypesByName">A map of property names to dependency types</param>
		public FastPropertyInjector(Dictionary<string, Type> fieldTypesByName)
		{
			_fieldTypesByName = fieldTypesByName;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Process(Object view, Type type, IInjector injector)
		{
			foreach (String fieldName in _fieldTypesByName.Keys)
			{
				FieldInfo field = view.GetType().GetField(fieldName);
				if (field != null)
				{
					object valueToInject = injector.GetInstance(_fieldTypesByName[fieldName]);
					if (valueToInject != null)
					{
						field.SetValue(view, valueToInject);
					}
				}
			}
		}

		public void Unprocess(Object view, Type type, IInjector injector)
		{
		}
	}
}

