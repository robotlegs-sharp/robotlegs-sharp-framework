//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Platforms.Unity.Extensions.MonoScriptCache;
using UnityEditor;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.Mediation.Impl
{
	[CustomEditor(typeof(MediatorAttach))]
	public class MediatorAttachEditor : Editor
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private bool start;

		private MediatorAttach mediatorAttach;

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void OnEnable()
		{
			if (start)
				return;

			start = true;

			mediatorAttach = target as MediatorAttach;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField ("View", mediatorAttach.View.GetType().Name);
			foreach (object mediator in mediatorAttach.Mediators) 
			{
				MonoScript ms = MonoScriptCacher.GetMonoScript(mediator.GetType());
				if (ms != null)
				{
					EditorGUILayout.ObjectField("Mediator", ms, typeof(MonoScript), false);
				}
				else
				{
					EditorGUILayout.LabelField ("Mediator", mediator.GetType().Name);
				}
			}
		}
	}
}

