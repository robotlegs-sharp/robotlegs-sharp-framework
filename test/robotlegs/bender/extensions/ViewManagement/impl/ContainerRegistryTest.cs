//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewManagement.Support;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement.Impl
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

		[Test]
		public void adding_fallback_handles_regardless()
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
		public void adding_fallback_should_be_found_as_parent()
		{
			object fallback = new object ();
			registry.SetFallbackContainer (fallback);
			object newView = new SupportView ();
			ContainerBinding parentBinding = registry.FindParentBinding (newView);
			Assert.That (parentBinding.Container, Is.EqualTo (fallback));
		}

		[Test]
		public void adding_fallback_should_retain_heirchy()
		{
			List<TreeContainerSupport> searchTrees = CreateTrees(6, 4);

			ContainerBinding firstExpectedBinding = registry.AddContainer(searchTrees[0]);

			registry.AddContainer(searchTrees[1].children[3].children[2].children[3]);
			registry.AddContainer(searchTrees[1].children[3].children[2]);	
			registry.AddContainer(searchTrees[1]);

			object fallbackContainer = new object ();
			registry.SetFallbackContainer (fallbackContainer);

			registry.AddContainer(searchTrees[1].children[3]);

			ContainerBinding topBinding = registry.FindParentBinding(searchTrees [1].children [3].children [2].children [3].children [3]);

			Assert.That(topBinding.Container, Is.EqualTo(searchTrees[1].children[3].children[2].children[3]));
			Assert.That(topBinding.Parent.Container, Is.EqualTo(searchTrees[1].children[3].children[2]));
			Assert.That(topBinding.Parent.Parent.Container, Is.EqualTo(searchTrees[1].children[3]));
			Assert.That(topBinding.Parent.Parent.Parent.Container, Is.EqualTo(searchTrees[1]));
			Assert.That(topBinding.Parent.Parent.Parent.Parent, Is.Not.Null);

			Assert.That(topBinding.Parent.Parent.Parent.Parent.Container, Is.EqualTo(fallbackContainer));
			Assert.That(topBinding.Parent.Parent.Parent.Parent.Parent, Is.Null);
		}

		[Test]
		public void removing_fallback_handles()
		{
			object fallbackContainer = new object ();
			registry.SetFallbackContainer (fallbackContainer);
			registry.RemoveFallbackContainer ();

			Assert.That(registry.FindParentBinding (new object ()), Is.Null);
			Assert.That (registry.FallbackBinding, Is.Null);
		}

		[Test]
		public void adding_two_fallbacks_should_remove_previous()
		{
			SupportContainer fallback1 = new SupportContainer ();
			SupportContainer fallback2 = new SupportContainer ();
			registry.SetFallbackContainer (fallback1);
			registry.SetFallbackContainer (fallback2);
			Assert.That (registry.FallbackBinding.Container, Is.EqualTo (fallback2));
		}

		[Test]
		public void adding_fallback_removes_roots()
		{
			registry.AddContainer (new SupportContainer ());
			registry.AddContainer (new SupportContainer ());

			Assert.That (registry.RootBindings.Count, Is.EqualTo (2));

			ContainerBinding binding = registry.SetFallbackContainer (new object ());
			Assert.That (registry.RootBindings, Is.EqualTo (new List<ContainerBinding> (){ binding }).AsCollection);
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