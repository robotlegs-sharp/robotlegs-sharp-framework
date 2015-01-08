using System;
using NUnit.Framework;
using robotlegs.bender.framework.api;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class LifecycleTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

//		private var target:Object;

		private Lifecycle lifecycle;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
//			target = {};
			lifecycle = new Lifecycle();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void lifecycle_starts_uninitialized()
		{
			Assert.AreEqual (LifecycleState.UNINITIALIZED, lifecycle.state);
			Assert.True (lifecycle.Uninitialized);
		}

		[Test]
		public void initialize_turns_state_active()
		{
			lifecycle.Initialize();
			Assert.AreEqual (LifecycleState.ACTIVE, lifecycle.state);
			Assert.True (lifecycle.Active);
		}

		[Test]
		public void TestAsync()
		{
			Assert.True (true);
//			Task task = new Task (Function1);
//			task.ContinueWith (Function2);
//			task.ContinueWith (Function3);

			Task t = Iterate (RunFunctions ());
		}

		public static Task Iterate(IEnumerable<Task> asyncIterator)
		{
			if (asyncIterator == null) throw new ArgumentNullException("asyncIterator");

			var enumerator = asyncIterator.GetEnumerator();
			if (enumerator == null) throw new InvalidOperationException("Invalid enumerable - GetEnumerator returned null");

			var tcs = new TaskCompletionSource<object>();
			tcs.Task.ContinueWith(_ => enumerator.Dispose(), TaskContinuationOptions.ExecuteSynchronously);

			Action<Task> recursiveBody = null;
			recursiveBody = delegate {
				try {
					if (enumerator.MoveNext()) enumerator.Current.ContinueWith(recursiveBody, TaskContinuationOptions.ExecuteSynchronously);
					else tcs.TrySetResult(null);
				}
				catch (Exception exc) { tcs.TrySetException(exc); }
			};

			recursiveBody(null);
			return tcs.Task;
		}

		private IEnumerable<Task> RunFunctions()
		{
			string input = "Input";
//			Task<string> resultA = DoAAsync(input);
//			yield return resultA;
//			Task<string> resultB = DoBAsync(resultA.Result);
//			yield return resultB;
//			Task<string> resultC = DoCAsync(resultB.Result);
//			yield return resultC;
			yield return DoAAsync ();
			yield return DoBAsync ();
			yield return DoCAsync ();
		}

		public async Task DoAAsync()
		{
			Console.WriteLine ("DoAAsync Enter");
			await Task.Delay (10);
			Console.WriteLine ("DoAAsync Exit");
		}

		public async Task DoBAsync()
		{
			Console.WriteLine ("DoBAsync Enter");
			await Task.Delay (10);
			Console.WriteLine ("DoBAsync Exit");
		}

		public async Task DoCAsync()
		{
			Console.WriteLine ("DoCAsync Enter");
			await Task.Delay (10);
			Console.WriteLine ("DoCAsync Exit");
		}

//		public async void Function1()
//		{
//			Console.WriteLine ("Function1");
//		}
//
//		public async void Function2()
//		{
//			Console.WriteLine ("Fucntion2");
//		}
//
//		public async void Function3()
//		{
//			Console.WriteLine ("Fucntion3");
//		}
	}
}

