//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using NUnit.Framework;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.DSL;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.Support;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;
using System;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl
{
    [TestFixture]
    public class DirectAsyncCommandMapTest
    {
        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        #region Fields

        private IContext context;

        private IInjector injector;
        private DirectAsyncCommandMap subject;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

        [SetUp]
        public void before()
        {
            context = new Context();
            injector = context.injector;
            injector.Map<IDirectAsyncCommandMap>().ToType<DirectAsyncCommandMap>();
            subject = injector.GetInstance<IDirectAsyncCommandMap>() as DirectAsyncCommandMap;
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void commands_aborted()
        {
            var aborted = false;
            subject.SetCommandsAbortedCallback(() =>
            {
                aborted = true;
            });
            subject.Map<NullAsyncCommand>()
                .Map<AbortAsyncCommand>()
                .Map<NullAsyncCommand2>()
                .Execute();
            Assert.AreEqual(true, aborted);
        }

        [Test]
        public void commands_all_executed()
        {
            var allExecuted = false;
            subject.SetCommandsExecutedCallback(() =>
            {
                allExecuted = true;
            });

            subject.Map<NullAsyncCommand>().Map<NullAsyncCommand2>().Execute();
            Assert.AreEqual(true, allExecuted);
        }

        [Test]
        public void commands_get_injected_with_DirectCommandMap_instance()
        {
            IDirectAsyncCommandMap actual = null;
            injector.Map(typeof(Action<IDirectAsyncCommandMap>), "ReportingFunction").ToValue((Action<IDirectAsyncCommandMap>)delegate (IDirectAsyncCommandMap passed)
            {
                actual = passed;
            });

            subject.Map<DirectAsyncCommandMapReportingAsyncCommand>().Execute();

            Assert.That(actual, Is.EqualTo(subject));
        }

        [Test]
        public void detains_command()
        {
            object command = new object();
            bool wasDetained = false;
            context.Detained += delegate (object obj)
            {
                wasDetained = true;
            };
            context.Detain(command);

            Assert.That(wasDetained, Is.True);
        }

        [Test]
        public void map_creates_IOnceCommandConfig()
        {
            Assert.That(subject.Map<NullAsyncCommand>(), Is.InstanceOf<IDirectAsyncCommandConfigurator>());
        }

        [Test]
        public void mapping_processor_is_called()
        {
            int callCount = 0;
            subject.AddMappingProcessor((CommandMappingList.Processor)delegate (ICommandMapping mapping)
            {
                callCount++;
            });
            subject.Map<NullAsyncCommand>();
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void releases_command()
        {
            object command = new object();
            bool wasReleased = false;
            context.Released += delegate (object obj)
            {
                wasReleased = true;
            };
            context.Detain(command);

            context.Release(command);

            Assert.That(wasReleased, Is.True);
        }

        [Test]
        public void sandboxed_directCommandMap_instance_does_not_leak_into_system()
        {
            IDirectAsyncCommandMap actual = injector.GetInstance<IDirectAsyncCommandMap>();

            Assert.That(actual, Is.Not.EqualTo(subject));
        }

        #endregion Methods
    }
}