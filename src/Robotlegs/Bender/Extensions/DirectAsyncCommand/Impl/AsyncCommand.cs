//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using System;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl
{
    public abstract class AsyncCommand : IAsyncCommand
    {
        /*============================================================================*/
        /* Public Properties                                                                */
        /*============================================================================*/

        #region Properties

        [Inject(true, "AsyncCommandExecutedCallback")]
        public Action<IAsyncCommand, bool> ExecutedCallback
        {
            get;
            protected set;
        }

        #endregion Properties

        /*============================================================================*/
        /* Public Functions                                                           */
        /*============================================================================*/

        #region Methods

        public virtual void Abort()
        {
            Executed(true);
        }

        public abstract void Execute();

        /*============================================================================*/
        /* Protected Functions                                                           */
        /*============================================================================*/

        protected virtual void Executed(bool stop = false)
        {
            ExecutedCallback?.Invoke(this, stop);
        }

        #endregion Methods
    }
}