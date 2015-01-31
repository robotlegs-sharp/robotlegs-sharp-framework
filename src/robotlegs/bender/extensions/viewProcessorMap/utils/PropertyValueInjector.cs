using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;
using System.Reflection;

namespace robotlegs.bender.extensions.viewProcessorMap.utils
{
	/// <summary>
	/// Avoids view reflection by using a provided map
	/// of property names to dependency values
	/// </summary>
	public class PropertyValueInjector
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Dictionary<string,object> _valuesByFieldName;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		/// <summary>
		/// <p>Creates a Value Property Injection Processor</p>
		/// <br/>
		/// <code>
		/// 	new PropertyValueInjector(
		///			new Dictionary<string, object> ()
		/// 		{
		///				{ "userService", myUserService }, 
		///				{ "userPM", myUserPM }
		///			};
		/// 	);
		/// </code>
		/// </summary>
		/// <param name="propertyTypesByName">A dictionary of property names to dependency values</param>

		public PropertyValueInjector(Dictionary<string,object> valuesByFieldName)
		{
			_valuesByFieldName = valuesByFieldName;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Process(object view, Type type, IInjector injector)
		{
			foreach (string fieldName in _valuesByFieldName.Keys)
			{
				FieldInfo field = type.GetField(fieldName);
				if (field != null)
				{
					field.SetValue(view, _valuesByFieldName [fieldName]);
				}
			}
		}

		public void Unprocess(Object view, Type type, IInjector injector)
		{

		}
	}
}