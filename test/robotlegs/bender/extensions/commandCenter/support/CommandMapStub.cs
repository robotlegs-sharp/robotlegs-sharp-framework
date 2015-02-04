using System;
using robotlegs.bender.extensions.commandCenter.api;
using Moq;

namespace robotlegs.bender.extensions.commandCenter.support
{
	public class CommandMapStub
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public virtual object KeyFactory(params object[] args)
		{
			string s = "";
			for (int i = 0; i < args.Length; i++)
			{
				s += args[i].ToString ();
				if (i < args.Length - 1)
					s += "::";
			}
			return s;
		}

		public virtual ICommandTrigger TriggerFactory(params object[] args)
		{
			return new Mock<ICommandTrigger> ().Object;
		}

		public virtual void Hook(params object[] args)
		{

		}
	}
}

