using System.Collections.Generic;
using System;
using strange.context.api;

namespace robotlegs.bender.extensions.matching
{
	public interface ITypeFilter : IMatcher
	{
		List<Type> AllOfTypes {get;}
		List<Type> AnyOfTypes {get;}
		List<Type> NoneOfTypes {get;}
		string Descriptor {get;}
	}
}