using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Represents errors which are induced by the caller
	/// </summary>
	public class CallerException: TelephonyException
	{
		protected CallerException(string message): base(message)
		{		
		}

		public CallerException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}
