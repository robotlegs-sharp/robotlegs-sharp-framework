using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{

	public class ViewProcessorViewHandler : IViewProcessorViewHandler
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private List<IViewProcessorMapping> _mappings = new List<IViewProcessorMapping>();

//		private Dictionary<> _knownMappings = new Dictionary<>(true);
		private Dictionary<object, List<IViewProcessorMapping>> _knownMappings = new Dictionary<object, List<IViewProcessorMapping>>();

		private IViewProcessorFactory _factory;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ViewProcessorViewHandler(IViewProcessorFactory factory)
		{
			_factory = factory;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void AddMapping(IViewProcessorMapping mapping)
		{
			int index = _mappings.IndexOf(mapping);
			if (index > -1)
				return;
			_mappings.Add(mapping);
			FlushCache();
		}

		public void RemoveMapping(IViewProcessorMapping mapping)
		{
			int index = _mappings.IndexOf(mapping);
			if (index == -1)
				return;
			_mappings.RemoveAt(index);
			FlushCache();
		}

		public void ProcessItem(object item, Type type)
		{
			IViewProcessorMapping[] interestedMappings = GetInterestedMappingsFor(item, type);
			if (interestedMappings != null)
				_factory.RunProcessors(item, type, interestedMappings);
		}

		public void UnprocessItem(object item, Type type)
		{
			IViewProcessorMapping[] interestedMappings = GetInterestedMappingsFor(item, type);
			if (interestedMappings != null)
				_factory.RunUnprocessors(item, type, interestedMappings);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void FlushCache()
		{
			_knownMappings = new Dictionary<object, List<IViewProcessorMapping>> ();
		}

		private IViewProcessorMapping[] GetInterestedMappingsFor(object view, Type type)
		{
			// we haven't seen this type before
			if (!_knownMappings.ContainsKey(type))
			{
				// we've seen this type before and nobody was interested
				if (_knownMappings[type] == null)
					return null;

				_knownMappings[type] = null;
				foreach (IViewProcessorMapping mapping in _mappings)
				{
					if (mapping.Matcher.Matches(view))
					{
						if(_knownMappings[type] == null)
						{
							_knownMappings[type] = new List<IViewProcessorMapping>();
						}
						_knownMappings[type].Add(mapping);
					}
				}
				// nobody cares, let's get out of here
				if (_knownMappings[type] == null)
					return null;
			}

			// these mappings really do care
			return _knownMappings[type].ToArray();
		}
	}
}

