//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.CommandCenter.API;
using System;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.API
{
    /// <summary>
    /// AsyncCommand interface
    /// </summary>
    /// <seealso cref="Robotlegs.Bender.Extensions.CommandCenter.API.ICommand" />
    public interface IAsyncCommand : ICommand
    {
        #region Properties

        /// <summary>
        /// Gets the executed callback.
        /// </summary>
        /// <value>The executed callback.</value>
        Action<IAsyncCommand, bool> ExecutedCallback
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Aborts asynchronous operation.
        /// </summary>
        void Abort();

        #endregion Methods
    }
}