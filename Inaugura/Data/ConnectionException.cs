#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

#endregion

namespace Inaugura.Data
{
    /// <summary>
    /// An exception which occurs when a database connection attempt fails
    /// </summary>
    public class ConnectionException : Inaugura.Data.DataException
	{
		public ConnectionException()
		{
		}

		public ConnectionException(string message) : base(message)
		{
		}

		public ConnectionException(string message, Exception innerException) : base(message,innerException)
		{
		}
	}
}
