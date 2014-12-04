using System;

namespace robotlegs.bender.framework.impl.guardSupport
{
	public class JustTheMiddleManGuard
	{
		[Inject]
		public BossGuard bossDecision;

		public bool Approve()
		{
			return bossDecision.Approve();
		}
	}
}

