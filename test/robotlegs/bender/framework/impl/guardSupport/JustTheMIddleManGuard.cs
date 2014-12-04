using System;

namespace robotlegs.bender.framework.impl.guardSupport
{
	public class BossGuard
	{
		protected bool _approve = false;

		public BossGuard(bool approve)
		{
			_approve = approve;
		}

		public bool Approve()
		{
			return _approve;
		}
	}
}

