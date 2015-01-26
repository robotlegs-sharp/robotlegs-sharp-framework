using System;

namespace robotlegs.bender.framework.api
{
	public interface ILifecycleEvent
	{
		event Action<Exception> ERROR;

		event Action STATE_CHANGE;

		event Action<object> PRE_INITIALIZE;
		event Action<object> INITIALIZE;
		event Action<object> POST_INITIALIZE;

		event Action<object> PRE_SUSPEND;
		event Action<object> SUSPEND;
		event Action<object> POST_SUSPEND;

		event Action<object> PRE_RESUME;
		event Action<object> RESUME;
		event Action<object> POST_RESUME;

		event Action<object> PRE_DESTROY;
		event Action<object> DESTROY;
		event Action<object> POST_DESTROY;
	}
}

