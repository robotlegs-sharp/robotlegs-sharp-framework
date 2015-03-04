using System;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.mediatorMap.dsl
{
	public interface IMediatorViewHandler : IViewHandler
	{
		void AddMapping (IMediatorMapping mapping);

		void RemoveMapping (IMediatorMapping mapping);
	}
}

