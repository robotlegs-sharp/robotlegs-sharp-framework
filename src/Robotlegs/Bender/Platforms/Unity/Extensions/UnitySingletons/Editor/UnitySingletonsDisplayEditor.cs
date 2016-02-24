//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Robotlegs.Bender.Platforms.Unity.Extensions.MonoScriptCache;
using swiftsuspenders.mapping;
using UnityEditor;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.UnitySingletons.Impl
{
	[CustomEditor(typeof(UnitySingletonsDisplay))]
	public class UnitySingletonsDisplayEditor : Editor
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private bool start;
		
		private UnitySingletonsDisplay unitySingletons;
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/
		
		private void OnEnable()
		{
			if (start)
				return;

			start = true;
			unitySingletons = target as UnitySingletonsDisplay;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public override void OnInspectorGUI ()
		{
			foreach (KeyValuePair<MappingId, object> kvp in unitySingletons.Factory.SingletonInstances) 
			{
				string label = kvp.Key.type.Name;
				if (kvp.Key.key != null)
					label += ": " + kvp.Key.key.ToString();

				MonoScript ms = MonoScriptCacher.GetMonoScript(kvp.Value.GetType());
				if (ms != null)
				{
					EditorGUILayout.ObjectField(label, MonoScriptCacher.GetMonoScript(kvp.Value.GetType()), typeof(MonoScript), false);
				}
				else
				{
					EditorGUILayout.LabelField(label, kvp.Value.GetType().Name);
				}
			}
		}
	}
}