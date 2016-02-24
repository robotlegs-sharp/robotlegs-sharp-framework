//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement.Support
{
	public class SupportView : SupportContainer, IView, IViewStateWatcher
	{
		public bool isAdded { get { return isAddedToStage; } }

		public event Action<object> added;
		public event Action<object> removed;
		public event Action<object> disabled;
		public event Action<object> enabled;
		public event Action<IView> RemoveView;
		public bool isAddedToStage;

		private bool registered = false;

		public void AddThisView()
		{
			Register ();
			isAddedToStage = true;
			if (added != null)
			{
				added(this);
			}
		}

		public void RemoveChild (SupportView child)
		{
			if (child.Parent == this)
			{
				child.RemoveThisView();
			}
		}

		public void RemoveThisView()
		{
			isAddedToStage = false;
			Parent = null;
			if (RemoveView != null)
			{
				RemoveView (this);
			}
			if (removed != null)
			{
				removed(this);
			}
		}

		public override void AddChild(SupportContainer child)
		{
			base.AddChild (child);
			if (child is SupportView) (child as SupportView).AddThisView();
		}

		public void Enable()
		{
			if (enabled != null)
			{
				enabled (this);
			}
		}

		public void Disable()
		{
			if (disabled != null)
			{
				disabled (this);
			}
		}

		public void Register()
		{
			if (registered)
				return;

			registered = true;
			ViewNotifier.RegisterView (this);
		}
			
		public override SupportContainer Parent
		{
			get
			{
				return base.Parent;
			}
			protected set
			{
				base.Parent = value;
				if (value != null)
				{
					Register ();
				}
			}
		}
	}
}

