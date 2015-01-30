using System;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.viewProcessorMap.support
{
	public interface ITrackingProcessor
	{
		void Process(object view, Type type, IInjector injector);

		void Unprocess(object view, Type type, IInjector injector);
	}

}

