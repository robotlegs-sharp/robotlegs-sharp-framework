using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;
using System.Reflection;

namespace robotlegs.bender.framework.impl
{
	public class ConfigManager
	{
		private static readonly Type[] NO_ARGS = new Type [0];

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private ObjectProcessor _objectProcessor = new ObjectProcessor();

		private List<object> _configs = new List<object>();

		private List<object> _queue = new List<object>();

		private IInjector _injector;

		private ILogger _logger;

		private bool _initialized = false;
		
		public IContext _context;
		
		public delegate void ProcessMatch(object obj);

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ConfigManager (IContext context)
		{
			_context = context;
			_injector = _context.injector;
			_logger = context.GetLogger(this);
			AddConfigHandler (new TypeMatcher (), HandleType);
			AddConfigHandler (new ObjectMatcher (), HandleObject);
			context.AfterInitializing (Initialize); //TODO: This should be at the end of when not after
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void AddConfig<T>() where T : class
		{
			AddConfig (typeof(T));
		}

		public void AddConfig(object config)
		{
			if (!_configs.Contains(config))
			{
				_configs.Add(config);
				_objectProcessor.ProcessObject(config);
			}
		}
		
		public void AddConfigHandler(IMatcher matcher, Action<object> process)
		{
			_objectProcessor.AddObjectHandler(matcher, process);
		}

		public void Destroy()
		{
			_objectProcessor.RemoveAllHandlers();
			_configs.Clear();
		}
		
		private void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;
				ProcessQueue();
			}
		}
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void HandleType(object obj)
		{
			if (_initialized)
			{
				_logger.Debug("Already initialized. Instantiating config type {0}", obj);
				ProcessType(obj as Type);
			}
			else
			{
				_logger.Debug("Not yet initialized. Queuing config class {0}", obj);
				_queue.Add(obj);
			}
		}

		private void HandleObject(object obj)
		{
			if (_initialized)
			{
				_logger.Debug("Already initialized. Injecting into config object {0}", obj);
				ProcessObject(obj);
			}
			else
			{
				_logger.Debug("Not yet initialized. Queuing config object {0}", obj);
				_queue.Add(obj);
			}
		}
		
		private void ProcessQueue()
		{
			foreach (object config in _queue)
			{
				if (config is Type)
				{
					_logger.Debug("Now initializing. Instantiating config class {0}", config);
					ProcessType (config as Type);
				}
				else
				{
					_logger.Debug("Now initializing. Injecting into config object {0}", config);
					ProcessObject (config);
				}
			}
			_queue.Clear();
		}

		private void ProcessType(Type type)
		{
			object obj = _injector.GetOrCreateNewInstance(type);
			InvokeConfigure (obj, type);
		}

		private void ProcessObject(object obj)
		{
			_injector.InjectInto (obj);
			InvokeConfigure (obj);
		}

		private void InvokeConfigure(object obj, Type type = null)
		{
			if (obj is IConfig)
				(obj as IConfig).Configure ();
			else 
			{
				if (type == null)
					type = obj.GetType ();
				MethodInfo method = type.GetMethod ("Configure", NO_ARGS);
				if (method != null)
					method.Invoke (obj, null);
			}
		}
	}

	public class TypeMatcher : IMatcher
	{
		public bool Matches(object obj)
		{
			return obj is Type;
		}
	}

	public class ObjectMatcher : IMatcher
	{
		public bool Matches(object obj)
		{
			return obj is Type == false;
		}
	}
}

