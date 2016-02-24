//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ContextView.API;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.ContextView.Impl
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

