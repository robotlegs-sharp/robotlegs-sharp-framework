//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.viewManager.api
{
	public interface IStageCrawler
	{
		ContainerBinding Binding { set; }
		void Scan(object view);
	}
}

