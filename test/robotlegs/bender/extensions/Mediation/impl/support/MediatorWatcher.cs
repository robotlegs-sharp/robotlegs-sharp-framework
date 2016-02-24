//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.Mediation.Impl.Support
{
	public class MediatorWatcher
	{
		protected List<string> _notifications = new List<string>();

		public List<string> Notifications
		{
			get
			{
				return _notifications;
			}
		}

		public void Notify(string message)
		{
			_notifications.Add(message);
		}
	}
}

