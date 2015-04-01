//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.viewManager.api
{
	public interface IViewManager
	{
		void AddContainer(object container);
		void RemoveContainer(object container);
		void SetFallbackContainer (object container);
		void RemoveFallbackContainer ();
		void AddViewHandler(IViewHandler handler);
		void RemoveViewHandler(IViewHandler handler);
		void RemoveAllHandlers();
		event Action<object> ContainerAdded;
		event Action<object> ContainerRemoved;
		List<object> Containers { get; }
	}
}

