using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// 
	/// </summary>
	public class TelephonyException : Exception
	{
		public TelephonyException(string message) : base(message)
		{
		}

		public TelephonyException(string message, Exception innerException) : base(message,innerException)
		{
		}
	}
}
