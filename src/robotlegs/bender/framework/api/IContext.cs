using System;
using robotlegs.bender.framework.impl;

namespace robotlegs.bender.framework.api
{
	public interface IContext
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
		bool initialized { get; }
		/// <summary>
		/// Will initialized the context. Doing so will
		/// - Fire the pre initilize callbacks
		/// - Process all the configs that have been added
		/// - Fire the post initilize callbacks
		/// </summary>
		IContext Initialize();

		IContext Install<T>() where T : IExtension;

		IContext Configure<T>() where T : class;
		IContext Configure(params object[] objects);

		/// <summary>
		/// Adds a callback function to be called when before the app has been initialized. This is before the configs have been processed
		/// </summary>
		/// <returns>The pre initialized callback.</returns>
		/// <param name="callback">Callback.</param>
		IContext AddPreInitializedCallback (ContextStateCallback.CallbackDelegate callback);

		/// <summary>
		/// Adds a callback function to be called when the app has been initialized, this is after all the configs have been processed
		/// </summary>
		/// <returns>The post initialized callback.</returns>
		/// <param name="callback">Callback.</param>
		IContext AddPostInitializedCallback (ContextStateCallback.CallbackDelegate callback);

		/// <summary>
		/// TODO
		/// </summary>
		/// <returns>The pre destroy callback.</returns>
		/// <param name="callback">Callback.</param>
		IContext AddPreDestroyCallback (ContextStateCallback.CallbackDelegate callback);

		/// <summary>
		/// TODO
		/// </summary>
		/// <returns>The post destroy callback.</returns>
		/// <param name="callback">Callback.</param>
		IContext AddPostDestroyCallback(ContextStateCallback.CallbackDelegate callback);

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
	}
}

