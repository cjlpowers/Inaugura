#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class ExceptionMessage : ErrorMessage
	{
		#region Properties 
		

		/// <summary>
		/// The entire exception string representation
		/// </summary>
		/// <value></value>
		public string Exception
		{
			get
			{
				if (this.SelectSingleNode("XmlMessage/Exception") == null)
					return string.Empty;
				else
				{
					return this["XmlMessage"]["Exception"].InnerText;
				}
			}
			set
			{
				if(this.SelectSingleNode("XmlMessage/Exception") == null)
				{
					XmlElement e = this.CreateElement("Exception");
					this.AppendChild(e);
				}
				this["XmlMessage/Exception"].InnerText = value;
			}
		}
		#endregion

		public ExceptionMessage(System.Xml.XmlDocument xmlDoc) : base(xmlDoc)
		{
		}

		public ExceptionMessage(Exception ex) : base(ex.Message)
		{
			this.Exception = ex.ToString();
		}
	}
}
