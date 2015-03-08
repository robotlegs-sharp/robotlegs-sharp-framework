using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.extensions.eventDispatcher.impl;

namespace robotlegs.bender.extensions.viewManager.support
{
	public class SupportEventView : SupportView
	{
		public IEventDispatcher dispatcher = new EventDispatcher();

		public SupportEventView ()
		{
		}
	}
}

