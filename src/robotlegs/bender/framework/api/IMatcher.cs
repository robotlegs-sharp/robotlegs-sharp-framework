using System;

namespace strange.context.api
{
	public interface IMatcher
	{
		bool Matches(object item);
	}
}

