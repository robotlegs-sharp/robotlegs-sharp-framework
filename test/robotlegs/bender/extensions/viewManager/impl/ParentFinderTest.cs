using System;
using System.Collections.Generic;
using NUnit.Framework;
using robotlegs.bender.extensions.viewManager.support;

namespace robotlegs.bender.extensions.viewManager.impl
{
	[TestFixture]
	public class ParentFinderTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IParentFinder parentFinder;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			parentFinder = new SupportParentFinder ();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Contains_Should_Find_Child()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			bool contains = parentFinder.Contains (searchTrees [1], searchTrees [1].children [3]);
			Assert.That (contains, Is.True);
		}

		[Test]
		public void Contains_Should_Not_Find_Sibling()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			bool contains = parentFinder.Contains (searchTrees [1].children[1], searchTrees [1].children [2]);
			Assert.That (contains, Is.False);
		}

		[Test]
		public void Contains_Should_Not_Find_Parent()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			bool contains = parentFinder.Contains (searchTrees [1].children[0], searchTrees [1]);
			Assert.That (contains, Is.False);
		}

		[Test]
		public void Contains_Should_Not_Find_Itself()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			bool contains = parentFinder.Contains (searchTrees [1], searchTrees [1]);
			Assert.That (contains, Is.False);
		}

		[Test]
		public void Find_Parent_Should_Find_Parent()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			Dictionary<object, ContainerBinding> dict = CreateDict (new object[]{ searchTrees[0], searchTrees [1], searchTrees[2], searchTrees[3] });
			object parent = parentFinder.FindParent (searchTrees [3].children [1].children [2].children [1], dict);
			Assert.That (parent, Is.EqualTo (searchTrees [3]));
		}

		[Test]
		public void Find_Parent_Should_Find_First_Parent()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			Dictionary<object, ContainerBinding> dict = CreateDict (new object[]{ searchTrees[3], searchTrees[3].children[1].children[2], searchTrees[3].children[1] });
			object parent = parentFinder.FindParent (searchTrees [3].children [1].children [2].children [1], dict);
			Assert.That (parent, Is.EqualTo (searchTrees [3].children[1].children[2]));
		}

		[Test]
		public void Find_Parent_Should_Not_Find_Child()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			Dictionary<object, ContainerBinding> dict = CreateDict (new object[]{ searchTrees[3].children[1].children[0], searchTrees[3].children[1].children[1], searchTrees[3].children[1].children[1].children[1] });
			object parent = parentFinder.FindParent (searchTrees [3].children [1], dict);
			Assert.That (parent, Is.Null);
		}

		[Test]
		public void Find_Parent_Should_Not_Find_Sibling()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees (5, 4);
			Dictionary<object, ContainerBinding> dict = CreateDict (new object[]{ searchTrees[3].children[0], searchTrees[3].children[2], searchTrees[3].children[3] });
			object parent = parentFinder.FindParent (searchTrees [3].children [1], dict);
			Assert.That (parent, Is.Null);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private List<TreeContainerSupport> CreateTrees(uint tree_depth, uint tree_width)
		{
			List<TreeContainerSupport> trees = new List<TreeContainerSupport>();
			for (uint i = 0; i < tree_width; i++)
			{
				TreeContainerSupport nextTree = new TreeContainerSupport(tree_depth, tree_width);
				trees.Add(nextTree);
			}
			return trees;
		}

		private Dictionary<object, ContainerBinding> CreateDict(object[] objects)
		{
			Dictionary<object, ContainerBinding> dict = new Dictionary<object, ContainerBinding> ();
			foreach (object obj in objects)
			{
				dict.Add (obj, new ContainerBinding(obj));
			}
			return dict;
		}
	}
}

