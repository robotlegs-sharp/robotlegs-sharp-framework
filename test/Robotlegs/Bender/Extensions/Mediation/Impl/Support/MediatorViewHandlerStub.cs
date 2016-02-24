//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.ViewManagement.API;
using Robotlegs.Bender.Extensions.Mediation.API;

namespace Robotlegs.Bender.Extensions.Mediation.Impl.Support
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

