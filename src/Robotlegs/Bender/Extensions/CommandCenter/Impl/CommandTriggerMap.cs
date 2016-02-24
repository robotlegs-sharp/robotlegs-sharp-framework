//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System.Collections.Generic;
using Robotlegs.Bender.Extensions.CommandCenter.API;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	public class CommandTriggerMap
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		//TODO: Make this part of the interface
		public delegate object KeyFactory (params object[] args);

		public delegate ICommandTrigger TriggerFactory (params object[] args);

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Dictionary<object, ICommandTrigger> _triggers = new Dictionary<object, ICommandTrigger>();

		private KeyFactory _keyFactory;

		private TriggerFactory _triggerFactory;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Creates a command trigger map
		/// </summary>
		/// <param name="keyFactory">Factory function to creates keys</param>
		/// <param name="triggerFactory">Factory function to create triggers</param>
		public CommandTriggerMap (KeyFactory keyFactory, TriggerFactory triggerFactory)
		{
			_keyFactory = keyFactory;
			_triggerFactory = triggerFactory;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public ICommandTrigger GetTrigger (params object[] args)
		{
			object key = GetKey(args);
			return _triggers.ContainsKey (key) ? _triggers [key] : _triggers[key] = CreateTrigger (args);
		}

		public ICommandTrigger RemoveTrigger(params object[] args)
		{
			return DestroyTrigger (GetKey (args));
		}


		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private object GetKey(object[] args)
		{
			return _keyFactory (args);
		}

		private ICommandTrigger CreateTrigger(object[] args)
		{
			return _triggerFactory (args);
		}

		private ICommandTrigger DestroyTrigger(object key)
		{
			if (!_triggers.ContainsKey (key))
				return null;
			ICommandTrigger trigger = _triggers [key];
			trigger.Deactivate ();
			_triggers.Remove (key);
			return trigger;
		}
	}
}