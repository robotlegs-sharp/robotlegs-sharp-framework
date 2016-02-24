//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;

namespace Robotlegs.Bender.Extensions.ViewManagement.Support
{
	public class CustomParentFinder : IParentFinder
	{
		private bool _contains;

		private object _parent;

		public CustomParentFinder(bool contains, object parent)
		{
			_contains = contains;
			_parent = parent;
		}

		public bool Contains (object parentContainer, object childContainer)
		{
			return _contains;
		}

		public object FindParent (object childView, Dictionary<object, ContainerBinding> containers)
		{
			return _parent;
		}

		public object FindParent (object childView, IEnumerable<ContainerBinding> containers)
		{
			return _parent;
		}
	}
}

