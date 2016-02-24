//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
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
using Moq;
using Robotlegs.Bender.Framework.API;
using NUnit.Framework;
using Robotlegs.Bender.Extensions.ViewProcessor.Support;
using Robotlegs.Bender.Extensions.ViewProcessor.DSL;

namespace Robotlegs.Bender.Extensions.ViewProcessor.Impl
{
	public class ViewProcessorMapperTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<IViewProcessorViewHandler> handler = new Mock<IViewProcessorViewHandler>();

		public Mock<ILogging> logger = new Mock<ILogging>();

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private ViewProcessorMapper mapper;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void Setup()
		{
			mapper = new ViewProcessorMapper(null, handler.Object, logger.Object);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void ToProcess_Registers_MappingConfig_With_Handler()
		{
			object mapping = mapper.ToProcess(typeof(NullProcessor));
			handler.Verify(viewProcessorViewHandler => viewProcessorViewHandler.AddMapping(It.Is<IViewProcessorMapping>(arg => arg == mapping)), Times.Once);
		}

		[Test]
		public void FromProcess_Removes_MappingConfig_From_Handler()
		{
			object mapping = mapper.ToProcess(typeof(NullProcessor));
			mapper.FromProcess(typeof(NullProcessor));
			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.RemoveMapping(
						It.Is<IViewProcessorMapping>(arg => arg == mapping)
				),Times.Once);
		}

		[Test]
		public void FromProcess_Removes_Only_Specified_MappingConfig_From_Handler()
		{
			object mapping = mapper.ToProcess(typeof(NullProcessor));
			object mapping2 = mapper.ToProcess(typeof(NullProcessor2));
			mapper.FromProcess(typeof(NullProcessor));
			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.RemoveMapping(
					It.Is<IViewProcessorMapping>(arg => arg == mapping)
				),Times.Once);
			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.RemoveMapping(
					It.Is<IViewProcessorMapping>(arg => arg == mapping2)
				),Times.Never);
		}

		[Test]
		public void FromAll_Removes_All_MappingConfigs_From_Handler()
		{
			object mapping = mapper.ToProcess(typeof(NullProcessor));
			object mapping2 = mapper.ToProcess(typeof(NullProcessor2));
			mapper.FromAll();
			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.RemoveMapping(
					It.Is<IViewProcessorMapping>(arg => arg == mapping)
				),Times.Once);
			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.RemoveMapping(
					It.Is<IViewProcessorMapping>(arg => arg == mapping2)
				),Times.Once);
		}

		[Test]
		public void ToProcess_Unregisters_Old_MappingConfig_And_Registers_New_One_When_Overwritten()
		{
			object oldMapping = mapper.ToProcess(typeof(NullProcessor));
			object newMapping = mapper.ToProcess(typeof(NullProcessor));

			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.RemoveMapping(
					It.Is<IViewProcessorMapping>(arg => arg == oldMapping)
				),Times.Once);

			handler.Verify(
				viewProcessorViewHandler => viewProcessorViewHandler.AddMapping(
					It.Is<IViewProcessorMapping>(arg => arg == newMapping)
				),Times.Once);
		}

		[Test]
		public void ToProcess_Warns_When_Overwritten()
		{
			object oldMapping = mapper.ToProcess(typeof(NullProcessor));
			mapper.ToProcess(typeof(NullProcessor));
			
			logger.Verify( _logger => _logger.Warn(It.IsAny<string>(),
				It.Is<object[]>(array => array[0] == null && array[1] == oldMapping)
			), Times.Once);
		}

	}
}