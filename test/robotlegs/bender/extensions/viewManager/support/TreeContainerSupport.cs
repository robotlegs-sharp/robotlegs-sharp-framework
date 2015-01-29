using System;
using robotlegs.bender.extensions.viewManager.support;
using System.Collections.Generic;

namespace robotlegs.bender
{
	public class TreeContainerSupport : SupportContainer
	{
		// Testable constants
		public List<TreeContainerSupport> children = new List<TreeContainerSupport>();

		public TreeContainerSupport(uint tree_depth, uint tree_width) {
			Populate(tree_depth, tree_width);
		}

		protected void Populate(uint tree_depth, uint tree_width)
		{
			if (tree_depth == 0) 
				return;

			for (uint i = 0; i < tree_width; i++)
			{
				TreeContainerSupport child = new TreeContainerSupport(tree_depth - 1, tree_width);
				children.Add(child);
				AddChild(child);
			}
		}
	}
}

