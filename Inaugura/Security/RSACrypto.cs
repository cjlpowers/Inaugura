#region Using directives

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Inaugura.Security
{
	/// <summary>
	/// RSA Wrapper. Performs all asymetric RSA encription tasks.
	/// </summary>
	public class RSACrypto
	{
 		#region Variables
		private RSACryptoServiceProvider mRSA;
		#endregion

		#region Properties
		public RSAParameters PublicKey
		{
			get
			{
				return this.mRSA.ExportParameters(false);
			}
		}

		public RSAParameters PrivateKey
		{
			get
			{
				return this.mRSA.ExportParameters(true);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// Creates an instance of RSA and generates random public and private keys
		/// </remarks>
		public RSACrypto()
		{
			this.mRSA = new RSACryptoServiceProvider();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="rsaXml">The RSA xml</param>
		/// <remarks>Uses the xml to initialize the private and public keys</remarks>
		public RSACrypto(string rsaXml)
		{
			this.mRSA = RSACrypto.GetRSAFromXml(rsaXml);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parameters">The RSA key information</param>
		public RSACrypto(RSAParameters parameters)
		{
			this.mRSA = new RSACryptoServiceProvider();
			this.mRSA.ImportParameters(parameters);
		}

		public string ToXmlString(bool includePrivateKey)
		{
			return this.mRSA.ToXmlString(includePrivateKey);
		}

		/// <summary>
		/// Encrypts data using RSA
		/// </summary>
		/// <param name="data">The data to encrypt</param>
		/// <param name="doOAEPPadding">Encrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.</param>
		/// <returns>The encrypted data</returns>
		public byte[] Encrypt(byte[] data, bool doOAEPPadding)
		{
			return RSACrypto.Encrypt(data, this.mRSA, doOAEPPadding);
		}

		/// <summary>
		/// Encrypts data using RSA encryption
		/// </summary>
		/// <param name="data">The data to encrypt</param>
		/// <param name="RSA">The RSA instance (needs only the public key information)</param>
		/// <param name="doOAEPPadding">Encrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.</param>
		/// <returns>The encrypted data</returns>
		static public byte[] Encrypt(byte[] data, RSACryptoServiceProvider RSA, bool doOAEPPadding)
		{			
			//Encrypt the passed byte array and specify OAEP padding.  
			//OAEP padding is only available on Microsoft Windows XP or
			//later.  
			return RSA.Encrypt(data, doOAEPPadding);
		}

		/// <summary>
		/// Encrypts data using RSA encryption
		/// </summary>
		/// <param name="data">The data to encrypt</param>
		/// <param name="publicKeyInfo">The public key information</param>
		/// <param name="doOAEPPadding">Encrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.</param>
		/// <returns>The encrypted data</returns>
		static public byte[] Encrypt(byte[] data, RSAParameters publicKeyInfo, bool doOAEPPadding)
		{
			//Create a new instance of RSACryptoServiceProvider.
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

			//Import the RSA Key information. This only needs
			//toinclude the public key information.
			RSA.ImportParameters(publicKeyInfo);

			//Encrypt the passed byte array and specify OAEP padding.  
			//OAEP padding is only available on Microsoft Windows XP or
			//later.  
			return RSA.Encrypt(data, doOAEPPadding);
		}

		/// <summary>
		/// Decrypts data using RSA
		/// </summary>
		/// <param name="data">The data to decrypt</param>
		/// <param name="doOAEPPadding">Decrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.</param>
		/// <returns>The decrypted data</returns>
		public byte[] Decrypt(byte[] encryptedData, bool doOAEPPadding)
		{
			return RSACrypto.Decrypt(encryptedData, this.mRSA, doOAEPPadding);
		}

		/// <summary>
		/// Decrypts data using RSA encryption
		/// </summary>
		/// <param name="data">The data to decrypt</param>
		/// <param name="RSA">The RSA instance (needs  the private key information)</param>
		/// <param name="doOAEPPadding">Decrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.</param>
		/// <returns>The decrypted data</returns>
		static public byte[] Decrypt(byte[] encryptedData, RSACryptoServiceProvider RSA, bool doOAEPPadding)
		{
			//Decrypt the passed byte array and specify OAEP padding.  
			//OAEP padding is only available on Microsoft Windows XP or
			//later.  
			return RSA.Decrypt(encryptedData, doOAEPPadding);
		}

		/// <summary>
		/// Decrypts data using RSA encryption
		/// </summary>
		/// <param name="data">The data to decrypt</param>
		/// <param name="privateKeyInfo">The private key information</param>
		/// <param name="doOAEPPadding">Decrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.</param>
		/// <returns>The decrypted data</returns>
		static public byte[] Decrypt(byte[] encryptedData, RSAParameters privateKeyInfo, bool doOAEPPadding)
		{
			//Create a new instance of RSACryptoServiceProvider.
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

			//Import the RSA Key information. This needs
			//to include the private key information.
			RSA.ImportParameters(privateKeyInfo);

			//Decrypt the passed byte array and specify OAEP padding.  
			//OAEP padding is only available on Microsoft Windows XP or
			//later.  
			return RSA.Encrypt(encryptedData, doOAEPPadding);
		}

		static public byte[] SignHash(RSACryptoServiceProvider RSA, byte[] hash)
		{
			//Create an RSAOPKCS1SignatureFormatter object and pass it the 
			//RSACryptoServiceProvider to transfer the key information.
			RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);

			//Set the hash algorithm to SHA1.
			RSAFormatter.SetHashAlgorithm("SHA1");

			//Create a signature for HashValue and return it.
			byte[] SignedHash = RSAFormatter.CreateSignature(hash);
			return SignedHash;
		}

		static public bool VerifySignedHash(RSACryptoServiceProvider RSA, byte[] hash, byte[] signedHash)
		{
			RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);

			RSADeformatter.SetHashAlgorithm("SHA1");

			//Verify the hash and display the results to the console.
			return RSADeformatter.VerifySignature(hash, signedHash);
		}

		static public RSACryptoServiceProvider GetRSAFromXml(string xml)
		{
			CspParameters csp = new CspParameters();
			csp.Flags = CspProviderFlags.UseMachineKeyStore;
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(csp);

			RSA.FromXmlString(xml);

			return RSA;
		}
		#endregion
	}
}
