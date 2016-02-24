//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using NUnit.Framework;
using Moq;
using Robotlegs.Bender.Extensions.CommandCenter.API;
using Robotlegs.Bender.Extensions.CommandCenter.DSL;

namespace Robotlegs.Bender.Extensions.CommandCenter.Impl
{
	[TestFixture]
	public class CommandMapperTest
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<ICommandMappingList> mappings;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private CommandMapper subject;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			mappings = new Mock<ICommandMappingList>();
			subject = new CommandMapper(mappings.Object);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void toCommand_creates_ICommandConfigurator()
		{
			Assert.That (subject, Is.Not.Null);
			Assert.That (subject.ToCommand<String>(), Is.InstanceOf<ICommandConfigurator> ());
		}

		[Test]
		public void toCommand_passes_CommandMapping_to_MappingList()
		{
			subject.ToCommand(typeof(String));
			mappings.Verify (m => m.AddMapping (It.IsAny<ICommandMapping> ()), Times.Once);
		}

		[Test]
		public void fromCommand_delegates_to_MappingList()
		{
			Type type = typeof(String);
			subject.FromCommand(type);
			mappings.Verify (m => m.RemoveMappingFor (It.Is<Type>(arg => type == arg )), Times.Once);
		}

		[Test]
		public void fromAll_delegates_to_MappingList()
		{
			subject.FromAll ();
			mappings.Verify (m => m.RemoveAllMappings(), Times.Once);
		}
	}
}

