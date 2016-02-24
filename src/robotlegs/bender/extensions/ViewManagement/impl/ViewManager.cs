//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Robotlegs.Bender.Extensions.ViewManagement.API;

namespace Robotlegs.Bender.Extensions.ViewManagement.Impl
{
	/// <summary>
	/// View manager, will be made by each context.
	/// It can add containers which will be passed into the Container Registry.
	/// But if you add a second container view with it, it will clone all it's view handlers added to itself for you
	/// </summary>

	public class ViewManager : IViewManager
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<object> ContainerAdded
		{
			add
			{
				_containerAdded += value;
			}
			remove 
			{
				_containerAdded -= value;
			}
		}

		public event Action<object> ContainerRemoved
		{
			add
			{
				_containerRemoved += value;
			}
			remove 
			{
				_containerRemoved -= value;
			}
		}

		public event Action<IViewHandler> HandlerAdd
		{
			add
			{
				_handlerAdd += value;
			}
			remove 
			{
				_handlerAdd -= value;
			}
		}

		public event Action<IViewHandler> HandlerRemove
		{
			add
			{
				_handlerRemove += value;
			}
			remove 
			{
				_handlerRemove -= value;
			}
		}

		public List<object> Containers
		{
			get
			{
				return _containers;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private Action<object> _containerAdded;

		private Action<object> _containerRemoved;

		private Action<IViewHandler> _handlerAdd;

		private Action<IViewHandler> _handlerRemove;

		private List<IViewHandler> _handlers = new List<IViewHandler>();

		private List<object> _containers = new List<object>();

		private object _fallbackContainer;

		private ContainerRegistry _registry;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public ViewManager(ContainerRegistry containerRegistry)
		{
			_registry = containerRegistry;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void AddContainer(object container)
		{
			if (!ValidContainer(container))
				return;

			_containers.Add(container);

			ContainerBinding containerBinding = _registry.AddContainer(container);
			AddHandlers (containerBinding);

			if (_containerAdded != null)
			{
				_containerAdded (container);
			}
		}

		public void RemoveContainer(object container)
		{
			if (!_containers.Remove (container))
				return;	

			ContainerBinding binding = _registry.GetBinding (container);
			if (binding != null) 
			{
				RemoveHandlers (binding);
			}

			if (_containerRemoved != null)
			{
				_containerRemoved (container);
			}
		}

		public void SetFallbackContainer(object container)
		{
			_fallbackContainer = container;

			ContainerBinding containerBinding = _registry.SetFallbackContainer (container);
			RemoveAllHandlers (false);
			AddHandlers (containerBinding);

			_registry.FallbackContainerRemove += FallbackContainerBindingRemoved;
		}

		public void RemoveFallbackContainer()
		{
			if (_fallbackContainer == null)
				return;

			_registry.RemoveFallbackContainer ();

		}

		public void AddViewHandler(IViewHandler handler)
		{
			if (_handlers.Contains(handler))
				return;

			_handlers.Add(handler);

			// Add new handler to our containers
			if (_fallbackContainer != null) 
			{
				_registry.GetBinding (_fallbackContainer).AddHandler (handler);
			} 
			else 
			{
				foreach (object container in _containers) 
				{
					_registry.AddContainer (container).AddHandler (handler);
				}
			}

			if (_handlerAdd != null)
			{
				_handlerAdd (handler);
			}
		}

		public void RemoveViewHandler(IViewHandler handler)
		{
			_handlers.Remove(handler);

			foreach(object container in _containers)
			{
				_registry.GetBinding(container).RemoveHandler(handler);
			}

			if (_handlerRemove != null)
			{
				_handlerRemove (handler);
			}
		}

		public void RemoveAllHandlers()
		{
			RemoveAllHandlers (true);
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private void RemoveAllHandlers(bool includeFallback)
		{
			if (includeFallback && _fallbackContainer != null) 
			{
				_registry.RemoveContainer (_fallbackContainer);
			}

			foreach (object container in _containers) 
			{
				ContainerBinding binding = _registry.GetBinding (container);
				RemoveHandlers (binding);
			}
		}

		private void AddAllHandlers()
		{
			foreach (object container in _containers) 
			{
				ContainerBinding binding = _registry.AddContainer (container);
				AddHandlers (binding);
			}
		}

		private void FallbackContainerBindingRemoved(object container)
		{
			_registry.FallbackContainerRemove -= FallbackContainerBindingRemoved;

			_fallbackContainer = null;

			AddAllHandlers ();
		}

		private void AddHandlers(ContainerBinding binding)
		{
			foreach (IViewHandler handler in _handlers)
			{
				binding.AddHandler(handler);
			}
		}

		private void RemoveHandlers(ContainerBinding binding)
		{
			foreach (IViewHandler handler in _handlers)
			{
				binding.RemoveHandler (handler);
			}
		}

		private bool ValidContainer(object container)
		{
			//TODO: Check for nested containers and already existing containers with this ViewManager
			if (_registry.FallbackBinding != null && _registry.FallbackBinding.Container == container)
				return true;
			foreach (object registeredContainer in _containers)
			{
				if (container == registeredContainer)
					return false;

				if (_registry.FallbackBinding != null && _registry.FallbackBinding.Container == registeredContainer)
					return false;

				if (_registry.Contains (registeredContainer, container) || _registry.Contains (container, registeredContainer))
					throw new Exception ("Containers can not be nested");
			}
			return true;
		}
	}
}

