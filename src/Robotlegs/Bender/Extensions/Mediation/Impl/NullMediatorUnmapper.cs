//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Extensions.Mediation.DSL;

namespace Robotlegs.Bender.Extensions.Mediation.Impl
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

