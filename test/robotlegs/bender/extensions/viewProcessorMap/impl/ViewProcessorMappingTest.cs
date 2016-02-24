//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//  Copyright (c) 2012 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System;
using Robotlegs.Bender.Extensions.Matching;
using NUnit.Framework;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public class ViewProcessorMappingTest
	{

		/*============================================================================*/
		/* Private Const Properties                                                   */
		/*============================================================================*/

		private List<Type> EMPTY_CLASS_LIST;

		private ITypeFilter MATCHER;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			EMPTY_CLASS_LIST = new List<Type> ();
			MATCHER = new TypeFilter(EMPTY_CLASS_LIST, EMPTY_CLASS_LIST, EMPTY_CLASS_LIST);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Mapping_Remembers_Matcher()
		{
			ViewProcessorMapping mapping = new ViewProcessorMapping(MATCHER, typeof(ViewInjectionProcessor));
			Assert.That(mapping.Matcher, Is.EqualTo(MATCHER));
		}

		[Test]
		public void Mapping_Handed_Processor_Class_Sets_ProcessorClass_Property()
		{
			Type processorClass = typeof(ViewInjectionProcessor);
			ViewProcessorMapping mapping = new ViewProcessorMapping(MATCHER, processorClass);
			Assert.That(mapping.ProcessorClass, Is.EqualTo(processorClass));
		}
			
		[Test]
		public void Mapping_Handed_Processor_Class_Leaves_Processor_Null()
		{
			Type processorClass = typeof(ViewInjectionProcessor);
			ViewProcessorMapping mapping = new ViewProcessorMapping(MATCHER, processorClass);
			Assert.That(mapping.Processor, Is.Null);
		}

		[Test]
		public void Mapping_Handed_Processor_Object_Sets_ProcessorClass_Property()
		{
			object processor = new ViewInjectionProcessor();
			ViewProcessorMapping mapping = new ViewProcessorMapping(MATCHER, processor);
			Assert.That(mapping.ProcessorClass, Is.EqualTo(typeof(ViewInjectionProcessor)));
		}

		[Test]
		public void Mapping_Handed_Processor_Object_Sets_Processor_Property()
		{
			object processor = new ViewInjectionProcessor();
			ViewProcessorMapping mapping = new ViewProcessorMapping(MATCHER, processor);
			Assert.That(mapping.Processor, Is.EqualTo(processor));
		}
	}
}
