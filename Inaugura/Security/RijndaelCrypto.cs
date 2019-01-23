#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Security.Cryptography;

#endregion

namespace Inaugura.Security
{
	public class RijndaelCrypto
	{
		#region Variables
		private RijndaelManaged mRijndael = new RijndaelManaged();
		#endregion

		#region Properties
		/// <summary>
		/// The Key
		/// </summary>
		/// <value></value>
		public byte[] Key
		{
			get
			{
				return this.mRijndael.Key;
			}
			set
			{
				this.mRijndael.Key = value;
			}
		}

		/// <summary>
		/// The Initialization Vector
		/// </summary>
		/// <value></value>
		public byte[] IV
		{
			get
			{
				return this.mRijndael.IV;
			}
			set
			{
				this.mRijndael.IV = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// Creates a new instance of the Rijndael Encryptor using a random key and initialization vector
		/// </remarks>
		public RijndaelCrypto()
		{
			this.mRijndael.GenerateKey();
			this.mRijndael.GenerateIV();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="key">The symmetric key</param>
		/// <param name="IV">The symetric initialization vector</param>
		public RijndaelCrypto(byte[] key, byte[] IV)
		{
			this.Key = key;
			this.IV = IV;
		}

		public byte[] Encrypt(byte[] data)
		{
			return this.Encrypt(data, this.Key, this.IV);
		}

		public byte[] Encrypt(byte[] data, byte[] key, byte[] IV)
		{
			// Get an encryptor 
			ICryptoTransform transform = this.mRijndael.CreateEncryptor(key,IV);

			MemoryStream memStream = new MemoryStream();
			CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

			// Encrypt the data
			cryptStream.Write(data, 0, data.Length);
			cryptStream.FlushFinalBlock();

			// return the encrypted data
			return memStream.ToArray();
		}

		public byte[] Decrypt(byte[] encryptedData)
		{
			return this.Decrypt(encryptedData, this.Key, this.IV);
		}

		public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] IV)
		{
			// Get an decryptor 
			ICryptoTransform transform = this.mRijndael.CreateDecryptor(key, IV);

			MemoryStream memStream = new MemoryStream(encryptedData);
			CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Read);

			byte[] data = new byte[encryptedData.Length];

			// decrypt the data
			int numberOfBytes = cryptStream.Read(data, 0, data.Length);

			using (MemoryStream mem = new MemoryStream(data,0,numberOfBytes))
			{
				data = mem.ToArray();
			}			
			// return the decrypted data
			return data;
		}

		#endregion
	}
}
