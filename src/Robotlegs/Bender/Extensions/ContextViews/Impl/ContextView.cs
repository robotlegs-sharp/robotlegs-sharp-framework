//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextViews.API;

namespace Robotlegs.Bender.Extensions.ContextViews.Impl
{

	public class ContextView : IContextView
	{
		private object _view;
				
		public ContextView (object view)
		{
			_view = view;
		}

		public object view
		{
			get
			{
				return _view;
			}
		}
	}
}

