#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public sealed class EncryptedMessage : XmlMessage
	{
		#region Properties		
		#endregion

		public EncryptedMessage(XmlDocument xmlDoc) : base(xmlDoc)
		{			
		}

		public EncryptedMessage(XmlMessage message, Inaugura.Security.RijndaelCrypto symmetricCrypto)
		{			
			byte[] data = Inaugura.Helper.StringToBytes(message.OuterXml);
			byte[] encryptedData = symmetricCrypto.Encrypt(data);

			XmlElement e = this.CreateElement("EncryptedData");
			e.InnerText = Convert.ToBase64String(encryptedData);
			this["XmlMessage"].AppendChild(e);

			/*
			System.IO.StringWriter textWriter = new System.IO.StringWriter();
			using (XmlTextWriter xw = new XmlTextWriter(textWriter))
			{
				xw.WriteStartDocument();
				xw.Flush();
				xw.WriteStartElement("EncryptedData");
				xw.Flush();				
				xw.WriteBase64(encryptedData, 0, encryptedData.Length);
				xw.WriteEndElement();
				xw.WriteEndDocument();
				xw.Flush();

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(textWriter.ToString());

				XmlNode node = this.ImportNode(xmlDoc["EncryptedData"],true);
				this["XmlMessage"].AppendChild(node);		
			}
			*/
		}

		public XmlMessage DecryptMessage(Inaugura.Security.RijndaelCrypto symmetricCrypto)
		{
			if (this.SelectSingleNode("XmlMessage/EncryptedData") == null)
				return null;
			else
			{
				byte[] encryptedData = Convert.FromBase64String(this["XmlMessage"]["EncryptedData"].InnerText);
				byte[] data = symmetricCrypto.Decrypt(encryptedData);				
				string xml = Inaugura.Helper.BytesToString(data);
				return XmlMessage.FromXml(xml);							
			}
		}
	}
}
