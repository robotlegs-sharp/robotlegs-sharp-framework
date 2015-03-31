//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
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

