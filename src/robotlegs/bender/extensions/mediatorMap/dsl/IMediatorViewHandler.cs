//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.mediatorMap.dsl
{
	public interface IMediatorViewHandler : IViewHandler
	{
		void AddMapping (IMediatorMapping mapping);

		void RemoveMapping (IMediatorMapping mapping);
	}
}

