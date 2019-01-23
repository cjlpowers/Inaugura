#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public sealed class CompressedMessage : XmlMessage
	{
		#region Properties
		public XmlMessage Message
		{
			get
			{
				if (this.SelectSingleNode("XmlMessage/CompressedData") == null)
					return null;
				else
				{
					string base64String = this["XmlMessage"]["CompressedData"].InnerText;
					byte[] compressedData = Convert.FromBase64String(base64String);
					string xml = Helper.UnCompressString(compressedData);
					return XmlMessage.FromXml(xml);
				}
			}
			private set
			{
				if (this.SelectSingleNode("XmlMessage/CompressedData") == null)
				{
					XmlElement e = this.CreateElement("CompressedData");
					this["XmlMessage"].AppendChild(e);
				}
				byte[] compressedData = Helper.CompressString(value.OuterXml);
				string base64String = Convert.ToBase64String(compressedData, Base64FormattingOptions.None);
				this["XmlMessage"]["CompressedData"].InnerText = base64String;
			}
		}
		#endregion
		public CompressedMessage(XmlDocument xmlDoc) : base(xmlDoc)
		{			
		}

		public CompressedMessage(XmlMessage message)
		{
			this.Message = message;
		}		
	}
}
