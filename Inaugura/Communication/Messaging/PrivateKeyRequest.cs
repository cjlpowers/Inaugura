#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class PrivateKeyRequest : XmlMessage
	{
		#region Properties
		/// <summary>
		/// The key used to encrypt the respose
		/// </summary>
		/// <value></value>
		public Inaugura.Security.RSACrypto PublicKey
		{
			get
			{
				if (this.SelectSingleNode("XmlMessage/PublicKey") == null)
					return null;
				else
				{
					string xml = this["XmlMessage"]["PublicKey"].InnerXml;
					Inaugura.Security.RSACrypto rsa = new Inaugura.Security.RSACrypto(xml);
					return rsa;
				}
			}
			set
			{
				if (this.SelectSingleNode("XmlMessage/PublicKey") == null)
				{
					XmlElement e = this.CreateElement("PublicKey");
					this["XmlMessage"].AppendChild(e);
				}
				string xml = value.ToXmlString(false);
				this["XmlMessage"]["PublicKey"].InnerXml = xml;
			}
		}
		#endregion

		#region Methods
		public PrivateKeyRequest(Inaugura.Security.RSACrypto publicKey)
		{
			this.PublicKey = publicKey;
		}

		public PrivateKeyRequest(XmlDocument xmlDoc) : base(xmlDoc)
		{
		}
		#endregion
	}
}
