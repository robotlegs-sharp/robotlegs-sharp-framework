using System;
using System.Threading.Tasks;

namespace robotlegs.bender.framework.impl.safelyCallBackSupport
{
	public class CreateHandler
	{
		public CreateHandler ()
		{
		}

		public static Action Handler(Delegate closure = null, object[] args = null)
		{
			return delegate() {
				if (closure != null)
					closure.DynamicInvoke (args);
			};
		}

//		public static Func<Task> AsyncHandler(Delegate closure = null, object[] args = null)
//		{
//			return Task delegate(object message, Delegate callback) {
//				if (closure != null)
//					closure.DynamicInvoke (args);
//			};
//		}
//		public function createAsyncHandler(closure:Function = null, ... params):Function
//		{
//			return function(message:Object, callback:Function):void {
//				setTimeout(function():void {
//					closure && closure.apply(null, params);
//					callback();
//				}, 5);
//			};
//		}
	}
}

