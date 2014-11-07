using System;
using robotlegs.bender.framework.api;
using strange.extensions.sequencer.api;
using strange.extensions.sequencer.impl;

namespace strange.extensions.sequencer
{
	public class SequencerExtension : IExtension
	{
		public void Extend(IContext context)
		{
			context.injectionBinder.Bind<ISequencer>().To<EventSequencer>().ToSingleton();
		}
	}
}

