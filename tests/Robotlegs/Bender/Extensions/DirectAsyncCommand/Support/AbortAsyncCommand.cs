//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Support
{
    public class AbortAsyncCommand : AsyncCommand
    {
        public override void Execute()
        {
            Abort();
        }
    }
}