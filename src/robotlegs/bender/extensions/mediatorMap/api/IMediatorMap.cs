//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.mediatorMap.dsl;

namespace robotlegs.bender.extensions.mediatorMap.api
{
	/// <summary>
	/// The Mediator Map allows you to bind Mediators to objects
	/// </summary>
	public interface IMediatorMap
	{
		/// <summary>
		/// Maps a matcher that will be tested against incoming items to be handled.
		/// </summary>
		/// <returns>the mapper so that you can continue the mapping.</returns>
		/// <param name="matcher">The type or package matcher specifying the rules for matching.</param>
		IMediatorMapper MapMatcher(ITypeMatcher matcher);

		/// <summary>
		/// Maps a type that will be tested against incoming items to be handled.
		/// Under the hood this will create a TypeMatcher for this type.
		/// </summary>
		/// <returns>the mapper so that you can continue the mapping.</returns>
		/// <param name="type">The class or interface to be matched against.</param>
		IMediatorMapper Map<T>();
		IMediatorMapper Map(Type type);

		/// <summary>
		/// Unmaps the matcher.
		/// </summary>
		/// <returns>the unmapper so that you can continue the unmapping.</returns>
		/// <param name="matcher">The type or package matcher specifying the rules for matching.</param>
		IMediatorUnmapper UnmapMatcher(ITypeMatcher matcher);

		/// <summary>
		/// Unmap the specified type.
		/// </summary>
		/// <returns>the unmapper so that you can continue the unmapping.</returns>
		/// <param name="type">The class or interface to be matched against.</param>
		IMediatorUnmapper Unmap<T>();
		IMediatorUnmapper Unmap(Type type);

		/// <summary>
		/// Mediates an item directly. If the item matches any mapped matchers or types then it will be mediated according to those mappings.
		/// </summary>
		/// <param name="item">The item to create mediators for.</param>
		void Mediate(object item);

		/// <summary>
		/// Removes the mediators for an item if there are any.
		/// </summary>
		/// <param name="item">The item to remove mediators for.</param>
		void Unmediate(object item);

		/// <summary>
		/// Removes all mediators
		/// </summary>
		void UnmediateAll();
	}
}

