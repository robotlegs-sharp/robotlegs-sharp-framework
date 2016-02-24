//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using Robotlegs.Bender.Extensions.EventManagement.API;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.EventCommand.Support;
using System;
using System.Collections.Generic;


namespace Robotlegs.Bender.Extensions.EventManagement.Impl
{

	public class EventRelayTest
	{

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IEventDispatcher source;

		private IEventDispatcher destination;

		private EventRelay subject;

		private List<Enum> reportedTypes;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			source = new EventDispatcher();
			destination = new EventDispatcher();
			reportedTypes = new List<Enum>();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void No_Relay_Before_Start()
		{
			CreateRelayFor(SupportEvent.Type.TYPE1);
			source.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That (reportedTypes, Is.Empty);
		}

		[Test]
		public void Relays_Specified_Events()
		{
			CreateRelayFor(SupportEvent.Type.TYPE1, SupportEvent.Type.TYPE2).Start();
			source.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			source.Dispatch(new SupportEvent(SupportEvent.Type.TYPE2));

			Assert.That(reportedTypes, Is.EquivalentTo(new List<Enum>() { SupportEvent.Type.TYPE1, SupportEvent.Type.TYPE2 }));
		}

		[Test]
		public void Ignores_Unspecified_Events()
		{
			CreateRelayFor().Start();
			source.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			Assert.That (reportedTypes, Is.Empty);
		}

		[Test]
		public void Relays_Specified_Events_But_Ignores_Unspecified_Events()
		{
			CreateRelayFor(SupportEvent.Type.TYPE1).Start();
			source.Dispatch(new SupportEvent(SupportEvent.Type.TYPE1));
			source.Dispatch(new SupportEvent(SupportEvent.Type.TYPE2));
			Assert.That (reportedTypes, Is.EquivalentTo (new List<Enum>() { SupportEvent.Type.TYPE1 } ));
		}

		[Test]
		public void No_Relay_After_Stop()
		{
			CreateRelayFor (SupportEvent.Type.TYPE1).Start ().Stop ();
			source.Dispatch (new SupportEvent (SupportEvent.Type.TYPE1));
			Assert.That (reportedTypes, Is.Empty);
		}

		[Test]
		public void Relay_Resumes()
		{
			CreateRelayFor (SupportEvent.Type.TYPE1).Start ().Stop ().Start ();
			source.Dispatch (new SupportEvent (SupportEvent.Type.TYPE1));
			Assert.That (reportedTypes, Is.EquivalentTo (new List<Enum>() { SupportEvent.Type.TYPE1 } ));
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private EventRelay CreateRelayFor(params Enum[] types)
		{
			subject = new EventRelay(source, destination, new List<Enum>(types));
			foreach (Enum type in types)
			{
				destination.AddEventListener(type, CatchEvent);
			}
			return subject;
		}

		private void CatchEvent(IEvent evt)
		{
			reportedTypes.Add(evt.type);
		}
	}
}
