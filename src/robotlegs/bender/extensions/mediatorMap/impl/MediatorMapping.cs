//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.extensions.matching;
using System.Collections.Generic;
using robotlegs.bender.extensions.mediatorMap.dsl;


namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorMapping : IMediatorMapping, IMediatorConfigurator
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/
		
		private ITypeFilter _matcher;
		
		/**
		 * @inheritDoc
		 */
		public ITypeFilter Matcher
		{
			get
			{ 
				return _matcher;
			}
		}
		
		private Type _mediatorType;
		
		/**
		 * @inheritDoc
		 */
		public Type MediatorType
		{
			get
			{
				return _mediatorType;
			}
		}
		
		private List<object> _guards = new List<object>();
		
		/**
		 * @inheritDoc
		 */
		public List<object> Guards
		{
			get
			{
				return _guards;
			}
		}
		
		private List<object> _hooks = new List<object>();
		
		/**
		 * @inheritDoc
		 */
		public List<object> Hooks
		{
			get
			{
				return _hooks;
			}
		}
		
		private bool _autoRemoveEnabled = true;
		
		/**
		 * @inheritDoc
		 */
		public bool AutoRemoveEnabled
		{
			get
			{
				return _autoRemoveEnabled;
			}
		}

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public MediatorMapping (ITypeFilter matcher, Type mediatorType)
		{
			_matcher = matcher;
			_mediatorType = mediatorType;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public IMediatorConfigurator WithGuards(params object[] guards)
		{
			_guards.AddRange (guards);
			return this;
		}

		public IMediatorConfigurator WithGuards<T>()
		{
			return WithGuards (typeof(T));
		}

		public IMediatorConfigurator WithGuards<T1, T2>()
		{
			return WithGuards (typeof(T1), typeof(T2));
		}

		public IMediatorConfigurator WithGuards<T1, T2, T3>()
		{
			return WithGuards (typeof(T1), typeof(T2), typeof(T3));
		}

		public IMediatorConfigurator WithGuards<T1, T2, T3, T4>()
		{
			return WithGuards (typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public IMediatorConfigurator WithGuards<T1, T2, T3, T4, T5>()
		{
			return WithGuards (typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}


		public IMediatorConfigurator WithHooks(params object[] hooks)
		{
			_hooks.AddRange (hooks);
			return this;
		}

		public IMediatorConfigurator WithHooks<T>()
		{
			return WithHooks (typeof(T));
		}

		public IMediatorConfigurator WithHooks<T1, T2>()
		{
			return WithHooks (typeof(T1), typeof(T2));
		}

		public IMediatorConfigurator WithHooks<T1, T2, T3>()
		{
			return WithHooks (typeof(T1), typeof(T2), typeof(T3));
		}

		public IMediatorConfigurator WithHooks<T1, T2, T3, T4>()
		{
			return WithHooks (typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public IMediatorConfigurator WithHooks<T1, T2, T3, T4, T5>()
		{
			return WithHooks (typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}


		public IMediatorConfigurator AutoRemove(bool value)
		{
			_autoRemoveEnabled = value;
			return this;
		}
	}
}

