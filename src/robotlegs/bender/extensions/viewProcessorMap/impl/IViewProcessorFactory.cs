using System;
using robotlegs.bender.extensions.viewProcessorMap.dsl;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public interface IViewProcessorFactory
	{
		void RunProcessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings);

		void RunUnprocessors(object view, Type type, IEnumerable<IViewProcessorMapping> processorMappings);

		void RunAllUnprocessors();
	}
}

