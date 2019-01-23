using System;

namespace Inaugura.Telephony
{
	/// <summary>
	/// Represents an error that occures because a user enters invalid input
	/// </summary>
	public class InvalidInputException : InputException
	{
		private string mUserInput;

		public InvalidInputException(string userInput, string message): base(message)
		{
			this.mUserInput = userInput;	
		}

		public string Input
		{
			get
			{
				return mUserInput;
			}
		}
	}
}
