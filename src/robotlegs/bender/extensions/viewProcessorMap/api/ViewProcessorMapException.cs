using System;

namespace robotlegs.bender.extensions.viewProcessorMap.api
{
	// TODO: Matt to James: Should this be renames ViewProcessorMapException??
	public class ViewProcessorMapException : Exception
	{
		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/// <summary>
		/// Creates a View Processor Map Error
		/// </summary>
		/// <param name="message">The error message</param>
		public ViewProcessorMapException(string message) : base(message)
		{

		}
	}
}

