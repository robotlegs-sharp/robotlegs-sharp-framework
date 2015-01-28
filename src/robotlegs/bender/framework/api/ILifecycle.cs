using System;

namespace robotlegs.bender.framework.api
{
	public interface ILifecycle : ILifecycleEvent
	{
		/**
		 * The current lifecycle state of the target object
		 */
		LifecycleState state {get;}

		/**
		 * Is this object uninitialized?
		 */
		bool Uninitialized { get; }

		/**
		 * Has this object been fully initialized?
		 */
		bool Initialized { get; }

		/**
		 * Is this object currently active?
		 */
		bool Active {get;}

		/**
		 * Has this object been fully suspended?
		 */
		bool Suspended { get; }

		/**
		 * Has this object been fully destroyed?
		 */
		bool Destroyed {get;}

		/**
		 * Initializes the lifecycle
		 * @param callback Initialization callback
		 */
		void Initialize (Action callback = null);

		/**
		 * Suspends the lifecycle
		 * @param callback Suspension callback
		 */
		void Suspend(Action callback = null);

		/**
		 * Resumes a suspended lifecycle
		 * @param callback Resumption callback
		 */
		void Resume(Action callback = null);

		/**
		 * Destroys an active lifecycle
		 * @param callback Destruction callback
		 */
		void Destroy(Action callback = null);

		/**
		 * A handler to run before the target object is initialized
		 *
		 * <p>The handler can be asynchronous. See: readme-async</p>
		 *
		 * @param handler Pre-initialize handler
		 * @return Self
		 */
		ILifecycle BeforeInitializing(Action callback);
		ILifecycle BeforeInitializing (HandlerMessageDelegate handler);
		ILifecycle BeforeInitializing (HandlerMessageCallbackDelegate handler);
		
		/**
		 * A handler to run during initialization
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Initialization handler
		 * @return Self
		 */
		ILifecycle WhenInitializing(Action handler);

		/**
		 * A handler to run after initialization
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Post-initialize handler
		 * @return Self
		 */
		ILifecycle AfterInitializing(Action handler);

		/**
		 * A handler to run before the target object is suspended
		 *
		 * <p>The handler can be asynchronous. See: readme-async</p>
		 *
		 * @param handler Pre-suspend handler
		 * @return Self
		 */
		ILifecycle BeforeSuspending(Action handler);
		ILifecycle BeforeSuspending (HandlerMessageDelegate handler);
		ILifecycle BeforeSuspending (HandlerMessageCallbackDelegate handler);

		/**
		 * A handler to run during suspension
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Suspension handler
		 * @return Self
		 */
		ILifecycle WhenSuspending(Action handler);

		/**
		 * A handler to run after suspension
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Post-suspend handler
		 * @return Self
		 */
		ILifecycle AfterSuspending(Action handler);

		/**
		 * A handler to run before the target object is resumed
		 *
		 * <p>The handler can be asynchronous. See: readme-async</p>
		 *
		 * @param handler Pre-resume handler
		 * @return Self
		 */
		ILifecycle BeforeResuming(Action handler);
		ILifecycle BeforeResuming (HandlerMessageDelegate handler);
		ILifecycle BeforeResuming (HandlerMessageCallbackDelegate handler);

		/**
		 * A handler to run during resumption
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Resumption handler
		 * @return Self
		 */
		ILifecycle WhenResuming(Action handler);

		/**
		 * A handler to run after resumption
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Post-resume handler
		 * @return Self
		 */
		ILifecycle AfterResuming(Action handler);

		/**
		 * A handler to run before the target object is destroyed
		 *
		 * <p>The handler can be asynchronous. See: readme-async</p>
		 *
		 * @param handler Pre-destroy handler
		 * @return Self
		 */
		ILifecycle BeforeDestroying(Action handler);
		ILifecycle BeforeDestroying (HandlerMessageDelegate handler);
		ILifecycle BeforeDestroying (HandlerMessageCallbackDelegate handler);

		/**
		 * A handler to run during destruction
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Destruction handler
		 * @return Self
		 */
		ILifecycle WhenDestroying(Action handler);

		/**
		 * A handler to run after destruction
		 *
		 * <p>Note: The handler must be synchronous.</p>
		 * @param handler Post-destroy handler
		 * @return Self
		 */
		ILifecycle AfterDestroying(Action handler);
	}
}

