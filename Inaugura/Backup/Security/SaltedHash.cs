using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;

namespace Inaugura.Security
{
    /// <summary>
    /// A class representing a salted hash
    /// </summary>
    public class SaltedHash: Xml.IXmlable
    {
        #region Variables
        private byte[] mSalt;
        private byte[] mHash;
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Address
        /// </summary>
        /// <value></value>
        public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("SaltedHash");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion

        #region Methods
        public SaltedHash(byte[] data)
        {
            // create some salt
            this.mSalt = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(this.mSalt);

            this.mHash = ComputeSaltedHash(data);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">The data</param>
        public SaltedHash(string data) : this(System.Text.UnicodeEncoding.ASCII.GetBytes(data))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The xml node</param>
        public SaltedHash(XmlNode node)
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml node may not be null");

            Inaugura.Xml.Helper.SetAttribute(node, "salt", Convert.ToBase64String(this.mSalt));
            node.InnerText = Convert.ToBase64String(this.mHash);            
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public void PopulateInstance(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml definition may not be null");

            if (node.Attributes["salt"] == null)
                throw new ArgumentException("The xml does not contain a salt attribute");

            this.mSalt = Convert.FromBase64String(node.Attributes["salt"].Value);
            this.mHash = Convert.FromBase64String(node.InnerText);
        }        

        /// <summary>
        /// Determins source of the salted hash matches specified data
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>True if there is a match, false otherwise</returns>
        public bool IsMatch(byte[] data)
        {
            return this.Equals(data);
        }

        /// <summary>
        /// Determins source of the salted hash matches specified data
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>True if there is a match, false otherwise</returns>
        public bool IsMatch(string data)
        {
            return this.Equals(data);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
                return false;

            // If parameter cannot be cast to Point return false.
            if (obj is byte[])
            {
                byte[] testData = obj as byte[];
                byte[] testHash = ComputeSaltedHash(testData);
                return (Convert.ToBase64String(this.mHash).Equals(Convert.ToBase64String(testHash)));
            }
            else if (obj is string)
                return Equals(System.Text.UnicodeEncoding.ASCII.GetBytes(obj.ToString()));
            else
                return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode =  this.mHash.GetHashCode();
            hashCode ^= this.mSalt.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Computes a salted hash
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns></returns>
        private byte[] ComputeSaltedHash(byte[] data)
        {
            // add the salt to the end of the data
            List<byte> saltedData = new List<byte>(data);
            saltedData.AddRange(this.mSalt);

            return Helper.Hash(saltedData.ToArray());           
        }
        #endregion
    }
}
