using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Represents errors which occures during a user input operation
	/// </summary>
	public class InputException: CallerException
	{
		protected InputException(string message)	: base(message)
		{		
		}
	}
}
