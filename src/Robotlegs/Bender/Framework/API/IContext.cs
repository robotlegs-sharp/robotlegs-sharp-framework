//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using System;
using Robotlegs.Bender.Framework.Impl;

namespace Robotlegs.Bender.Framework.API
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
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// Is this context uninitialized?
        /// </summary>
        bool Uninitialized { get; }

        /// <summary>
        /// Is this context initialized?
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Is this context active?
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// Is this context suspended?
        /// </summary>
        bool Suspended { get; }

        /// <summary>
        /// Has this context been destroyed?
        /// </summary>
        bool Destroyed { get; }

        /// <summary>
        /// Will initialized the context. Doing so will
        /// - Fire the pre initilize callbacks
        /// - Process all the configs that have been added
        /// - Fire the post initilize callbacks
        /// </summary>
        /// <param name="callback">Initialization callback</param>
        IContext Initialize(Action callback = null);

        /// <summary>
        /// Suspends this context
        /// </summary>
        /// <param name="callback">Suspension callback.</param>
        IContext Suspend(Action callback = null);

        /// <summary>
        /// Suspends this context (if active).
        /// </summary>
        /// <param name="callback">Resume callback.</param>
        IContext Resume(Action callback = null);

        /// <summary>
        /// Destroys this context (if alive)
        /// </summary>
        /// <param name="callback">Destruction callback.</param>
        IContext Destroy(Action callback = null);

        /// <summary>
        /// Determines whether the custom extensions or bundles is installed.
        /// </summary>
        /// <typeparam name="T">IExtension class</typeparam>
        /// <returns>
        /// <c>true</c> if the custom extensions or bundles is installed; otherwise, <c>false</c>.
        /// </returns>
        bool IsInstalled<T>() where T : IExtension;

        /// <summary>
        /// Determines whether the custom extensions or bundles is installed.
        /// </summary>
        /// <param name="type">Type with an 'Extend(IContext context)' method signature.</param>
        /// <returns>
        /// <c>true</c> ifthe custom extensions or bundles is installed; otherwise, <c>false</c>.
        /// </returns>
        bool IsInstalled(Type type);

        /// <summary>
        /// Installs custom extensions or bundles into the context
        /// </summary>
        /// <typeparam name="T">IExtension class</typeparam>
        IContext Install<T>() where T : IExtension;

        /// <summary>
        /// Installs custom extensions or bundles into the context
        /// </summary>
        /// <param name="type">Type with an 'Extend(IContext context)' method signature</param>
        IContext Install(Type type);

        /// <summary>
        /// Installs custom extensions or bundles into the context
        /// </summary>
        /// <param name="extension">IExtension instance</param>
        IContext Install(IExtension extension);

        /// <summary>
        /// Configures the context with custom configurations
        /// </summary>
        /// <typeparam name="T">Configuration class</typeparam>
        IContext Configure<T>() where T : class;

        /// <summary>
        /// Configures the context with custom configurations
        /// </summary>
        /// <param name="objects">Configuration objects</param>
        IContext Configure(params object[] objects);

        /// <summary>
        /// A handler to run before the context is initialized. This is before the configs have been processed
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Callback to fire</param>
        IContext BeforeInitializing(Action callback);

        /// <summary>
        /// A handler to run before the context is initialized. This is before the configs have been processed
        /// </summary>
        /// <returns>This</returns>
        /// <param name="handler">Handler to fire</param>
        IContext BeforeInitializing(HandlerMessageDelegate handler);

        /// <summary>
        /// A asynchronous handler to run before the context is initialized. This is before the
        /// configs have been processed
        /// </summary>
        /// <returns>This</returns>
        /// <param name="handler">
        /// Handler to fire, you must fire the callback given to the hander to continue initialization
        /// </param>
        IContext BeforeInitializing(HandlerMessageCallbackDelegate handler);

        /// <summary>
        /// A handler to run during initialization
        /// </summary>
        /// <returns>The initializing.</returns>
        /// <param name="callback">Initialization callback</param>
        IContext WhenInitializing(Action callback);

        /// <summary>
        /// A handler to run after initialization
        /// </summary>
        /// <returns>The initializing.</returns>
        /// <param name="callback">Post-initialize callback</param>
        IContext AfterInitializing(Action callback);

        /// <summary>
        /// A handler to run before the target object is suspended
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Callback to fire</param>
        IContext BeforeSuspending(Action callback);

        /// <summary>
        /// A handler to run before the target object is suspended
        /// </summary>
        /// <returns>This</returns>
        /// <param name="handler">Handler to fire</param>
        IContext BeforeSuspending(HandlerMessageDelegate handler);

        /// <summary>
        /// A asynchronous handler to run before the target object is suspended
        /// </summary>
        /// <returns>This</returns>
        /// <param name="handler">
        /// Handler to fire, you must fire the callback given to the hander to continue suspension
        /// </param>
        IContext BeforeSuspending(HandlerMessageCallbackDelegate handler);

        /// <summary>
        /// A handler to run during suspension
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Suspension callback</param>
        IContext WhenSuspending(Action callback);

        /// <summary>
        /// A handler to run after suspension
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Post suspend callback</param>
        IContext AfterSuspending(Action callback);

        /// <summary>
        /// A handler to run before the context is resumed
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Pre-resume callback</param>
        IContext BeforeResuming(Action callback);

        /// <summary>
        /// A handler to run before the context is resumed
        /// </summary>
        /// <returns>Pre-resume handler</returns>
        IContext BeforeResuming(HandlerMessageDelegate handler);

        /// <summary>
        /// A asynchronous handler to run before the context is resumed
        /// </summary>
        /// <param name="handler">
        /// Handler to fire, you must fire the callback given to the hander to continue resumption
        /// </param>
        IContext BeforeResuming(HandlerMessageCallbackDelegate handler);

        /// <summary>
        /// A handler to run before the context is resumed
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Resumption callback</param>
        IContext WhenResuming(Action callback);

        /// <summary>
        /// A handler to run before the context is resumed
        /// </summary>
        /// <returns>This</returns>
        /// <param name="callback">Post resume callback.</param>
        IContext AfterResuming(Action callback);

        /// <summary>
        /// A handler to run before the context is destroyed
        /// </summary>
        /// <returns>The destroying.</returns>
        /// <param name="callback">Pre-destroy callback.</param>
        IContext BeforeDestroying(Action callback);

        /// <summary>
        /// A handler to run before the context is destroyed
        /// </summary>
        /// <returns>The destroying.</returns>
        /// <param name="handler">
        /// Handler to fire, you must fire the callback given to the hander to continue destruction
        /// </param>
        IContext BeforeDestroying(HandlerMessageDelegate handler);

        /// <summary>
        /// A handler to run before the context is destroyed
        /// </summary>
        /// <returns>The destroying.</returns>
        /// <param name="handler">Pre-destroy handler.</param>
        IContext BeforeDestroying(HandlerMessageCallbackDelegate handler);

        /// <summary>
        /// A handler to run during destruction
        /// </summary>
        /// <returns>The destroying.</returns>
        /// <param name="callback">Destruction callback.</param>
        IContext WhenDestroying(Action callback);

        /// <summary>
        /// A handler to run after destruction
        /// </summary>
        /// <returns>The destroying.</returns>
        /// <param name="callback">Post destroy callback.</param>
        IContext AfterDestroying(Action callback);

        /// <summary>
        /// Adds a config match and handler for processing configs. Generally you would call this
        /// from an extension to add more functionality to your configs if needed
        /// </summary>
        /// <returns>The config handler.</returns>
        /// <param name="matcher">The matching conditions run on all configs</param>
        /// <param name="handler">
        /// The process match function to run when the match has succeded on a config
        /// </param>
        IContext AddConfigHandler(IMatcher matcher, Action<object> handler);

        /// <summary>
        /// Retrieves a logger for a given source
        /// </summary>
        /// <returns>this</returns>
        /// <param name="source">Logging source</param>
        ILogging GetLogger(Object source);

        /// <summary>
        /// Adds a custom log target
        /// </summary>
        /// <returns>this</returns>
        /// <param name="target">Log Target</param>
        IContext AddLogTarget(ILogTarget target);

        /// <summary>
        /// Pins instances in memory
        /// </summary>
        /// <param name="instances">Instances to pin</param>
        IContext Detain(params object[] instances);

        /// <summary>
        /// Unpins instances from memory
        /// </summary>
        /// <param name="instances">Instances to unpin</param>
        IContext Release(params object[] instances);

        /// <summary>
        /// Adds an uninitialized context as a child.
        /// <para>This setups up an injection chain.</para>
        /// </summary>
        /// <returns>This</returns>
        /// <param name="child">The context to add as a child</param>
        IContext AddChild(IContext child);

        /// <summary>
        /// Removes a child context from this context
        /// </summary>
        /// <returns>This</returns>
        /// <param name="child">The child context to remove</param>
        IContext RemoveChild(IContext child);
    }
}