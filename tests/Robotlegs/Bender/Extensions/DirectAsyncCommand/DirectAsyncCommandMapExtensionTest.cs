//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved.
//
//  NOTICE: You are permitted to use, modify, and distribute this file
//  in accordance with the terms of the license agreement accompanying it.
//------------------------------------------------------------------------------

using NUnit.Framework;
using Robotlegs.Bender.Extensions.DirectAsyncCommand.API;
using Robotlegs.Bender.Framework.Impl;

namespace Robotlegs.Bender.Extensions.DirectAsyncCommand
{
    [TestFixture]
    public class DirectAsyncCommandMapExtensionTest
    {
        /*============================================================================*/
        /* Private Properties                                                         */
        /*============================================================================*/

        #region Fields

        private Context context;

        #endregion Fields

        /*============================================================================*/
        /* Test Setup and Teardown                                                    */
        /*============================================================================*/

        #region Methods

        [SetUp]
        public void before()
        {
            context = new Context();
            context.Install<DirectAsyncCommandMapExtension>();
        }

        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
        public void directCommandMap_is_mapped_into_injector()
        {
            object actual = null;
            context.WhenInitializing(delegate ()
            {
                actual = context.injector.GetInstance<IDirectAsyncCommandMap>();
            });
            context.Initialize();
            Assert.That(actual, Is.InstanceOf<IDirectAsyncCommandMap>());
        }

        #endregion Methods
    }
}