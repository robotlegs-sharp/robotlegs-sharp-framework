using System;
using robotlegs.bender.framework.impl;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class Context : IContext, IPinEvent
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> Detained {
			add 
			{
				_pin.Detained += value;
			}
			remove 
			{
				_pin.Detained -= value;
			}
		}

		public event Action<object> Released
		{
			add
			{
				_pin.Released += value;
			}
			remove 
			{
				_pin.Released -= value;
			}
		}

		public IInjector injector
		{
			get { return _injector; }
		}

		public LogLevel LogLevel
		{
			get
			{
				return _logManager.logLevel;
			}
			set
			{
				_logManager.logLevel = value;
			}
		}

		public bool initialized
		{
			get { return _initialized; }
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector = new RobotlegsInjector();

		private LogManager _logManager = new LogManager();

		private bool _initialized = false;

		private ConfigManager _configManager;

		private ExtensionInstaller _extensionInstaller;
		
		private ContextStateCallback _preInitilizeCallback;
		private ContextStateCallback _postInitializeCallback;
		private ContextStateCallback _preDestroyCallback;
		private ContextStateCallback _postDestroyCallback;

		private Pin _pin;

		private ILogger _logger;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Creates a new Context
		/// </summary>
		public Context ()
		{
			Setup();
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IContext Initialize()
		{
			PreInitialized();

			_configManager.Initialize ();
			_initialized = true;
//			UnityEngine.Debug.Log ("Initalize");
			
			PostInitialized ();
			return this;
		}

		public IContext Install<T>() where T : IExtension
		{
			_extensionInstaller.Install<T>();
			return this;
		}

		public IContext Configure<T>() where T : class
		{
			_configManager.AddConfig<T>();
			return this;
		}

		public IContext Configure(params object[] objects)
		{
			foreach (Object obj in objects)
				_configManager.AddConfig(obj);
			return this;
		}

		// The states the context goes through in order

		// New Context uninitialized
		// User installs and runs Extensions
		// User adds configs

		// Context gets initialized either by user or a config
		// Context fires pre-initilized callbacks
		// Context processes configs
		// Initialized flag set
		// Context fires post-initilized callbacks
		
		// Context gets destroyed either by user or a config
		// Pre Destroyed
		// Destroyed
		
		public IContext AddChild(IContext child)
		{
			throw new NotImplementedException();
		}
		
		public IContext RemoveChild(IContext child)
		{
			throw new NotImplementedException();
		}
		
		// Handle this process match from the config
		public IContext AddConfigHandler(IMatcher matcher, Action<object> handler)
		{
			_configManager.AddConfigHandler (matcher, handler);
			return this;
		}
		
		public ILogger GetLogger(object source)
		{
			return _logManager.GetLogger(source);
		}

		public IContext AddLogTarget(ILogTarget target)
		{
			_logManager.AddLogTarget(target);
			return this;
		}
		
		public IContext Detain(params object[] instances)
		{
			foreach (object instance in instances)
			{
				_pin.Detain(instance);
			}
			return this;
		}
		
		public IContext Release(params object[] instances)
		{
			foreach (object instance in instances)
			{
				_pin.Release(instance);
			}
			return this;
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		/// <summary>
		/// Configures mandatory context dependencies
		/// </summary>
		private void Setup()
		{
			_injector.Map (typeof(IInjector)).ToValue (_injector);
			_injector.Map (typeof(IContext)).ToValue (this);

			_logger = _logManager.GetLogger(this);
			_pin = new Pin();

			_extensionInstaller = new ExtensionInstaller (this);
			_configManager = new ConfigManager (this);

			_preInitilizeCallback = new ContextStateCallback ();
			_postInitializeCallback = new ContextStateCallback ();
			_preDestroyCallback = new ContextStateCallback ();
			_postDestroyCallback = new ContextStateCallback ();

			_preInitilizeCallback.AddCallback(BeforeInitializingCallback);
			_postInitializeCallback.AddCallback(AfterInitializingCallback);
			_preDestroyCallback.AddCallback(BeforeDestroyingCallback);
			_postDestroyCallback.AddCallback(AfterDestroyingCallback);
		}

		private void PreInitialized()
		{
			_preInitilizeCallback.ProcessCallbacks ();
		}

		private void PostInitialized()
		{
			_postInitializeCallback.ProcessCallbacks ();
		}
		
		public IContext AddPreInitializedCallback(ContextStateCallback.CallbackDelegate callback)
		{
			_preInitilizeCallback.AddCallback (callback);
			return this;
		}
		
		public IContext AddPostInitializedCallback(ContextStateCallback.CallbackDelegate callback)
		{
			_postInitializeCallback.AddCallback (callback);
			return this;
		}
		
		public IContext AddPreDestroyCallback(ContextStateCallback.CallbackDelegate callback)
		{
			_preDestroyCallback.AddCallback (callback);
			return this;
		}
		
		public IContext AddPostDestroyCallback(ContextStateCallback.CallbackDelegate callback)
		{
			_postDestroyCallback.AddCallback (callback);
			return this;
		}
		
		private void BeforeInitializingCallback()
		{
			_logger.Info("Initializing...");
		}
		
		private void AfterInitializingCallback()
		{
			_logger.Info("Initialize complete");
		}
		
		private void BeforeDestroyingCallback()
		{
			_logger.Info("Destroying...");
		}
		
		private void AfterDestroyingCallback()
		{
//			_extensionInstaller.Destroy();
			_configManager.Destroy();
			_pin.ReleaseAll();
//			_injectionBinder.TearDown();
//			RemoveChildren();
			_logger.Info("Destroy Complete");
			_logManager.RemoveAllTargets();
		}
	}
}

