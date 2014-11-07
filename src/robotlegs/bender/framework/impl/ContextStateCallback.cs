using System;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	public class ContextStateCallback
	{
		public delegate void CallbackDelegate ();
		private List<CallbackDelegate> callbacks;

		public ContextStateCallback ()
		{
			callbacks = new List<CallbackDelegate> ();
		}

		public void AddCallback(CallbackDelegate callback)
		{
			callbacks.Add (callback);
		}

		public void ProcessCallbacks()
		{
			foreach (CallbackDelegate callback in callbacks) 
				callback();
		}

		public void RemoveCallback(CallbackDelegate callback)
		{
			callbacks.Remove (callback);
		}
	}
}

