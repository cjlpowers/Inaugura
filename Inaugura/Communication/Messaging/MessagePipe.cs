#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public abstract class MessagePipe : IDisposable
	{
		#region Variables
		private bool mPrivateKeyExchanged = false;
		private string guid = System.Guid.NewGuid().ToString();
		private Inaugura.Security.RijndaelCrypto mRijndaelCrypto;	// The Rijndael crypto object used to encrypt/decrypt data transfered over the pipe
		private MessagingNetwork mNetwork;							// The network the message pipe communicates over
		#endregion

		#region Properties

		protected MessagingNetwork Network
		{
			get
			{
				return this.mNetwork;
			}
			private set
			{
				this.mNetwork = value;
			}
		}

		public Inaugura.Security.RijndaelCrypto SymmetricCrypto
		{
			get
			{
				return this.mRijndaelCrypto;
			}
			private set
			{
				this.mRijndaelCrypto = value;
				this.WriteDebug("Key = " + Convert.ToBase64String(value.Key));
				this.WriteDebug("IV = " + Convert.ToBase64String(value.IV));
			}
		}
		
				
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="network">The messaging network that the pipe communicates over</param>
		protected MessagePipe(MessagingNetwork network) : this(network, null)
		{			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="network">The messaging network that the pipe communicates over</param>
		/// <param name="nagotiateEncryption">True if the pipe will attempt to create a secure connection</param>
		protected MessagePipe(MessagingNetwork network, Inaugura.Security.RijndaelCrypto privateKey)
		{
			this.Network = network;

			if(privateKey != null)
				this.SymmetricCrypto = privateKey;			
		}

		public void WriteDebug(string text)
		{
			System.Diagnostics.Debug.WriteLine("Message Pipe (" + this.guid + ") : " + text);
		}


		protected virtual XmlMessage ProcessRequest(XmlMessage request)
		{
			if (request is PrivateKeyRequest && !this.mPrivateKeyExchanged)
			{
				this.mPrivateKeyExchanged = true;

				PrivateKeyRequest req = request as PrivateKeyRequest;

				byte[] encryptedKey = req.PublicKey.Encrypt(this.SymmetricCrypto.Key, false);
				byte[] encryptedIV = req.PublicKey.Encrypt(this.SymmetricCrypto.IV, false);

				PrivateKeyResponse response = new PrivateKeyResponse(encryptedKey, encryptedIV);
				return response;

			}
			else if (request is EncryptedCompressedMessage)
			{
				if (request is EncryptedCompressedMessage)
				{
					EncryptedCompressedMessage req = request as EncryptedCompressedMessage;
					request = req.DecryptMessage(this.SymmetricCrypto);
				}
				XmlMessage response = this.Network.ProcessRequest(request);
				return new EncryptedCompressedMessage(response, this.SymmetricCrypto);
			}
			else // else use one of the registered message processors
			{
				return new ErrorMessage("Messages must be encrypted");
			}
		}

		public bool EstablishSecurePipe()
		{
			// ask the other party for its public key
			PrivateKeyRequest keyRequest = new PrivateKeyRequest(this.Network.AsymmetricCrypto);
			XmlMessage response = this.SendMessage(keyRequest);
			if (response is PrivateKeyResponse)
			{
				PrivateKeyResponse res = response as PrivateKeyResponse;
				byte[] key = this.Network.AsymmetricCrypto.Decrypt(res.EncryptedKey, false);
				byte[] IV = this.Network.AsymmetricCrypto.Decrypt(res.EncryptedIV, false);
				this.SymmetricCrypto = new Inaugura.Security.RijndaelCrypto(key, IV);
				return true;
			}
			else
				return false;
		}

		public abstract XmlMessage SendMessage(XmlMessage message);




		/*
		/// <summary>
		/// Compresses a message
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		protected XmlDocument Compress(XmlDocument message)
		{
			System.Diagnostics.Debug.Assert(message.FirstChild.Name != "CompressedMessage", "Message is already compressed");

			System.IO.StringWriter textWriter = new System.IO.StringWriter();
			using (XmlTextWriter xw = new XmlTextWriter(textWriter))
			{
				xw.WriteStartDocument();
				xw.Flush();
				xw.WriteStartElement("CompressedXml");
				xw.Flush();
				byte[] compressedData = Helper.CompressString(message.OuterXml);
				xw.WriteBase64(compressedData, 0, compressedData.Length);
				xw.WriteEndElement();
				xw.WriteEndDocument();
				xw.Flush();
				
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(textWriter.ToString());
				return xmlDoc;
			}
		}

		/// <summary>
		/// Uncompresses any compressed messages
		/// </summary>
		/// <param name="message"></param>
		/// <returns>If the method is compressed the method returns the uncompressed message otherwise it returns the origional message</returns>
		protected XmlDocument Uncompress(XmlDocument message)
		{
			System.IO.StringReader textReader = new System.IO.StringReader(message.OuterXml);
			using (XmlTextReader xr = new XmlTextReader(textReader))
			{
				xr.MoveToContent();
				xr.MoveToElement();
				if (xr.Name == "CompressedXml")
				{
					const int bufferSize = 4096;
					byte[] buffer = new byte[bufferSize];

					using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(512))
					{
						int bytesRead = 0;
						do
						{
							bytesRead = xr.ReadBase64(buffer, 0, bufferSize);
							memStream.Write(buffer, 0, bytesRead);
						} while (bytesRead >= bufferSize);

						byte[] compressedData = memStream.ToArray();
						string xml = Helper.UnCompressString(compressedData);
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(xml);
						return xmlDoc;
					}
				}
				return message;
			}		
		}


		protected XmlDocument Encrypt(XmlDocument message, Inaugura.Security.RijndaelCrypto crypto)
		{
			System.Diagnostics.Debug.Assert(message.FirstChild.Name != "EncryptedXml", "Message is already encrypted");

			System.IO.StringWriter textWriter = new System.IO.StringWriter();
			using (XmlTextWriter xw = new XmlTextWriter(textWriter))
			{
				xw.WriteStartDocument();
				xw.Flush();
				xw.WriteStartElement("EncryptedXml");
				xw.Flush();
				byte[] encryptedData = crypto.Encrypt(Helper.StringToBytes(message.OuterXml));
				xw.WriteBase64(encryptedData, 0, encryptedData.Length);
				xw.WriteEndElement();
				xw.WriteEndDocument();
				xw.Flush();

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(textWriter.ToString());
				return xmlDoc;
			}
		}

		/// <summary>
		/// Uncompresses any compressed messages
		/// </summary>
		/// <param name="message"></param>
		/// <returns>If the method is compressed the method returns the uncompressed message otherwise it returns the origional message</returns>
		protected XmlDocument Decrypt(XmlDocument message, Inaugura.Security.RijndaelCrypto crypto)
		{
			System.IO.StringReader textReader = new System.IO.StringReader(message.OuterXml);
			using (XmlTextReader xr = new XmlTextReader(textReader))
			{
				xr.MoveToContent();
				xr.MoveToElement();
				if (xr.Name == "EncryptedXml")
				{
					const int bufferSize = 4096;
					byte[] buffer = new byte[bufferSize];

					using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(512))
					{
						int bytesRead = 0;
						do
						{
							bytesRead = xr.ReadBase64(buffer, 0, bufferSize);
							memStream.Write(buffer, 0, bytesRead);
						} while (bytesRead >= bufferSize);

						byte[] encryptedData = memStream.ToArray();
						string xml = Helper.BytesToString(crypto.Decrypt(encryptedData));
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(xml);
						return xmlDoc;
					}
				}
				return message;
			}
		}
		*/

		#region IDisposable Members

		public virtual void Dispose()
		{			
		}

		#endregion
	}
}
