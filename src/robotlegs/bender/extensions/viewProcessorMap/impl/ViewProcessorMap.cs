//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
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

		[Inject(true)] //TODO: When propertyInfo.IsOptional tag works for the injector in IL2CPP remove optional inject tag 
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
			string descriptor = matcher.CreateTypeFilter().Descriptor;
			IViewProcessorMapper mapper;
			if (_mappers.ContainsKey (descriptor)) {
				mapper = _mappers [descriptor];
			}
			else
			{
				mapper = _mappers[descriptor] = CreateMapper(matcher);
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
			IViewProcessorMapper mapper;
			if(!_mappers.TryGetValue(matcher.CreateTypeFilter().Descriptor, out mapper))
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