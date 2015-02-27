//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using robotlegs.bender.extensions.viewManager.api;


namespace robotlegs.bender.extensions.viewManager.impl
{
	public class ContainerBinding
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<ContainerBinding> BINDING_EMPTY;

		/// <summary>
		/// The parent binding in relation to this container
		/// </summary>
		/// <value>The parent.</value>
		public ContainerBinding Parent {get;set;}

		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>The container.</value>
		public object Container 
		{
			get
			{
				return _container;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		/// <summary>
		/// The view handlers added to the container
		/// </summary>
		private List<IViewHandler> _handlers = new List<IViewHandler>();

		private object _container;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/


		/// <summary>
		/// Initializes a new instance of the <see cref="robotlegs.bender.extensions.viewManager.impl.ContainerBinding"/> class.
		/// </summary>
		/// <param name="container">Container to be associated with the binding</param>
		public ContainerBinding (object container)
		{
			_container = container;
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/// <summary>
		/// Adds a view handler to this binding
		/// </summary>
		/// <param name="handler">Handler.</param>
		public void AddHandler(IViewHandler handler)
		{
			if (_handlers.Contains (handler))
				return;
			_handlers.Add(handler);
		}
		
		/// <summary>
		/// Removes a view handler from this binding
		/// </summary>
		public void RemoveHandler(IViewHandler handler)
		{
			_handlers.Remove(handler);

			if (_handlers.Count == 0 && BINDING_EMPTY != null)
			{
				BINDING_EMPTY (this);
			}
		}

		/// <summary>
		/// Will trigger all IViewHandlers added to this binding and pass it your view
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="type">Type.</param>
		public void HandleView(object view, Type type)
		{
			foreach (IViewHandler handler in _handlers)
			{
				handler.HandleView (view, type);
			}
		}
	}
}

