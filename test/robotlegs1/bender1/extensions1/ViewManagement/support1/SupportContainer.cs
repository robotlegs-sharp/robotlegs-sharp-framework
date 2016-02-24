//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;

namespace Robotlegs.Bender.Extensions.ViewManagement.Support
{
	public class SupportContainer
	{
		protected int _childCount;

		public int NumChildren
		{
			get
			{
				return _childCount;
			}
		}

		public virtual SupportContainer Parent
		{
			get
			{
				return _parent;
			}
			protected set
			{
				_parent = value;
			}
		}

		protected SupportContainer _parent;

		public virtual void AddChild(SupportContainer child)
		{
			_childCount++;
			child.Parent = this;
		}

		public virtual void RemoveChild(SupportContainer child)
		{
			if (child.Parent == this)
			{
				child.Parent = null;
			}
		}
	}
}

