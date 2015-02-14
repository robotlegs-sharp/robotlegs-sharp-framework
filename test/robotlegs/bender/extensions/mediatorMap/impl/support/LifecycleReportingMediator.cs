using System;
using robotlegs.bender.extensions.mediatorMap.api;

namespace robotlegs.bender.extensions.mediatorMap.impl.support
{

	public class LifecycleReportingMediator : IMediator
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject(true, "preInitializeCallback")]
		public Action<string> preInitializeCallback;

		[Inject(true, "initializeCallback")]
		public Action<string> initializeCallback;

		[Inject(true, "postInitializeCallback")]
		public Action<string> postInitializeCallback;

		[Inject(true, "preDestroyCallback")]
		public Action<string> preDestroyCallback;

		[Inject(true, "destroyCallback")]
		public Action<string> destroyCallback;

		[Inject(true, "postDestroyCallback")]
		public Action<string> postDestroyCallback;

		public bool initialized;

		public bool destroyed;

		public object view;

		public object viewComponent
		{
			set
			{
				this.view = value;
			}
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void PreInitialize()
		{
			if(preInitializeCallback != null)
			{
				preInitializeCallback("preInitialize");
			}
		}

		public void Initialize()
		{
			initialized = true;
			if(initializeCallback != null)
			{
				initializeCallback("initialize");
			}
		}

		public void PostInitialize()
		{
			if(postInitializeCallback  != null)
			{
				postInitializeCallback("postInitialize");
			}
		}

		public void PreDestroy()
		{
			if(preDestroyCallback != null)
			{
				preDestroyCallback("preDestroy");
			}
		}

		public void Destroy()
		{
			destroyed = true;

			if(destroyCallback != null)
			{
				destroyCallback("destroy");
			}
		}

		public void PostDestroy()
		{
			if(postDestroyCallback != null)
			{
				postDestroyCallback("postDestroy");
			}
		}
	}
}
