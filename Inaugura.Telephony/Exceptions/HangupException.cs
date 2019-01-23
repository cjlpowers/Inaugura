using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// An exception which occurs when a caller hangs up within the progress of a call
	/// </summary>
	public class HangupException: CallerException
	{
		public HangupException(string message) : base(message)
		{
		}

		public HangupException(string message, Exception innerException) : base(message,innerException)
		{
		}
	}
}
