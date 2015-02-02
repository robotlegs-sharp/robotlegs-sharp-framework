using System;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class CallbackCommand
	{
		[Inject("ExecuteCallback")]
		public Action callback;

		public void Execute()
		{
			callback ();
		}
	}
}

