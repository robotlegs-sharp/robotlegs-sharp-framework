using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.api;


namespace robotlegs.bender.extensions.mediatorMap.impl.support
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