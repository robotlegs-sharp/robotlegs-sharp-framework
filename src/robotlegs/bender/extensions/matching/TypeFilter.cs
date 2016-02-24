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
	public class TypeFilter : ITypeFilter //TODO: Consider renaming this due to conflicts with System.Reflection.TypeFilter
	{
		protected List<Type> allOfTypes;

		public List<Type> AllOfTypes
		{
			get
			{
				return allOfTypes; 
			}
		}
		
		protected List<Type> anyOfTypes;

		public List<Type> AnyOfTypes
		{
			get 
			{
				return anyOfTypes; 
			}
		}
		
		protected List<Type> noneOfTypes;

		public List<Type> NoneOfTypes
		{
			get
			{
				return noneOfTypes;
			}
		}
		
		protected string descriptor;

		public string Descriptor
		{
			get
			{
				return descriptor == null ? descriptor = CreateDescriptor() : descriptor;
			}
		}

		public TypeFilter(IEnumerable<Type> allOf, IEnumerable<Type> anyOf, IEnumerable<Type> noneOf)
		{
			if (allOf == null || anyOf == null || noneOf == null)
				throw new ArgumentNullException("TypeFilter parameters can not be null");

			allOfTypes = new List<Type>(allOf);
			anyOfTypes = new List<Type>(anyOf);
			noneOfTypes = new List<Type>(noneOf);
		}
		
		public bool Matches (object item)
		{
			Type itemType = item.GetType();

			int i = allOfTypes.Count;
			while (i-- != 0)
			{
				if (!allOfTypes[i].IsAssignableFrom(itemType))
					return false;
			}
			
			i = noneOfTypes.Count;
			while (i-- != 0)
			{
				if (noneOfTypes[i].IsAssignableFrom(itemType))
					return false;
			}

			if (anyOfTypes.Count == 0 && (allOfTypes.Count > 0 || noneOfTypes.Count > 0))
				return true;
			
			i = anyOfTypes.Count;
			while (i-- != 0)
			{
				if (anyOfTypes[i].IsAssignableFrom(itemType))
					return true;
			}

			return false;
		}

		public string GetClassName(Type type)
		{
			return type.FullName;
//			return type.AssemblyQualifiedName; // Removed this, as it's more readable above
		}

		public List<string> AlphabetiseCaseInsensitiveClassNames(IEnumerable<Type> types)
		{
			List<string> allClassNames = new List<string>();

			foreach (Type type in types)
			{
				allClassNames.Add (GetClassName (type));
			}

			allClassNames.Sort(string.Compare);

			return allClassNames;
		}

		protected string CreateDescriptor()
		{
			List<string> allOfClassNames = AlphabetiseCaseInsensitiveClassNames(allOfTypes);
			List<string> anyOfClassNames = AlphabetiseCaseInsensitiveClassNames(anyOfTypes);
			List<string> noneOfClassNames = AlphabetiseCaseInsensitiveClassNames(noneOfTypes);
			return "all of: " + string.Join(", ", allOfClassNames.ToArray())
				+ ", any of: " + string.Join(", ", anyOfClassNames.ToArray())
				+ ", none of: " + string.Join(", ", noneOfClassNames.ToArray());
		}
	}
}

