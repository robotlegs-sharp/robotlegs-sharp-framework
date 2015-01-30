using System;
using robotlegs.bender.framework.api;
using System.Reflection;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.viewProcessorMap.utils
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
		//TODO: Matt Update this code hinting doc so its correct to C# syntax
		/// <summary>
		/// <p>Creates a Fast Property Injection Processor</p>
		/// <br/>
		/// <code>
		/// 	new FastPropertyInjector(
		/// 		new Dictionary<string, Type>(
		/// 			"userService",IUserService 
		///	 			"userPM",UserPM
		/// 		)
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

