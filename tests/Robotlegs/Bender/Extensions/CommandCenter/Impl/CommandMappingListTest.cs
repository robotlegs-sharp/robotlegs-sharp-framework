//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Robotlegs.Bender.Extensions.CommandCenter.Support;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Framework.API;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
    public class CommandMappingListTest
    {
        /*============================================================================*/
        /* Public Properties                                                          */
        /*============================================================================*/


        #region Fields

        public Mock<ILogging> logger;

        public Mock<ICommandTrigger> trigger;

        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        private ICommandMapping mapping1;
        private ICommandMapping mapping2;
        private ICommandMapping mapping3;
        private List<CommandMappingList.Processor> processors;
        private CommandMappingList subject;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

        [SetUp]
        public void before()
        {
            logger = new Mock<ILogging>();
            trigger = new Mock<ICommandTrigger>();
            processors = new List<CommandMappingList.Processor>();
            subject = new CommandMappingList(trigger.Object, processors, logger.Object);
            mapping1 = new CommandMapping(typeof(NullCommand));
            mapping2 = new CommandMapping(typeof(NullCommand2));
            mapping3 = new CommandMapping(typeof(NullCommand3));
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void getList_returns_similar_list()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            Assert.That(subject.GetList(), Is.EquivalentTo(subject.GetList()));
        }

        [Test]
        public void getList_returns_unique_list()
        {
            Assert.That(subject.GetList().GetHashCode(), Is.Not.EqualTo(subject.GetList().GetHashCode()));
        }

        [Test]
        public void list_has_mapping()
        {
            subject.AddMapping(mapping1);
            Assert.That(subject.GetList().IndexOf(mapping1), Is.EqualTo(0));
        }

        [Test]
        public void list_is_empty()
        {
            Assert.That(subject.GetList().Count, Is.EqualTo(0));
        }

        [Test]
        public void list_is_empty_after_mappings_are_removed()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.RemoveMapping(mapping1);
            subject.RemoveMapping(mapping2);
            Assert.That(subject.GetList().Count, Is.EqualTo(0));
        }

        [Test]
        public void list_is_empty_after_removeAll()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            subject.RemoveAllMappings();
            Assert.That(subject.GetList().Count, Is.EqualTo(0));
        }

        [Test]
        public void list_not_empty_after_mapping_added()
        {
            subject.AddMapping(mapping1);
            Assert.That(subject.GetList().Count, Is.EqualTo(1));
        }

        [Test]
        public void mapping_processor_is_called()
        {
            int callCount = 0;
            processors.Add((CommandMappingList.Processor)delegate (ICommandMapping mapping)
            {
                callCount++;
            });
            subject.AddMapping(mapping1);
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void mapping_processor_is_given_mappings()
        {
            List<ICommandMapping> mappings = new List<ICommandMapping>();
            processors.Add((CommandMappingList.Processor)delegate (ICommandMapping mapping)
            {
                mappings.Add(mapping);
            });
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            Assert.That(mappings, Is.EqualTo(new List<ICommandMapping>() { mapping1, mapping2, mapping3 }).AsCollection);
        }

        [Test]
        public void sortFunction_is_called_after_mappings_are_added()
        {
            Mock<IComparer<ICommandMapping>> priorityComparer = new Mock<IComparer<ICommandMapping>>();
            priorityComparer.Setup(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>())).Returns(0);
            subject.WithSortFunction(priorityComparer.Object);
            addPriorityMappings();
            subject.GetList();
            priorityComparer.Verify(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>()), Times.AtLeastOnce);
        }

        [Test]
        public void sortFunction_is_not_called_after_a_mapping_is_removed()
        {
            Mock<IComparer<ICommandMapping>> priorityComparer = new Mock<IComparer<ICommandMapping>>();
            priorityComparer.Setup(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>())).Returns(0);
            subject.WithSortFunction(priorityComparer.Object);

            addPriorityMappings();
            subject.GetList();
            priorityComparer.Invocations.Clear();
            subject.RemoveMappingFor(typeof(NullCommand));
            subject.GetList();
            priorityComparer.Verify(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>()), Times.Never);
        }

        [Test]
        public void sortFunction_is_only_called_once_after_mappings_are_added()
        {
            Mock<IComparer<ICommandMapping>> priorityComparer = new Mock<IComparer<ICommandMapping>>();
            priorityComparer.Setup(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>())).Returns(0);
            subject.WithSortFunction(priorityComparer.Object);

            addPriorityMappings();
            subject.GetList();

            // Reset Times.count to zero
            priorityComparer.Invocations.Clear();

            subject.GetList();
            priorityComparer.Verify(c => c.Compare(It.IsAny<ICommandMapping>(), It.IsAny<ICommandMapping>()), Times.Never);
        }

        [Test]
        public void sortFunction_is_used()
        {
            subject.WithSortFunction(new PriorityMappingComparer());
            PriorityMapping mapping1 = new PriorityMapping(typeof(NullCommand), 1);
            PriorityMapping mapping2 = new PriorityMapping(typeof(NullCommand2), 2);
            PriorityMapping mapping3 = new PriorityMapping(typeof(NullCommand3), 3);
            subject.AddMapping(mapping3);
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            Assert.That(subject.GetList(), Is.EquivalentTo(new List<ICommandMapping>() { mapping1, mapping2, mapping3 }));
        }

        [Test]
        public void trigger_is_activated_when_first_mapping_is_added()
        {
            subject.AddMapping(mapping1);
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void trigger_is_deactivated_when_all_mappings_are_removed()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.AddMapping(mapping3);
            subject.RemoveAllMappings();
            trigger.Verify(t => t.Deactivate(), Times.Once);
        }

        [Test]
        public void trigger_is_deactivated_when_last_mapping_is_removed()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.RemoveMappingFor(mapping1.CommandClass);
            subject.RemoveMappingFor(mapping2.CommandClass);
            trigger.Verify(t => t.Deactivate(), Times.Once);
        }

        [Test]
        public void trigger_is_not_activated_for_second_identical_mapping()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping1);
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void trigger_is_not_activated_when_mapping_overwritten()
        {
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void trigger_is_not_activated_when_second_mapping_is_added()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            trigger.Verify(t => t.Activate(), Times.Once);
        }

        [Test]
        public void trigger_is_not_deactivated_when_list_is_already_empty()
        {
            subject.RemoveAllMappings();
            trigger.Verify(t => t.Deactivate(), Times.Never);
        }

        [Test]
        public void trigger_is_not_deactivated_when_second_last_mapping_is_removed()
        {
            subject.AddMapping(mapping1);
            subject.AddMapping(mapping2);
            subject.RemoveMappingFor(mapping1.CommandClass);
            trigger.Verify(t => t.Deactivate(), Times.Never);
        }

        [Test]
        public void warning_logged_when_mapping_overwritten()
        {
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            subject.AddMapping(new CommandMapping(typeof(NullCommand)));
            logger.Verify(r => r.Warn(It.IsRegex("already mapped"), It.IsAny<object[]>()), Times.Once);
        }

        /*============================================================================*/
        /* Private Functions                                                          */
        /*============================================================================*/

        private void addPriorityMappings()
        {
            subject.AddMapping(new PriorityMapping(typeof(NullCommand), 1));
            subject.AddMapping(new PriorityMapping(typeof(NullCommand2), 2));
            subject.AddMapping(new PriorityMapping(typeof(NullCommand3), 3));
        }

        #endregion Methods
    }
}