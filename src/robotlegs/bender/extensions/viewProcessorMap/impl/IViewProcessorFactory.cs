using System;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public interface IViewProcessorFactory
	{
		void RunProcessors(object view, Type type, IViewProcessorMapping[] processorMappings);

		void RunUnprocessors(object view, Type type, IViewProcessorMapping[] processorMappings);

		void RunAllUnprocessors();
	}
}

