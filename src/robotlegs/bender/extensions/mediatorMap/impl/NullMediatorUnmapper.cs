//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.extensions.mediatorMap.dsl;


namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class NullMediatorUnmapper : IMediatorUnmapper
	{
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void FromMediator(Type mediatorTyp)
		{
		}

		public void FromAll()
		{
		}
	}
}

