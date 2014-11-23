using System;
using System.Collections.Generic;
using robotlegs.bender.framework.api;

namespace robotlegs.bender.framework.impl
{
	public class ConfigManager
	{
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
			AddConfigHandler (new MatchTypeIConfig (), HandleIConfigType);
			AddConfigHandler (new MatchIConfig (), HandleIConfigObject);
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
		
		public void Initialize()
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

		private void HandleIConfigType(object config)
		{
			if (_initialized)
			{
				_logger.Debug("Already initialized. Instantiating config type {0}", config);
				ProcessIConfigType(config);
			}
			else
			{
				_logger.Debug("Not yet initialized. Queuing config class {0}", config);
				_queue.Add(config);
			}
		}

		private void HandleIConfigObject(object config)
		{
			if (_initialized)
			{
				_logger.Debug("Already initialized. Injecting into config object {0}", config);
				ProcessIConfigObject(config);
			}
			else
			{
				_logger.Debug("Not yet initialized. Queuing config object {0}", config);
				_queue.Add(config);
			}
		}
		
		private void ProcessQueue()
		{
			foreach (object config in _queue)
			{
				if (config is Type)
				{
					_logger.Debug("Now initializing. Instantiating config class {0}", config);
					ProcessIConfigType(config);
				}
				else
				{
					_logger.Debug("Now initializing. Injecting into config object {0}", config);
					ProcessIConfigObject(config);
				}
			}
			_queue.Clear();
		}

		private void ProcessIConfigType(object configType)
		{
			IConfig config = _injector.GetOrCreateNewInstance (configType as Type) as IConfig;
			if (config != null) config.Configure ();
		}

		private void ProcessIConfigObject(object config)
		{
			IConfig typedConfig = config as IConfig;
			_injector.InjectInto (typedConfig);
			if (typedConfig != null) typedConfig.Configure ();
		}
	}
	
	public class MatchTypeIConfig : IMatcher
	{
		public bool Matches(object obj)
		{
			return (obj is Type && typeof(IConfig).IsAssignableFrom(obj as Type));
		}
	}
	
	public class MatchIConfig : IMatcher
	{
		public bool Matches(object obj)
		{
			return obj is IConfig;
		}
	}
}

