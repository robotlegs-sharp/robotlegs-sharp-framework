using System;
using NUnit.Framework;
using robotlegs.bender.extensions.viewManager.api;
using robotlegs.bender.extensions.viewManager.support;

namespace robotlegs.bender.extensions.viewManager.impl
{
	[TestFixture]
	public class ViewNotifierTest
	{
		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[TearDown]
		public void After()
		{
			ViewNotifier.SetRegistry (null);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void Test_Set_Registry()
		{
			ContainerRegistry cr = new ContainerRegistry ();
			ViewNotifier.SetRegistry (cr);
			Assert.That (ViewNotifier.Registry, Is.EqualTo(cr));
		}

		[Test]
		public void Test_Nullable_Registry()
		{
			ViewNotifier.SetRegistry (null);
			Assert.That (ViewNotifier.Registry, Is.Null);
		}
		
		[Test]
		public void Test_Register_When_Null_No_Exception()
		{
			ViewNotifier.SetRegistry (null);
			ViewNotifier.RegisterView (new object (), typeof(object));
		}

		[Test]
		public void Test_Register_View_Calls_View_Handler()
		{
			object expectedView = null;
			Type expectedType = null;
			object actualView = new object ();
			Type actualType = actualView.GetType ();

			ContainerRegistry cr = new ContainerRegistry ();
			ViewNotifier.SetRegistry (cr);
			object container = new object ();
			cr.SetFallbackContainer (container);
			cr.GetBinding (container).AddHandler (new CallbackViewHandler (delegate(object view, Type type)
			{
				expectedType = type;
				expectedView = view;
			}));
			ViewNotifier.RegisterView (actualView, actualType);

			Assert.That (actualView, Is.EqualTo (expectedView));
			Assert.That (actualType, Is.EqualTo (expectedType));
		}
	}
}

