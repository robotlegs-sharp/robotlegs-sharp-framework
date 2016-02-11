//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;

namespace robotlegs.bender.framework.impl
{
	public class Context : IContext
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<Exception> ERROR
		{
			add
			{
				_lifecycle.ERROR += value;
			}
			remove 
			{
				_lifecycle.ERROR -= value;
			}
		}

		public event Action STATE_CHANGE 
		{
			add
			{
				_lifecycle.STATE_CHANGE += value;
			}
			remove
			{
				_lifecycle.STATE_CHANGE -= value;
			}
		}

		public event Action<object> PRE_INITIALIZE 
		{
			add
			{
				_lifecycle.PRE_INITIALIZE += value;
			}
			remove 
			{
				_lifecycle.PRE_INITIALIZE -= value;
			}
		}

		public event Action<object> INITIALIZE
		{
			add
			{
				_lifecycle.INITIALIZE += value;
			}
			remove 
			{
				_lifecycle.INITIALIZE -= value;
			}
		}

		public event Action<object> POST_INITIALIZE
		{
			add	
			{
				_lifecycle.POST_INITIALIZE += value;
			}
			remove 
			{
				_lifecycle.POST_INITIALIZE -= value;
			}
		}

		public event Action<object> PRE_SUSPEND
		{
			add	
			{
				_lifecycle.PRE_SUSPEND += value;
			}
			remove 
			{
				_lifecycle.PRE_SUSPEND -= value;
			}
		}

		public event Action<object> SUSPEND
		{
			add	
			{
				_lifecycle.SUSPEND += value;
			}
			remove 
			{
				_lifecycle.SUSPEND -= value;
			}
		}

		public event Action<object> POST_SUSPEND
		{
			add	
			{
				_lifecycle.POST_SUSPEND += value;
			}
			remove 
			{
				_lifecycle.POST_SUSPEND -= value;
			}
		}

		public event Action<object> PRE_RESUME
		{
			add	
			{
				_lifecycle.PRE_RESUME += value;
			}
			remove 
			{
				_lifecycle.PRE_RESUME -= value;
			}
		}

		public event Action<object> RESUME
		{
			add	
			{
				_lifecycle.RESUME += value;
			}
			remove 
			{
				_lifecycle.RESUME -= value;
			}
		}

		public event Action<object> POST_RESUME
		{
			add	
			{
				_lifecycle.POST_RESUME += value;
			}
			remove 
			{
				_lifecycle.POST_RESUME -= value;
			}
		}

		public event Action<object> PRE_DESTROY
		{
			add	
			{
				_lifecycle.PRE_DESTROY += value;
			}
			remove 
			{
				_lifecycle.PRE_DESTROY -= value;
			}
		}

		public event Action<object> DESTROY
		{
			add	
			{
				_lifecycle.DESTROY += value;
			}
			remove 
			{
				_lifecycle.DESTROY -= value;
			}
		}

		public event Action<object> POST_DESTROY
		{
			add	
			{
				_lifecycle.POST_DESTROY += value;
			}
			remove 
			{
				_lifecycle.POST_DESTROY -= value;
			}
		}

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

		public LifecycleState state
		{
			get { return _lifecycle.state; }
		}

		public bool Uninitialized
		{
			get { return _lifecycle.Uninitialized; }
		}

		public bool Initialized
		{
			get { return _lifecycle.Initialized; }
		}

		public bool Active
		{
			get { return _lifecycle.Active; }
		}

		public bool Suspended
		{
			get { return _lifecycle.Suspended; }
		}

		public bool Destroyed
		{
			get { return _lifecycle.Destroyed; }
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector = new RobotlegsInjector();

		private LogManager _logManager = new LogManager();

		private List<IContext> _children = new List<IContext>();

		private Pin _pin;

		private Lifecycle _lifecycle;

		private ConfigManager _configManager;

		private ExtensionInstaller _extensionInstaller;

		private ILogging _logger;

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

		public void Initialize(Action callback = null)
		{
			_lifecycle.Initialize (null);
		}

		public void Suspend(Action callback = null)
		{
			_lifecycle.Suspend (null);
		}

		public void Resume(Action callback = null)
		{
			_lifecycle.Resume (null);
		}

		public void Destroy(Action callback = null)
		{
			_lifecycle.Destroy (null);
		}

		public IContext BeforeInitializing(Action handler)
		{
			_lifecycle.BeforeInitializing (handler);
			return this;
		}

		public IContext BeforeInitializing (HandlerMessageDelegate handler)
		{
			_lifecycle.BeforeInitializing (handler);
			return this;
		}

		public IContext BeforeInitializing (HandlerMessageCallbackDelegate handler)
		{
			_lifecycle.BeforeInitializing (handler);
			return this;
		}

		public IContext WhenInitializing(Action handler)
		{
			_lifecycle.WhenInitializing (handler);
			return this;
		}

		public IContext AfterInitializing(Action handler)
		{
			_lifecycle.AfterInitializing (handler);
			return this;
		}

		public IContext BeforeSuspending(Action handler)
		{
			_lifecycle.BeforeSuspending (handler);
			return this;
		}

		public IContext BeforeSuspending (HandlerMessageDelegate handler)
		{
			_lifecycle.BeforeSuspending (handler);
			return this;
		}

		public IContext BeforeSuspending (HandlerMessageCallbackDelegate handler)
		{
			_lifecycle.BeforeSuspending (handler);
			return this;
		}

		public IContext WhenSuspending(Action handler)
		{
			_lifecycle.WhenSuspending (handler);
			return this;
		}

		public IContext AfterSuspending(Action handler)
		{
			_lifecycle.AfterSuspending (handler);
			return this;
		}

		public IContext BeforeResuming(Action handler)
		{
			_lifecycle.BeforeResuming (handler);
			return this;
		}

		public IContext BeforeResuming (HandlerMessageDelegate handler)
		{
			_lifecycle.BeforeResuming (handler);
			return this;
		}

		public IContext BeforeResuming (HandlerMessageCallbackDelegate handler)
		{
			_lifecycle.BeforeResuming (handler);
			return this;
		}

		public IContext WhenResuming(Action handler)
		{
			_lifecycle.WhenResuming(handler);
			return this;
		}

		public IContext AfterResuming(Action handler)
		{
			_lifecycle.AfterResuming (handler);
			return this;
		}

		public IContext BeforeDestroying(Action handler)
		{
			_lifecycle.BeforeDestroying (handler);
			return this;
		}

		public IContext BeforeDestroying (HandlerMessageDelegate handler)
		{
			_lifecycle.BeforeDestroying (handler);
			return this;
		}

		public IContext BeforeDestroying (HandlerMessageCallbackDelegate handler)
		{
			_lifecycle.BeforeDestroying (handler);
			return this;
		}

		public IContext WhenDestroying(Action callback)
		{
			_lifecycle.WhenDestroying(callback);
			return this;
		}

		public IContext AfterDestroying(Action callback)
		{
			_lifecycle.AfterDestroying (callback);
			return this;
		}

		public IContext Install<T>() where T : IExtension
		{
			_extensionInstaller.Install<T>();
			return this;
		}

		public IContext Install(Type type)
		{
			_extensionInstaller.Install(type);
			return this;
		}

		public IContext Install(IExtension extension)
		{
			_extensionInstaller.Install(extension);
			return this;
		}

		public IContext Configure<T>() where T : class
		{
			_configManager.AddConfig<T>();
			return this;
		}

		public IContext Configure(params IConfig[] configs)
		{
			foreach (IConfig config in configs)
				_configManager.AddConfig(config);
			return this;
		}

		public IContext Configure(params object[] objects)
		{
			foreach (Object obj in objects)
				_configManager.AddConfig(obj);
			return this;
		}
		
		public IContext AddChild(IContext child)
		{
			if (!_children.Contains (child)) 
			{
				_logger.Info("Adding child context {0}", new object[]{child});
				if (!child.Uninitialized)
				{
					_logger.Warn("Child context {0} must be uninitialized", new object[]{child});
				}
				if (child.injector.parent != null)
				{
					_logger.Warn("Child context {0} must not have a parent Injector", new object[]{child});
				}
				_children.Add(child);
				child.injector.parent = injector;
				child.POST_DESTROY += OnChildDestroy;

			}
			return this;
		}

		public IContext RemoveChild(IContext child)
		{
			if (_children.Contains(child))
			{
				_logger.Info("Removing child context {0}", new object[]{child});
				_children.Remove(child);
				child.injector.parent = null;
				child.POST_DESTROY -= OnChildDestroy;

			}
			else
			{
				_logger.Warn("Child context {0} must be a child of {1}", new object[]{child, this});
			}
			return this;
		}

		// Handle this process match from the config
		public IContext AddConfigHandler(IMatcher matcher, Action<object> handler)
		{
			_configManager.AddConfigHandler (matcher, handler);
			return this;
		}
		
		public ILogging GetLogger(object source)
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
			_lifecycle = new Lifecycle(this);
			_configManager = new ConfigManager (this);
			_extensionInstaller = new ExtensionInstaller (this);
			BeforeInitializing(BeforeInitializingCallback);
			AfterInitializing(AfterInitializingCallback);
			BeforeDestroying(BeforeDestroyingCallback);
			AfterDestroying(AfterDestroyingCallback);
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
			_extensionInstaller.Destroy();
			_configManager.Destroy();
			_pin.ReleaseAll();
			_injector.Teardown ();
			RemoveChildren();
			_logger.Info("Destroy Complete");
			_logManager.RemoveAllTargets();
		}

		private void OnChildDestroy(object context)
		{
			RemoveChild (context as IContext);
		}

		private void RemoveChildren()
		{
			foreach (IContext child in _children.ToArray()) 
			{
				RemoveChild (child);
			}
			_children.Clear ();
		}
	}
}

