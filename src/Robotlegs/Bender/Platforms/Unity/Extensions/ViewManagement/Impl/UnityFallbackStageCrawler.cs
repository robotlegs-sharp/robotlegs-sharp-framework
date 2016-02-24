//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.Mediation.API;
using Robotlegs.Bender.Extensions.ViewManagement.Impl;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.ViewManager.Impl
{
	public class UnityFallbackStageCrawler : StageCrawler
	{
		protected override void ScanContainer (object container)
		{
			ScanAll ();
		}
		
		private void ProcessViewsFromRoot(Transform view)
		{
			ScanAll ();
		}

		private void ScanAll()
		{
			object[] viewScripts = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
			
			foreach (MonoBehaviour viewScript in viewScripts)
			{
				if (viewScript is IView)
				{
					ProcessView (viewScript);
				}
			}
		}
	}
}