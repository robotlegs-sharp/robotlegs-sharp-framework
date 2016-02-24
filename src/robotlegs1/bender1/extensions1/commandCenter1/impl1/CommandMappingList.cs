//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	public class CommandMappingList : ICommandMappingList
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		//TODO: Make this part of the interface
		public delegate void Processor (ICommandMapping mapping);

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Dictionary<Type, ICommandMapping> _mappingsByCommand = new Dictionary<Type, ICommandMapping>();

		private List<ICommandMapping> _mappings = new List<ICommandMapping>();

		private ICommandTrigger _trigger;

		private IEnumerable<Processor> _processors; 

		private ILogging _logger;

//		private Comparison<ICommandMapping> _compareFunction;
		private IComparer<ICommandMapping> _compareFunction;

		private bool _sorted;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Create a command mapping list
		/// </summary>
		/// <param name="trigger">The trigger that owns this list</param>
		/// <param name="processors">A reference to the mapping processors for this command map</param>
		/// <param name="logger">Optional logger</param>
		public CommandMappingList (ICommandTrigger trigger, IEnumerable<Processor> processors, ILogging logger = null)
		{
			_trigger = trigger;
			_processors =  processors;
			_logger = logger;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public List<ICommandMapping> GetList ()
		{
			if (!_sorted)
				SortMappings();
			return _mappings.GetRange (0, _mappings.Count);
		}

		public ICommandMappingList WithSortFunction(IComparer<ICommandMapping> sorter)
		{
			_sorted = false;
			_compareFunction = sorter;
			return this;
		}

		public void AddMapping (ICommandMapping mapping)
		{
			_sorted = false;
			ApplyProcessors (mapping);
			ICommandMapping oldMapping = null;
			_mappingsByCommand.TryGetValue(mapping.CommandClass, out oldMapping);
			if (oldMapping != null)
				OverwriteMapping (oldMapping, mapping);
			else 
			{
				StoreMapping (mapping);
				if (_mappings.Count == 1)
					_trigger.Activate ();
			}
		}

		public void RemoveMapping (ICommandMapping mapping)
		{
			if (_mappingsByCommand.ContainsKey (mapping.CommandClass)) 
			{
				DeleteMapping (mapping);
				if (_mappings.Count == 0)
					_trigger.Deactivate ();
			}
		}

		public void RemoveMappingFor (Type commandClass)
		{
			if (_mappingsByCommand.ContainsKey (commandClass)) 
			{
				RemoveMapping (_mappingsByCommand [commandClass]);
			}
		}

		public void RemoveAllMappings ()
		{
			if (_mappings.Count > 0) 
			{
				int length = _mappings.Count;
				List<ICommandMapping> list = _mappings.GetRange (0, length);
				while (length-- > 0) 
				{
					DeleteMapping (list[length]);
				}
				_trigger.Deactivate ();
			}
		}


		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void StoreMapping(ICommandMapping mapping)
		{
			_mappingsByCommand [mapping.CommandClass] = mapping;
			_mappings.Add (mapping);
			if (_logger != null)
				_logger.Debug("{0} mapped to {1}", new object[]{_trigger, mapping});
		}

		private void DeleteMapping(ICommandMapping mapping)
		{
			_mappingsByCommand.Remove (mapping.CommandClass);
			_mappings.Remove (mapping);
			if (_logger != null)
				_logger.Debug("{0} unmapped from {1}", new object[]{_trigger, mapping});
		}

		private void OverwriteMapping(ICommandMapping oldMapping, ICommandMapping newMapping)
		{
			if (_logger != null)
				_logger.Warn("{0} already mapped to {1}\n" +
					"If you have overridden this mapping intentionally you can use 'unmap()' " +
					"prior to your replacement mapping in order to avoid seeing this message.\n",
					new object[]{_trigger, oldMapping});
			DeleteMapping (oldMapping);
			StoreMapping (newMapping);
		}

		private void SortMappings()
		{
			if (_compareFunction != null)
				_mappings.Sort (_compareFunction);
			_sorted = true;
		}

		private void ApplyProcessors(ICommandMapping mapping)
		{
			foreach (Processor processor in _processors) 
			{
				processor(mapping);
			}
		}
	}
}