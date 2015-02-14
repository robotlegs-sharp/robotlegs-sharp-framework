using System;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.eventDispatcher.support
{
	public class CustomEvent : Event
	{
		public enum Type
		{
			A,
			B,
			C
		}

		public string message;

		public CustomEvent (Type type, string message) : base(type)
		{
			this.message = message;
		}
	}
}

