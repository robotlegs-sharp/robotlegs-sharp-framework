using System;

namespace robotlegs.bender.framework.api
{
	public interface IPinEvent
	{
		event Action<object> Detained;
		event Action<object> Released;
	}
}

