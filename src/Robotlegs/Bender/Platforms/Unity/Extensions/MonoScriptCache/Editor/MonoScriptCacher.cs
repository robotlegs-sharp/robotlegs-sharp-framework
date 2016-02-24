//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Robotlegs.Bender.Platforms.Unity.Extensions.MonoScriptCache
{
	public static class MonoScriptCacher
	{
		/*============================================================================*/
		/* Private Static Properties                                                  */
		/*============================================================================*/

		private static Dictionary<Type, MonoScript> monoScriptsCache;
		
		/*============================================================================*/
		/* Public Static Functions                                                    */
		/*============================================================================*/

		public static MonoScript GetMonoScript(Type type)
		{
			if (monoScriptsCache == null) 
			{
				GetMonoscriptCache ();
			}

			MonoScript monoScript;
			monoScriptsCache.TryGetValue (type, out monoScript);
			return monoScript;
		}
		
		/*============================================================================*/
		/* Private Static Functions                                                   */
		/*============================================================================*/

		private static void GetMonoscriptCache()
		{
			monoScriptsCache = new Dictionary<Type, MonoScript> ();

			MonoScript[] monoScripts = Resources.FindObjectsOfTypeAll<MonoScript>();
			foreach (MonoScript monoScript in monoScripts) 
			{
				Type monoScriptType = monoScript.GetClass();
				if (monoScriptType == null)
					continue;

				monoScriptsCache[monoScriptType] = monoScript;
			}
		}
	}
}

