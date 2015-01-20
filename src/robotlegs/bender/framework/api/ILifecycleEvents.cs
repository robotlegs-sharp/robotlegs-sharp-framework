using System;

namespace robotlegs.bender.framework.api
{
	public interface ILifecycleEvents
	{
		event Action PRE_INITIALIZE;
		event Action INITIALIZE;
		event Action POST_INITIALIZE;

		event Action PRE_SUSPEND;
		event Action SUSPEND;
		event Action POST_SUSPEND;

		event Action PRE_RESUME;
		event Action RESUME;
		event Action POST_RESUME;

		event Action PRE_DESTROY;
		event Action DESTROY;
		event Action POST_DESTROY;
	}
}

