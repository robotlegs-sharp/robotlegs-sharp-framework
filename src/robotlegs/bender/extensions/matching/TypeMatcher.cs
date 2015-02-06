using System.Collections;
using System.Collections.Generic;
using System;

namespace robotlegs.bender.extensions.matching
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
			PushAddedTypesTo(allOf, allOfTypes);
			return this;
		}
		
		public TypeMatcher AnyOf(params Type[] anyOf)
		{
			PushAddedTypesTo(anyOf, anyOfTypes);
			return this;
		}
		
		public TypeMatcher NoneOf(params Type[] noneOf)
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
			return new TypeMatcher().AllOf(allOfTypes.ToArray()).AnyOf(anyOfTypes.ToArray()).NoneOf(noneOfTypes.ToArray());
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

		protected void PushAddedTypesTo(Type[] types, List<Type> targetSet)
		{
			if (typeFilter != null)
				ThrowSealedMatcherError();

			PushValuesToTypeList(types, targetSet);
		}

		protected void ThrowSealedMatcherError()
		{
			throw new TypeMatcherException(TypeMatcherException.SEALED_MATCHER);
		}

		protected void PushValuesToTypeList(Type[] types, List<Type> targetSet)
		{
			foreach (Type type in types)
				targetSet.Add(type);
		}
	}
}