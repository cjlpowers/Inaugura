using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Inaugura.Maps;

namespace Inaugura
{
    /// <summary>
    /// An abstract class which represents a person
    /// </summary>
    public abstract class Person : Xml.IXmlable
    {
        #region Variables
        private string mFirstName;
        private string mLastName;
        private string mPhoneNumber;
        private string mEmail;        
        private Address mAddress;
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Person");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The Customer's First Name
        /// </summary>
        /// <value></value>
        public string FirstName
        {
            get
            {
                return this.mFirstName;
            }
            set
            {
                this.mFirstName = value;
            }
        }

        /// <summary>
        /// The Customer's Last Name
        /// </summary>
        /// <value></value>
        public string LastName
        {
            get
            {
                return this.mLastName;
            }
            set
            {
                this.mLastName = value;
            }
        }

        /// <summary>
        /// The Customer's Phone number
        /// </summary>
        /// <value></value>
        public string PhoneNumber
        {
            get
            {
                return this.mPhoneNumber;
            }
            set
            {
                string str = value.Replace("(","");
                str = str.Replace(")", "");
                str = str.Replace("-", "");
                str = str.Replace(" ", "");
                this.mPhoneNumber =str;
            }
        }

        /// <summary>
        /// The Customer's Email Address
        /// </summary>
        /// <value></value>
        public string Email
        {
            get
            {
                return this.mEmail;
            }
            set
            {
                this.mEmail = value;
            }
        }
        
        /// <summary>
        /// The Customer's Address
        /// </summary>
        /// <value></value>
        public Address Address
        {
            get
            {
                return this.mAddress;
            }
            set
            {
                this.mAddress = value;
            }
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">The Xml Node which defines the object</param>        
        protected Person(XmlNode node) : this()
        {
            this.PopulateInstance(node);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Person()
        {
            this.Address = new Address();
        }
        
        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml node may not be null");

            Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());            
                        
            Inaugura.Xml.Helper.SetAttribute(node, "firstName", this.FirstName);
            Inaugura.Xml.Helper.SetAttribute(node, "lastName", this.LastName);
            Inaugura.Xml.Helper.SetAttribute(node, "email", this.Email);

            if (this.PhoneNumber != null && this.PhoneNumber != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "phoneNumber", this.PhoneNumber);

            XmlNode addressNode = node.OwnerDocument.CreateElement("Address");
            this.Address.PopulateNode(addressNode);
            node.AppendChild(addressNode);       
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public virtual void PopulateInstance(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml definition may not be null");

            Inaugura.Xml.Helper.EnsureAttributeExists(node, "firstName");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "lastName");
            Inaugura.Xml.Helper.EnsureNodeExists(node, "Address");
                    
            this.FirstName = Inaugura.Xml.Helper.GetAttribute(node, "firstName");
            this.LastName = Inaugura.Xml.Helper.GetAttribute(node, "lastName");

            this.Email = Inaugura.Xml.Helper.GetAttribute(node, "email");            

            if (node.Attributes["phoneNumber"] != null)
                this.PhoneNumber = Inaugura.Xml.Helper.GetAttribute(node, "phoneNumber");

            this.Address = new Address(node["Address"]);
        }

        /// <summary>
        /// Gets a hash chode for this object
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            if (this.mAddress != null)
                hashCode ^= this.mAddress.GetHashCode();         
            if (this.mEmail != null)
                hashCode ^= this.mEmail.GetHashCode();
            if (this.mFirstName != null)
                hashCode ^= this.mFirstName.GetHashCode();            
            if (this.mLastName != null)
                hashCode ^= this.mLastName.GetHashCode();
            if (this.mPhoneNumber != null)
                hashCode ^= this.mPhoneNumber.GetHashCode();
            return hashCode;        
        }

        #endregion
    }
}
