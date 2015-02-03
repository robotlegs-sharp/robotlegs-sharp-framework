using System;
using NUnit.Framework;
using robotlegs.bender.framework.unity.extensions.viewManager.impl;
using robotlegs.bender.extensions.viewManager.support;
using System.Collections.Generic;
using robotlegs.bender.extensions.viewManager.api;

namespace robotlegs.bender.extensions.viewManager.impl
{
	[TestFixture]
	public class ContainerRegistryTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ContainerRegistry registry;

		private SupportContainer container;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			registry = new ContainerRegistry();
			registry.SetParentFinder (new SupportParentFinder());
			container = new SupportContainer();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Parent_Finder_Positive()
		{
			bool contains = true;
			object parent = new object();
			registry.SetParentFinder(new CustomParentFinder(contains, parent));
			List<ContainerBinding> nullList = null;
			object expectedParent = registry.FindParent (null, nullList);
			object expectedContains = registry.Contains (null, nullList);
			Assert.That (contains, Is.EqualTo (expectedContains));
			Assert.That (parent, Is.EqualTo (parent));
		}

		[Test]
		public void Parent_Finder_Negative()
		{
			bool contains = false;
			object parent = null;
			List<ContainerBinding> nullList = null;
			registry.SetParentFinder(new CustomParentFinder(contains, parent));
			object expectedParent = registry.FindParent (null, nullList);
			object expectedContains = registry.Contains (null, nullList);
			Assert.That (contains, Is.EqualTo (expectedContains));
			Assert.That (parent, Is.EqualTo (parent));
		}

		[Test]
		public void addContainer()
		{
			registry.AddContainer(container);
		}


		[Test]
		public void finds_correct_nearest_interested_container_view_and_returns_its_binding()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);
			registry.AddContainer(searchTrees[0]);
			registry.AddContainer(searchTrees[1]);

			TreeContainerSupport correctTree = searchTrees[1];

			SupportContainer searchItem = correctTree.children[3].children[3].children[3].children[3];

			ContainerBinding result = registry.FindParentBinding(searchItem);

			Assert.That(correctTree, Is.EqualTo(result.Container), "Finds correct nearest interested container view and returns its binding");
		}

		[Test]
		public void binding_returns_with_correct_interested_parent_chain()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);
			registry.AddContainer(searchTrees[0]);
			registry.AddContainer(searchTrees[1]);
			registry.AddContainer(searchTrees[1].children[3]);

			SupportContainer searchItem = searchTrees[1].children[3].children[3].children[3].children[3];

			ContainerBinding result = registry.FindParentBinding(searchItem);

			Assert.That(searchTrees[1].children[3], Is.EqualTo(result.Container), "Binding returns with correct container view");
			Assert.That(searchTrees[1], Is.EqualTo(result.Parent.Container), "Binding returns with correct container parent view");
			Assert.That(result.Parent.Parent, Is.Null, "Further parents are null");
		}

		[Test]
		public void binding_returns_with_correct_interested_parent_chain_if_interested_views_added_in_wrong_order()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);
			ContainerBinding a = registry.AddContainer(searchTrees[0]);
			ContainerBinding b = registry.AddContainer(searchTrees[1].children[3]);
			ContainerBinding c = registry.AddContainer(searchTrees[1]);

			SupportContainer searchItem = searchTrees[1].children[3].children[3].children[3].children[3];

			ContainerBinding result = registry.FindParentBinding(searchItem);

			Assert.That(searchTrees[1].children[3], Is.EqualTo(result.Container), "Binding returns with correct container view");
			Assert.That(searchTrees[1], Is.EqualTo(result.Parent.Container), "Binding returns with correct container parent view");
			Assert.That(result.Parent.Parent, Is.Null, "Further parents are null");
		}

		[Test]
		public void binding_returns_with_correct_interested_parent_chain_if_interested_views_added_in_wrong_order_with_gaps()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);
			registry.AddContainer(searchTrees[0]);
			registry.AddContainer(searchTrees[1].children[3].children[2]);
			registry.AddContainer(searchTrees[1]);

			SupportContainer searchItem = searchTrees[1].children[3].children[2].children[3].children[3];

			ContainerBinding result = registry.FindParentBinding(searchItem);

			Assert.That(searchTrees[1].children[3].children[2], Is.EqualTo(result.Container), "Binding returns with correct container view");
			Assert.That(searchTrees[1], Is.EqualTo(result.Parent.Container), "Binding returns with correct container parent view");
			Assert.That(result.Parent.Parent, Is.Null, "Further parents are null");
		}

		[Test]
		public void binding_returns_with_correct_interested_parent_chain_after_removal()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);
			registry.AddContainer(searchTrees[0]);
			registry.AddContainer(searchTrees[1]);
			registry.AddContainer(searchTrees[1].children[3].children[2].children[3]);
			registry.AddContainer(searchTrees[1].children[3].children[2]);
			registry.AddContainer(searchTrees[1].children[3]);

			registry.RemoveContainer(searchTrees[1].children[3].children[2]);

			SupportContainer searchItem = searchTrees[1].children[3].children[2].children[3].children[3];

			ContainerBinding result = registry.FindParentBinding(searchItem);

			Assert.That(searchTrees[1].children[3].children[2].children[3], Is.EqualTo(result.Container), "Binding returns with correct container view");
			Assert.That(searchTrees[1].children[3], Is.EqualTo(result.Parent.Container), "Binding returns with correct container parent view");
			Assert.That(searchTrees[1], Is.EqualTo(result.Parent.Parent.Container), "Binding returns with correct container parent parent view");
			Assert.That(result.Parent.Parent.Parent, Is.Null, "Further parents are null");
		}

		[Test]
		public void returns_null_if_search_item_is_not_inside_an_included_view()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);
			registry.AddContainer(searchTrees[0]);
			registry.AddContainer(searchTrees[1]);
			registry.AddContainer(searchTrees[1].children[3].children[2].children[3]);
			registry.AddContainer(searchTrees[1].children[3].children[2]);
			registry.AddContainer(searchTrees[1].children[3]);

			registry.RemoveContainer(searchTrees[1].children[3].children[2]);

			SupportContainer searchItem = searchTrees[2].children[3].children[2].children[3].children[3];

			ContainerBinding result = registry.FindParentBinding(searchItem);

			Assert.That(result, Is.Null, "Returns null if not inside an included view");
		}

		[Test]
		public void returns_root_container_view_bindings_one_item()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(1, 1);
			ContainerBinding expectedBinding = registry.AddContainer(searchTrees[0]);
			List<ContainerBinding> expectedRootBindings = new List<ContainerBinding>{expectedBinding};

			Assert.That (expectedRootBindings, Is.EquivalentTo(registry.RootBindings), "Returns root container view bindings one item");
		}

		[Test]
		public void returns_root_container_view_bindings_many_items()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);

			ContainerBinding firstExpectedBinding = registry.AddContainer(searchTrees[0]);

			registry.AddContainer(searchTrees[1].children[3].children[2].children[3]);
			registry.AddContainer(searchTrees[1].children[3].children[2]);

			ContainerBinding secondExpectedBinding = registry.AddContainer(searchTrees[1]);

			registry.AddContainer(searchTrees[1].children[3]);

			List<ContainerBinding> expectedRootBindings = new List<ContainerBinding>{firstExpectedBinding, secondExpectedBinding};

			Assert.That (expectedRootBindings, Is.EquivalentTo(registry.RootBindings), "Returns root container view bindings one item");
		}

		[Test]
		public void returns_root_container_view_bindings_many_items_after_removals()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(5, 4);

			ContainerBinding firstExpectedBinding = registry.AddContainer(searchTrees[0]);

			registry.AddContainer(searchTrees[1].children[3].children[2].children[3]);
			registry.AddContainer(searchTrees[1].children[3].children[2]);
			registry.AddContainer(searchTrees[1]);

			ContainerBinding secondExpectedBinding = registry.AddContainer(searchTrees[1].children[3]);

			registry.RemoveContainer(searchTrees[1]);

			List<ContainerBinding> expectedRootBindings = new List<ContainerBinding>{firstExpectedBinding, secondExpectedBinding};

			Assert.That (expectedRootBindings, Is.EquivalentTo(registry.RootBindings), "Returns root container view bindings one item");
		}

		[Test]
		public void adding_container_dispatches_event()
		{
			int callCount = 0;
			registry.ContainerAdd += delegate(object obj) {
				callCount++;
			};
			registry.AddContainer(container);
			registry.AddContainer(container);
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void removing_container_dispatches_event()
		{
			int callCount = 0;
			registry.ContainerRemove += delegate(object obj) {
				callCount++;
			};
			registry.AddContainer(container);
			registry.RemoveContainer(container);
			registry.RemoveContainer(container);
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void adding_root_container_dispatches_event()
		{
			int callCount = 0;
			registry.RootContainerAdd += delegate(object obj) {
				callCount++;
			};
			registry.AddContainer(container);
			Assert.That(callCount, Is.EqualTo(1));
		}

		[Test]
		public void empty_binding_is_removed()
		{
			IViewHandler handler = new CallbackViewHandler();
			registry.AddContainer(container).AddHandler(handler);
			registry.GetBinding(container).RemoveHandler(handler);
			Assert.That(registry.GetBinding(container), Is.Null);
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
	}
}