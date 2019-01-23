using System;
using System.Security.Cryptography;
using System.Text;

namespace Inaugura.Security
{
	/// <summary>
	/// Summary description for License.
	/// </summary>
    public class Helper
    {
        private RSACryptoServiceProvider mRSA;
        private byte[] mSignedHash;

        public RSACryptoServiceProvider RSA
        {
            get
            {
                return this.mRSA;
            }
        }

        public byte[] SignedHash
        {
            get
            {
                return this.mSignedHash;
            }
        }

        public Helper(RSACryptoServiceProvider RSA)
        {
            this.mRSA = RSA;
        }

        public Helper(string rsaXml)
        {
            this.mRSA = new RSACryptoServiceProvider();
            this.mRSA.FromXmlString(rsaXml);
        }

        public byte[] CreateLicense(string serial)
        {
            byte[] h1 = Hash(System.Text.UnicodeEncoding.ASCII.GetBytes(serial));
            byte[] h2 = RSACrypto.SignHash(this.mRSA, h1);

            this.mSignedHash = h2;

            return this.mSignedHash;
        }

        static public byte[] GetHashFromFile(string fileName)
        {
            System.IO.FileStream file = System.IO.File.OpenRead(fileName);
            byte[] h1 = new byte[file.Length];
            file.Read(h1, 0, (int)file.Length);
            file.Close();

            return h1;
        }

        #region Hashing
        /// <summary>
        /// Creates a hash of a string
        /// </summary>
        /// <param name="str">The string to hash</param>
        /// <returns>The hash</returns>
        static public byte[] Hash(byte[] data)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();         
            SHA1 shaM = new SHA1Managed();
            return shaM.ComputeHash(data);
        }

        static public string GetHashString(string str)
        {
            return System.Text.ASCIIEncoding.ASCII.GetString(Hash(System.Text.UnicodeEncoding.ASCII.GetBytes(str)));
        }
        #endregion
    }
}
