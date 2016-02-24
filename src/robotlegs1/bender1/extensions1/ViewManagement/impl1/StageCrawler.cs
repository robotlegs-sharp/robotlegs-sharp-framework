//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement.Impl
{
	public abstract class StageCrawler : IStageCrawler
	{
		private ContainerBinding _binding;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/
		public ContainerBinding Binding
		{
			set
			{
				_binding = value;
			}
		}

		public void Scan(object view)
		{
			ScanContainer(view);
		}

		/*============================================================================*/
		/* Protected Functions                                                        */
		/*============================================================================*/

		protected abstract void ScanContainer(object container);

		protected void ProcessView(object view)
		{
			_binding.HandleView(view, view.GetType());
		}
	}
}

