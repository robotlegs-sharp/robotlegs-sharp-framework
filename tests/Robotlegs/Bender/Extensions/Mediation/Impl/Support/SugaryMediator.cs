//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Bundles.MVCS;
using System;
using Robotlegs.Bender.Extensions.EventManagement.API;


namespace Robotlegs.Bender.Extensions.Mediation.Impl.Support
{

	public class SugaryMediator : Mediator
	{

		public override void Initialize()
		{

		}

		public override void Destroy ()
		{

		}

		public void Try_addViewListener(Enum eventType, Action callback)
		{
			AddViewListener (eventType, callback);
		}

		public void Try_addContextListener(Enum eventType, Action callback)
		{
			AddContextListener(eventType, callback);
		}

		public void Try_removeViewListener(Enum eventType, Action callback)
		{
			RemoveViewListener(eventType, callback);
		}

		public void Try_removeContextListener(Enum eventType, Action callback)
		{
			RemoveContextListener(eventType, callback);
		}

		public void Try_dispatch(IEvent evt)
		{
			Dispatch(evt);
		}

	}

}