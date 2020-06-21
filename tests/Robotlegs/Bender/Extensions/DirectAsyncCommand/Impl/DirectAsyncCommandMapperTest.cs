//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using Moq;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.Support;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.DSL;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.Support;
using Robotlegs.Bender.Framework.Impl.GuardSupport;
using System.Collections.Generic;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand.Impl
{
    [TestFixture]
    public class DirectAsyncCommandMapperTest
    {
        #region Fields

        private ICommandMapping caughtMapping = null;
        private Mock<IAsyncCommandExecutor> mockExecutor;
        private Mock<ICommandMappingList> mockMappingList;
        private DirectAsyncCommandMapper subject;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

        [TearDown]
        public void after()
        {
            caughtMapping = null;
            subject = null;
        }

        [SetUp]
        public void before()
        {
            mockExecutor = new Mock<IAsyncCommandExecutor>();
            mockMappingList = new Mock<ICommandMappingList>();
            mockMappingList.Setup(m => m.AddMapping(It.IsAny<ICommandMapping>())).Callback<ICommandMapping>(r => caughtMapping = r);
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void calls_executor_executeAsyncCommands_with_arguments()
        {
            List<ICommandMapping> list = new List<ICommandMapping>();
            mockMappingList.Setup(m => m.GetList()).Returns(list);
            CreateMapper<NullAsyncCommand>().Execute(null);

            mockExecutor.Verify(e => e.ExecuteAsyncCommands(It.Is<List<ICommandMapping>>(arg1 => arg1 == list), It.Is<CommandPayload>(arg2 => arg2 == null)), Times.Once);
        }

        [Test]
        public void map_creates_new_mapper_instance()
        {
            IDirectAsyncCommandConfigurator newMapper = CreateMapper<NullAsyncCommand>().Map<NullAsyncCommand2>();

            Assert.That(newMapper, Is.Not.Null);
            Assert.That(newMapper, Is.Not.EqualTo(subject));
        }

        [Test]
        public void mapping_is_fireOnce_by_default()
        {
            CreateMapper<NullAsyncCommand>();

            Assert.That(caughtMapping.FireOnce, Is.True);
        }

        [Test]
        public void registers_new_commandMapping_with_CommandMappingList()
        {
            CreateMapper<NullAsyncCommand>();

            Assert.That(caughtMapping, Is.InstanceOf<ICommandMapping>());
        }

        [Test]
        public void withExecuteMethod_sets_executeMethod_of_mapping()
        {
            string executeMethod = "otherThanExecute";
            CreateMapper<NullAsyncCommand>().WithExecuteMethod(executeMethod);

            Assert.That(caughtMapping.ExecuteMethod, Is.EqualTo(executeMethod));
        }

        [Test]
        public void withGuards_sets_guards_of_mapping()
        {
            object[] expected = new object[] { typeof(HappyGuard), typeof(GrumpyGuard) };
            CreateMapper<NullAsyncCommand>().WithGuards(expected);

            Assert.That(caughtMapping.Guards, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void withHooks_sets_hooks_of_mapping()
        {
            object[] expected = new object[] { typeof(ClassReportingCallbackHook), typeof(ClassReportingCallbackHook) };
            CreateMapper<NullAsyncCommand>().WithHooks(expected);

            Assert.That(caughtMapping.Hooks, Is.EqualTo(expected).AsCollection);
        }

        [Test]
        public void withPayloadInjection_sets_payloadInjection_of_mapping()
        {
            CreateMapper<NullAsyncCommand>().WithPayloadInjection(false);

            Assert.That(caughtMapping.PayloadInjectionEnabled, Is.False);
        }

        /*============================================================================*/
        /* Private Functions                                                          */
        /*============================================================================*/

        private DirectAsyncCommandMapper CreateMapper<T>()
        {
            subject = new DirectAsyncCommandMapper(mockExecutor.Object, mockMappingList.Object, typeof(T));
            return subject;
        }

        #endregion Methods
    }
}