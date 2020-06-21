//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl;
using System;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Support
{
    public class DirectAsyncCommandMapReportingAsyncCommand : AsyncCommand
    {
        #region Fields

        [Inject]
        public IDirectAsyncCommandMap dcm;

        [Inject("ReportingFunction")]
        public Action<IDirectAsyncCommandMap> reportingFunc;

        #endregion Fields

        #region Methods

        public override void Execute()
        {
            reportingFunc.Invoke(dcm);
        }

        #endregion Methods
    }
}