//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;


namespace robotlegs.bender.framework.impl
{
	/// <summary>
	/// The object processor, the core for Config Manager.
	/// It takes adds IMatchers which checks if you can process the object with your handler function
	/// </summary>

	public class ObjectProcessor
	{
		private List<ObjectHandler> _handlers = new List<ObjectHandler>();

		public void AddObjectHandler (IMatcher matcher, Action<object> handler)
		{
			_handlers.Add(new ObjectHandler(matcher, handler));
		}

		public void ProcessObject(object obj)
		{
			// I know we try to avoid counting every loop cycle, but I want to be able to add to the handlers during the loop cycle.
			//foreach (ObjectHandler handler in _handlers)
			for(int i = 0; i < _handlers.Count; i++)
			{
				_handlers[i].Handle(obj);
			}
		}

		public void RemoveAllHandlers()
		{
			_handlers.Clear();
		}
	}

	/// <summary>
	/// Object handler
	/// A single instance of checking a IMatcher and then processing your handler function
	/// </summary>

	public class ObjectHandler
	{
		private IMatcher _matcher;
		private Action<object> _handler;

		public ObjectHandler(IMatcher matcher, Action<object> handler)
		{
			_handler = handler;
			_matcher = matcher;
		}
		
		public void Handle(object obj)
		{
			if (_matcher.Matches(obj))
				_handler(obj);
		}
	}
}

