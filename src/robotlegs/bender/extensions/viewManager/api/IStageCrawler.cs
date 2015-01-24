using System;
using robotlegs.bender.extensions.viewManager.impl;

namespace robotlegs.bender.extensions.viewManager.api
{
	public interface IStageCrawler
	{
		ContainerBinding Binding { set; }
		void Scan(object view);
	}
}

