#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class PrivateKeyResponse : XmlMessage
	{
		#region Properties
		public byte[] EncryptedKey
		{
			get
			{
				if (this.SelectSingleNode("XmlMessage/EncryptedKey") == null)
					throw new InvalidDataException("The message does not contain key information");
				else
				{
					string encryptedKey = this["XmlMessage"]["EncryptedKey"].InnerText;
					return Convert.FromBase64String(encryptedKey);
				}
			}
			protected set
			{
				if (this.SelectSingleNode("XmlMessage/EncryptedKey") == null)
				{
					XmlElement e = this.CreateElement("EncryptedKey");
					this["XmlMessage"].AppendChild(e);
				}
				this["XmlMessage"]["EncryptedKey"].InnerText = Convert.ToBase64String(value);
			}
		}

		public byte[] EncryptedIV
		{
			get
			{
				if (this.SelectSingleNode("XmlMessage/EncryptedIV") == null)
					throw new InvalidDataException("The message does not contain IV information");
				else
				{
					string encryptedKey = this["XmlMessage"]["EncryptedIV"].InnerText;
					return Convert.FromBase64String(encryptedKey);
				}
			}
			protected set
			{
				if (this.SelectSingleNode("XmlMessage/EncryptedIV") == null)
				{
					XmlElement e = this.CreateElement("EncryptedIV");
					this["XmlMessage"].AppendChild(e);
				}
				this["XmlMessage"]["EncryptedIV"].InnerText = Convert.ToBase64String(value);
			}
		}
		#endregion

		public PrivateKeyResponse(XmlDocument xmlDoc) : base(xmlDoc)
		{
		}

		public PrivateKeyResponse(byte[] encryptedKey, byte[] encryptedIV)
		{
			this.EncryptedKey = encryptedKey;
			this.EncryptedIV = encryptedIV;
		}
	}
}
