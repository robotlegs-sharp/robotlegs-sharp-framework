using System.Collections.Generic;
using System;

namespace robotlegs.bender.extensions.viewProcessorMap.support
{
	public class Processor
	{
		[Inject("timingTracker")]
		public List<Type> timingTracker;

		public void Process(object view, Type type, object injector)
		{
			timingTracker.Add(typeof(Processor));
		}

		public void Unprocess(object view, Type type, object injector)
		{

		}
	}
}