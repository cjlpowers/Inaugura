#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class ErrorMessage : XmlMessage
	{
		#region Properties
		/// <summary>
		/// The error message
		/// </summary>
		/// <value></value>
		public string Message
		{
			get
			{
				if (this["XmlMessage"].Attributes["message"] == null)
					return string.Empty;
				else
				{
					return this["XmlMessage"].Attributes["message"].Value;
				}
			}
			set
			{
				this["XmlMessage"].SetAttribute("message", value);
			}
		}
		#endregion

		#region Methods
		public ErrorMessage(System.Xml.XmlDocument xmlDoc) : base(xmlDoc)
		{
		}

		public ErrorMessage(string message)
		{
			this.Message = message;
		}
		#endregion
	}
}
