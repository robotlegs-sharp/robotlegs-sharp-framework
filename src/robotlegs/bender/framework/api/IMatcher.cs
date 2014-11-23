using System;

namespace robotlegs.bender.framework.api
{
	public interface IMatcher
	{
		bool Matches(object item);
	}
}

