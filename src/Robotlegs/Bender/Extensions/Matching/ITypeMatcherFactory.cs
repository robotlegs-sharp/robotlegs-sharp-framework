//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

namespace Robotlegs.Bender.Extensions.Matching
{
	public interface ITypeMatcherFactory : ITypeMatcher
	{
		TypeMatcher Clone(); 
	}
}

