//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using robotlegs.bender.framework.api;


namespace robotlegs.bender.extensions.matching
{
	public class InstanceOfMatcher : IMatcher
	{
		private Type _type;
		
		public InstanceOfMatcher(Type type)
		{
			_type = type;
		}
		
		public bool Matches(object item)
		{
			return item.GetType().IsAssignableFrom(_type);
		}
	}
}

