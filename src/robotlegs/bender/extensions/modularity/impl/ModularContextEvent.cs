using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.modularity.impl
{
	public class ModularContextEvent : IEvent
	{
		private Type _type;
		private IContext _context;
		private object _contextView;

		public enum Type
		{
			CONTEXT_ADD,
			CONTEXT_REMOVE
		}

		public Enum type
		{
			get
			{
				return _type;
			}
		}

		public IContext Context
		{
			get
			{
				return _context;
			}
		}

		public object ContextView
		{
			get
			{
				return _contextView;
			}
		}

		public ModularContextEvent(ModularContextEvent.Type type, IContext context, object contextView)
		{
			_type = type;
			_context = context;
			_contextView = contextView;
		}
	}
}