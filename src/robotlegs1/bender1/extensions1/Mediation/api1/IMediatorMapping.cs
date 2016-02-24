//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.Matching;

namespace Robotlegs.Bender.Extensions.Mediation.API
{
	public interface IMediatorMapping
	{
		/// <summary>
		/// The matcher for this mapping
		/// </summary>
		/// <value>The matcher for this mapping</value>
		ITypeFilter Matcher {get;}

		/// <summary>
		/// The concrete mediator class
		/// </summary>
		/// <value>The concrete mediator class</value>
		Type MediatorType {get;}

		/// <summary>
		/// A list of guards to check before allowing mediator creation
		/// </summary>
		/// <value>A list of guards to check before allowing mediator creation</value>
		List<object> Guards {get;}

		/// <summary>
		/// A list of hooks to run before creating a mediator
		/// </summary>
		/// <value>A list of hooks to run before creating a mediator</value>
		List<object> Hooks {get;}

		/// <summary>
		/// Should the mediator be removed when the mediated item looses scope?
		/// </summary>
		/// <value>Should the mediator be removed when the mediated item looses scope?</value>
		bool AutoRemoveEnabled {get;}
	}
}

