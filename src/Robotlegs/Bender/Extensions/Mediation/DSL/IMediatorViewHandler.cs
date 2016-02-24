//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.Mediation.DSL
{
	public interface IMediatorViewHandler : IViewHandler
	{
		void AddMapping (IMediatorMapping mapping);

		void RemoveMapping (IMediatorMapping mapping);
	}
}

