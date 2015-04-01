//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System;

namespace robotlegs.bender.extensions.matching
{

	/**
	 * A Package Matcher matches types in a given package
	 */
	public class NamespaceMatcher : ITypeMatcher
	{

		/*============================================================================*/
		/* Protected Properties                                                       */
		/*============================================================================*/

		protected List<string> _anyOfPackages = new List<string>();

		protected List<string> _noneOfPackages = new List<string>();

		protected string _requirePackage;

		protected ITypeFilter _typeFilter;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/**
		 * @inheritDoc
		 */
		public ITypeFilter CreateTypeFilter()
		{
			return _typeFilter == null ? _typeFilter = BuildTypeFilter () : _typeFilter;
		}

		/**
		 * The full package that is required
		 * @param fullPackage
		 * @return Self
		 */
		public NamespaceMatcher Require(string fullPackage)
		{
			if (_typeFilter != null)
				ThrowSealedMatcherError();

			if (_requirePackage != null)
				throw new Exception("You can only set one required package on this PackageMatcher (two non-nested packages cannot both be required, and nested packages are redundant.)");

			_requirePackage = fullPackage;
			return this;
		}

		/**
		 * Any packages that an item might be declared
		 */
		public NamespaceMatcher AnyOf(params string[]  packages)
		{

			pushAddedPackagesTo(packages, _anyOfPackages);
			return this;
		}

		/**
		 * Packages that an item must not live in
		 */
		public NamespaceMatcher NoneOf(params string[]  packages)
		{
			pushAddedPackagesTo(packages, _noneOfPackages);
			return this;
		}

		/**
		 * Locks this matcher
		 */
		public void Lock()
		{
			CreateTypeFilter();
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected ITypeFilter BuildTypeFilter()
		{
			if (((_requirePackage == null) || _requirePackage.Length == 0) &&
				(_anyOfPackages.Count == 0) &&
				(_noneOfPackages.Count == 0))
			{
				throw new TypeMatcherException(TypeMatcherException.EMPTY_MATCHER);
			}
			return new NamespaceFilter(_requirePackage, _anyOfPackages, _noneOfPackages);
		}

		protected void pushAddedPackagesTo(string[] packages, List<string> targetSet)
		{
			if (_typeFilter != null)
				ThrowSealedMatcherError();

			targetSet.AddRange (packages);
		}

		protected void ThrowSealedMatcherError()
		{
			throw new Exception("This TypeMatcher has been sealed and can no longer be configured");
		}
	}
}
