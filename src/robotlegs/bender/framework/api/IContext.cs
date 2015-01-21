using System;
using robotlegs.bender.framework.impl;

namespace robotlegs.bender.framework.api
{
	public interface IContext : IPinEvent, ILifecycleEvent
	{
		/// <summary>
		/// The injection binder this context relies on. Use this to bind and unbind anything you require
		/// </summary>
		/// <value>The injection binder.</value>
		IInjector injector { get; }
		
		/// <summary>
		/// Gets or sets the current log level
		/// </summary>
		/// <value>The log level</value>
		LogLevel LogLevel {get;set;}	

		/// <summary>
		/// Returns if the app has been initialized yet
		/// </summary>
		/// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
		bool Uninitialized { get; }
		bool Initialized { get; }
		bool Active { get; }
		bool Suspended { get; }
		bool Destroyed { get; }
		/// <summary>
		/// Will initialized the context. Doing so will
		/// - Fire the pre initilize callbacks
		/// - Process all the configs that have been added
		/// - Fire the post initilize callbacks
		/// </summary>
		void Initialize(Action callback = null);
		void Suspend (Action callback = null);
		void Resume (Action callback = null);
		void Destroy (Action callback = null);

		IContext Install<T>() where T : IExtension;
		IContext Install (Type type);
		IContext Install (IExtension extension);

		IContext Configure<T>() where T : class;
		IContext Configure(params object[] objects);

		/// <summary>
		/// Adds a callback function to be called when before the app has been initialized. This is before the configs have been processed
		/// </summary>
		/// <returns>The pre initialized callback.</returns>
		/// <param name="callback">Callback.</param>
		IContext BeforeInitializing (Action callback);
		IContext BeforeInitializing (MessageDispatcher.HandlerMessageDelegate handler);
		IContext BeforeInitializing (MessageDispatcher.HandlerMessageCallbackDelegate handler);
		IContext WhenInitializing (Action callback);
		IContext AfterInitializing (Action callback);

		IContext BeforeSuspending (Action callback);
		IContext BeforeSuspending (MessageDispatcher.HandlerMessageDelegate handler);
		IContext BeforeSuspending (MessageDispatcher.HandlerMessageCallbackDelegate handler);
		IContext WhenSuspending (Action callback);
		IContext AfterSuspending (Action callback);

		IContext BeforeResuming (Action callback);
		IContext BeforeResuming (MessageDispatcher.HandlerMessageDelegate handler);
		IContext BeforeResuming (MessageDispatcher.HandlerMessageCallbackDelegate handler);
		IContext WhenResuming (Action callback);
		IContext AfterResuming (Action callback);

		IContext BeforeDestroying (Action callback);
		IContext BeforeDestroying (MessageDispatcher.HandlerMessageDelegate handler);
		IContext BeforeDestroying (MessageDispatcher.HandlerMessageCallbackDelegate handler);
		IContext WhenDestroying (Action callback);
		IContext AfterDestroying(Action callback);

		/// <summary>
		/// Adds a config match and handler for processing configs.
		/// Generally you would call this from an extension to add more functionality to your configs if needed
		/// </summary>
		/// <returns>The config handler.</returns>
		/// <param name="matcher">The matching conditions run on all configs</param>
		/// <param name="handler">The process match function to run when the match has succeded on a config</param>
		IContext AddConfigHandler(IMatcher matcher, Action<object> handler);

		/// <summary>
		/// Retrieves a logger for a given source
		/// </summary>
		/// <returns>this</returns>
		/// <param name="source">Logging source</param>
		ILogger GetLogger(Object source);

		/// <summary>
		/// Adds a custom log target
		/// </summary>
		/// <returns>this</returns>
		/// <param name="target">Log Target</param>
		IContext AddLogTarget(ILogTarget target);


		IContext Detain (params object[] instances);
		IContext Release (params object[] instances);

		IContext AddChild(IContext child);
		IContext RemoveChild(IContext child);
	}
}

