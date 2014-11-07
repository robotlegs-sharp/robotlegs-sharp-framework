using System;
using System.Collections.Generic;

namespace robotlegs.bender.extensions.commandCenter.api
{
	public interface ICommandMapping
	{/**
		 * The concrete Command Class for this mapping
		 */
		Type CommandClass {get;}
		
		/**
		 * The "execute" method to invoke on the Command instance
		 */

		string ExecuteMethod {get;}
		
		/**
		 * A list of Guards to query before execution
		 */
		List<object> Guards {get;}
		
		/**
		 * A list of Hooks to run during execution
		 */
		List<object> Hooks {get;}
		
		/**
		 * Unmaps a Command after a successful execution
		 */
		bool FireOnce {get;}
		
		/**
		 * Supply the payload values via instance injection
		 */
		bool PayloadInjectionEnabled {get;}
		
		/**
		 * The "execute" method to invoke on the Command instance
		 */
		ICommandMapping SetExecuteMethod(string name);
		
		/**
		 * A list of Guards to query before execution
		 */
		ICommandMapping AddGuards(params object[] guards);
		
		/**
		 * A list of Hooks to run during execution
		 */
		ICommandMapping AddHooks(params object[] hooks);
		
		/**
		 * Unmaps a Command after a successful execution
		 */
		ICommandMapping SetFireOnce(bool value);
		
		/**
		 * Supply the payload values via instance injection
		 */
		ICommandMapping SetPayloadInjectionEnabled(bool value);
	}
}