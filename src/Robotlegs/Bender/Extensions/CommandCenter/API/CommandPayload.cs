//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

 using System;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.CommandCenter.API
{
	public class CommandPayload
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/
		
		private List<object> _values;
		
		/**
		 * Ordered list of values
		 */
		public List<object> Values
		{
			get
			{
				return _values;
			}
		}
		
		private List<Type> _classes;
		
		/**
		 * Ordered list of value classes
		 */
		public List<Type> Classes
		{
			get
			{
				return _classes;
			}
		}
		
		/**
		 * The number of payload items
		 */
		public uint length
		{
			get
			{
				return _classes != null ? (uint)_classes.Count : 0;
			}
		}
		
		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/
		
		/**
		 * Creates a command payload
		 * @param values Optional values
		 * @param classes Optional classes
		 */
		public CommandPayload(IEnumerable<object> values, IEnumerable<Type> classes)
		{
			if (values != null)
			{
				_values = new List<object> (values);
			}
			if (classes != null) 
			{
				_classes = new List<Type> (classes);
			}
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/
		
		/**
		 * Adds an item to this payload
		 * @param payloadValue The value
		 * @param payloadClass The class of the value
		 * @return Self
		 */
		public CommandPayload AddPayload(object payloadValue, Type payloadClass)
		{
			if (_values == null)
				_values = new List<object>();

			_values.Add(payloadValue);

			if (_classes == null)
				_classes = new List<Type>();
				
			_classes.Add(payloadClass);
			
			return this;
		}
		
		/**
		 * Does this payload have any items?
		 * @return Boolean
		 */
		public bool HasPayload()
		{
			// todo: the final clause will make this fail silently
			// todo: rethink
			return _values != null && _values.Count > 0
				&& _classes != null && _classes.Count == _values.Count;
		}
	}
}