//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System.Collections.Generic;
using System;

namespace robotlegs.bender.extensions.commandCenter.api
{
	public interface ICommandMappingList
	{

		/**
		 * Optional mapping sort function
		 * @param sorter Sort function
		 * @return Self
		 */
		ICommandMappingList WithSortFunction(IComparer<ICommandMapping> sorter);
		
		/**
		 * Sorted list of active mappings
		 * @return List of mappings
		 */
		List<ICommandMapping> GetList();
		
		/**
		 * Adds a mapping to the mapping list
		 * @param mapping Command mapping
		 */

		void AddMapping(ICommandMapping mapping);
		
		/**
		 * Removes a mapping from the mapping list
		 * @param mapping Command mapping
		 */
		void RemoveMapping(ICommandMapping mapping);
		
		/**
		 * Removes a mapping from the mapping list using the Command class
		 * @param commandClass The command class to remove the mapping for
		 */
		void RemoveMappingFor(Type commandClass);
		
		/**
		 * Removes all mappings for this command mapping list
		 */
		void RemoveAllMappings();
	}
}