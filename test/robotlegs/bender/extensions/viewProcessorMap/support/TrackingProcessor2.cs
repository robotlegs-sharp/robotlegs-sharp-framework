//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System;
using robotlegs.bender.framework.api;


namespace robotlegs.bender.extensions.viewProcessorMap.support
{
	public class TrackingProcessor2 : ITrackingProcessor
	{
		/*============================================================================*/
		/* Private Properties                                                          */
		/*============================================================================*/

		private List<object> _processedViews = new List<object>();

		private List<object> _unprocessedViews = new List<object>();

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public List<object> ProcessedViews
		{
			get
			{
				return _processedViews;
			}
		}


		public List<object> UnprocessedViews
		{
			get
			{
				return _unprocessedViews;
			}
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Process(object view, Type type, IInjector injector)
		{
			_processedViews.Add(view);
		}

		public void Unprocess(object view, Type type, IInjector injector)
		{
			_unprocessedViews.Add(view);
		}
	}
}