//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.Matching
{
	public class TypeMatcher : ITypeMatcher, ITypeMatcherFactory 
	{
		//TODO: Write Package Matcher / Package Filter
		
		/*============================================================================*/
		/* Protected Properties                                                       */
		/*============================================================================*/

		protected List<Type> allOfTypes = new List<Type>();

		protected List<Type> anyOfTypes = new List<Type>();

		protected List<Type> noneOfTypes = new List<Type>();

		protected ITypeFilter typeFilter;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public TypeMatcher AllOf(params Type[] allOf)
		{
			return AllOf (allOf as IEnumerable<Type>);
		}

		public TypeMatcher AllOf(IEnumerable<Type> allOf)
		{
			PushAddedTypesTo(allOf, allOfTypes);
			return this;
		}

		public TypeMatcher AnyOf(params Type[] anyOf)
		{
			return AnyOf (anyOf as IEnumerable <Type>);
		}

		public TypeMatcher AnyOf(IEnumerable<Type> anyOf)
		{
			PushAddedTypesTo(anyOf, anyOfTypes);
			return this;
		}

		public TypeMatcher NoneOf(params Type[] noneOf)
		{
			return NoneOf (noneOf as IEnumerable <Type>);
		}

		public TypeMatcher NoneOf(IEnumerable<Type> noneOf)
		{
			PushAddedTypesTo(noneOf, noneOfTypes);
			return this;
		}

		public ITypeFilter CreateTypeFilter()
		{
			return typeFilter != null ? typeFilter : typeFilter = BuildTypeFilter();
		}

		public ITypeMatcherFactory Lock()
		{
			CreateTypeFilter();
			return this;
		}

		public TypeMatcher Clone()
		{
			return new TypeMatcher().AllOf(allOfTypes).AnyOf(anyOfTypes).NoneOf(noneOfTypes);
		}
		
		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected ITypeFilter BuildTypeFilter()
		{
			if (allOfTypes.Count == 0 && anyOfTypes.Count == 0 && noneOfTypes.Count == 0)
				throw new TypeMatcherException(TypeMatcherException.EMPTY_MATCHER);

			return new TypeFilter(allOfTypes, anyOfTypes, noneOfTypes);
		}

		protected void PushAddedTypesTo(IEnumerable<Type> types, List<Type> targetSet)
		{
			if (typeFilter != null)
				ThrowSealedMatcherError ();

			targetSet.AddRange (types);
		}

		protected void ThrowSealedMatcherError()
		{
			throw new TypeMatcherException(TypeMatcherException.SEALED_MATCHER);
		}
	}
}