//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using robotlegs.bender.extensions.eventDispatcher.api;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.extensions.eventDispatcher.impl
{
	public class LifecycleEventRelay
	{
		/*============================================================================*/
		/* Private Static Properties                                                  */
		/*============================================================================*/

		private IEventDispatcher _destination;

		private ILifecycleEvent _source;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public LifecycleEventRelay (ILifecycleEvent source, IEventDispatcher destination)
		{
			_source = source;
			_destination = destination;

			_source.STATE_CHANGE += HandleStateChange;

			_source.PRE_INITIALIZE += HandlePreInitialize;
			_source.INITIALIZE += HandleInitialize;
			_source.POST_INITIALIZE += HandlePostInitialize;

			_source.PRE_SUSPEND += HandlePreSuspend;
			_source.SUSPEND += HandleSuspend;
			_source.POST_SUSPEND += HandlePostSuspend;

			_source.PRE_RESUME += HandlePreResume;
			_source.RESUME += HandleResume;
			_source.POST_RESUME += HandlePostResume;

			_source.PRE_DESTROY += HandlePreDestroy;
			_source.DESTROY += HandleDestroy;
			_source.POST_DESTROY += HandlePostDestroy;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Destroy()
		{
			_source.STATE_CHANGE -= HandleStateChange;

			_source.PRE_INITIALIZE -= HandlePreInitialize;
			_source.INITIALIZE -= HandleInitialize;
			_source.POST_INITIALIZE -= HandlePostInitialize;

			_source.PRE_SUSPEND -= HandlePreSuspend;
			_source.SUSPEND -= HandleSuspend;
			_source.POST_SUSPEND -= HandlePostSuspend;

			_source.PRE_RESUME -= HandlePreResume;
			_source.RESUME -= HandleResume;
			_source.POST_RESUME -= HandlePostResume;

			_source.PRE_DESTROY -= HandlePreDestroy;
			_source.DESTROY -= HandleDestroy;
			_source.POST_DESTROY -= HandlePostDestroy;

			_source = null;
			_destination = null;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void HandleStateChange()
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.STATE_CHANGE));
		}

		private void HandlePreInitialize(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.PRE_INITIALIZE));
		}

		private void HandleInitialize(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.INITIALIZE));
		}

		private void HandlePostInitialize(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.POST_INITIALIZE));
		}

		private void HandlePreSuspend(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.PRE_SUSPEND));
		}

		private void HandleSuspend(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.SUSPEND));
		}

		private void HandlePostSuspend(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.POST_SUSPEND));
		}

		private void HandlePreResume(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.PRE_RESUME));
		}

		private void HandleResume(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.RESUME));
		}

		private void HandlePostResume(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.POST_RESUME));
		}

		private void HandlePreDestroy(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.PRE_DESTROY));
		}

		private void HandleDestroy(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.DESTROY));
		}

		private void HandlePostDestroy(object target)
		{
			_destination.Dispatch (new LifecycleEvent (LifecycleEvent.Type.POST_DESTROY));
		}
	}
}

