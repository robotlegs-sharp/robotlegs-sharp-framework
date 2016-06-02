using NUnit.Framework;
using Robotlegs.Bender.Framework.API;
using Robotlegs.Bender.Framework.Impl;

namespace Robotlegs.Bender.Bundles.MVCS
{
	[TestFixture]
	public class MVCSBundleTest
	{
        /*============================================================================*/
        /* Tests                                                                      */
        /*============================================================================*/

        [Test]
		public void MVCSBundleInstalls()
		{            
            IContext context = new Context ();
			context.Install<MVCSBundle> ();
			context.Initialize ();
		}
    }
}