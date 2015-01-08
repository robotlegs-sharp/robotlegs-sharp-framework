using System;

namespace robotlegs.bender.framework.api
{
	public enum LifecycleState
	{
		UNINITIALIZED,
		INITIALIZING,
		ACTIVE,
		SUSPENDING,
		SUSPENDED,
		RESUMING,
		DESTROYING,
		DESTROYED
	}
}

