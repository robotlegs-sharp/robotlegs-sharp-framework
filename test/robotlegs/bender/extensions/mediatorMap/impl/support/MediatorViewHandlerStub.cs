using System;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.mediatorMap.impl.support
{
	public class MediatorViewHandlerStub : IViewHandler
	{
		public virtual void AddMapping(IMediatorMapping mapping)
		{
		}

		public virtual void RemoveMapping(IMediatorMapping mapping)
		{
		}

		public virtual void HandleView (object view, Type type)
		{	
		}
	}
}

