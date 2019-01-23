#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class RequestNotSupported : ErrorMessage
	{
		public RequestNotSupported(XmlDocument xmlDoc) : base(xmlDoc)
		{
		}

		public RequestNotSupported(string message) : base(message)
		{
			this.Message = message;
		}
	}
}
