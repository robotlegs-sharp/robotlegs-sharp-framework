using System;
using robotlegs.bender.extensions.viewProcessorMap.api;
using robotlegs.bender.extensions.viewManager.api;
using System.Collections.Generic;
using robotlegs.bender.extensions.matching;
using robotlegs.bender.extensions.viewProcessorMap.dsl;

namespace robotlegs.bender.extensions.viewProcessorMap.impl
{
	public class ViewProcessorMap : IViewProcessorMap, IViewHandler
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Dictionary<string, IViewProcessorMapper> _mappers = new Dictionary<string, IViewProcessorMapper>();

		private IViewProcessorViewHandler _handler;

		private IViewProcessorUnmapper NULL_UNMAPPER = new NullViewProcessorUnmapper();

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ViewProcessorMap(IViewProcessorFactory factory, IViewProcessorViewHandler handler = null)
		{
			if (handler == null)
			{
				handler = new ViewProcessorViewHandler(factory);
			}
			_handler = handler;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IViewProcessorMapper MapMatcher(ITypeMatcher matcher)
		{
			IViewProcessorMapper mapper = _mappers [matcher.CreateTypeFilter ().Descriptor];
			if(mapper == null)
			{
				mapper = _mappers[matcher.CreateTypeFilter().Descriptor] = CreateMapper(matcher);
			}
			return mapper;
		}
			
		public IViewProcessorMapper Map(Type type)
		{
			ITypeMatcher matcher = new TypeMatcher().AllOf(type);
			return MapMatcher(matcher);
		}

		public IViewProcessorUnmapper UnmapMatcher(ITypeMatcher matcher)
		{
			IViewProcessorMapper mapper = _mappers[matcher.CreateTypeFilter().Descriptor];
			if (mapper == null)
				return NULL_UNMAPPER;
			return mapper as IViewProcessorUnmapper;
		}

		public IViewProcessorUnmapper Unmap(Type type)
		{
			ITypeMatcher matcher = new TypeMatcher().AllOf(type);
			return UnmapMatcher(matcher);
		}

		public void Process(object item)
		{
			_handler.ProcessItem(item, item.GetType());
		}

		public void Unprocess(object item)
		{
			_handler.UnprocessItem(item, item.GetType());
		}

		public void HandleView(object view, Type type)
		{
			_handler.ProcessItem(view, type);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private IViewProcessorMapper CreateMapper(ITypeMatcher matcher)
		{
			return new ViewProcessorMapper(matcher.CreateTypeFilter(), _handler);
		}
	}
}