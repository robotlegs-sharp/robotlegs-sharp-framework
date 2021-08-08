//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand
{
    public class DirectAsyncCommandMapExtension : IExtension
    {
        /*============================================================================*/
        /* Public Functions                                                           */
        /*============================================================================*/

        public void Extend(IContext context)
        {
            context.injector.Map(typeof(IDirectAsyncCommandMap)).ToType(typeof(Impl.DirectAsyncCommandMap));
        }
    }
}