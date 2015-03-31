//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.extensions.contextview.api;
using UnityEngine;


namespace robotlegs.bender.platforms.unity.extensions.contextview.impl
{
	public class TransformContextView : IContextView
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public object view
		{
			get 
			{
				return _transform;
			}
		}
		
		public Transform transform
		{
			get
			{
				return _transform;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private Transform _transform;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public TransformContextView (Transform view)
		{
			_transform = view;
		}
	}
}

