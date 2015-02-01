using System;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.mediatorMap.impl.support
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

