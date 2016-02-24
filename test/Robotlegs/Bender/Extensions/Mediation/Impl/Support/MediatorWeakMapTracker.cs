//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Robotlegs.Bender.Extensions.Mediation.API;


namespace Robotlegs.Bender.Extensions.Mediation.Impl.Support
{
	public class MediatorWeakMapTracker
	{

		protected Dictionary<IMediator, bool> _mediators = new Dictionary<IMediator, bool>();

		public Dictionary<IMediator, bool> TrackedMediators
		{
			get
			{
				return _mediators;
			}
		}

		public void TrackMediator(IMediator mediator)
		{
			_mediators.Add (mediator, true);
		}
	}
}