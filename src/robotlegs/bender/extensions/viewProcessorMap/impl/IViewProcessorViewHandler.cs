using System;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public interface IViewProcessorViewHandler
	{
		void AddMapping (IViewProcessorMapping mapping);

		void RemoveMapping (IViewProcessorMapping mapping);

		void ProcessItem(object item, Type type);

		void UnprocessItem(object item, Type type);
	}
}

