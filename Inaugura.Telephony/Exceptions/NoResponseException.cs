using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Represents an error that occures due to a lack of response
	/// </summary>
	public class NoResponseException : InputException
	{
		public NoResponseException(string message) : base(message)
		{
		}
	}
}
