//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using UnityEditor;
using System.Collections.Generic;
using swiftsuspenders.mapping;
using robotlegs.bender.platforms.unity.extensions.monoscriptCache;

namespace robotlegs.bender.platforms.unity.extensions.unitySingletons.impl
{
	[CustomEditor(typeof(UnitySingletons))]
	public class UnitySingletonsEditor : Editor
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private bool start;
		
		private UnitySingletons unitySingletons;
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/
		
		private void OnEnable()
		{
			if (start)
				return;

			start = true;
			unitySingletons = target as UnitySingletons;
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

				MonoScript ms = MonoScriptCache.GetMonoScript(kvp.Value.GetType());
				if (ms != null)
				{
					EditorGUILayout.ObjectField(label, MonoScriptCache.GetMonoScript(kvp.Value.GetType()), typeof(MonoScript), false);
				}
				else
				{
					EditorGUILayout.LabelField(label, kvp.Value.GetType().Name);
				}
			}
		}
	}
}