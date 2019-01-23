using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Inaugura.Maps;


namespace Inaugura
{
    public class Customer :  Inaugura.Xml.IXmlable, Inaugura.Data.Caching.ICacheable
    {
        #region Variables
        private string mID;
        private string mCompanyID;
        private string mFirstName;
        private string mLastName;
        private string mCustomerNumber;
        private string mPhoneNumber;
        private string mEmail;
        private string mPassword;
        private Address mAddress;
        private Details mDetails;
        private bool mFromCache;
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Customer");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion

        #region IPerson Members
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
                this.mPhoneNumber = value;
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
        /// The Customer's Password
        /// </summary>
        /// <value></value>
        public string Password
        {
            get
            {
                return this.mPassword;
            }
            set
            {
                this.mPassword = value;
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

        #region Properties
        /// <summary>
        /// The GUID for this customer
        /// </summary>
        /// <value></value>
        public string ID
        {
            get
            {
                return this.mID;
            }
            private set
            {
                this.mID = value;
            }
        }

        /// <summary>
        /// The ID of the Company which this customer is a member
        /// </summary>
        /// <value></value>
        public string CompanyID
        {
            get
            {
                return this.mCompanyID;
            }
            set
            {
                this.mCompanyID = value;
            }
        }

        /// <summary>
        /// The Customer Number
        /// </summary>
        /// <value></value>
        public string CustomerNumber
        {
            get
            {
                return this.mCustomerNumber;
            }
            set
            {
                this.mCustomerNumber = value;
            }
        }

        /// <summary>
        /// Additional Details specific to this Customer
        /// </summary>
        /// <value></value>
        public Details Details
        {
            get
            {
                return this.mDetails;
            }
            private set
            {
                this.mDetails = value;
            }
        }
        #endregion

        #region ICacheable Members
        /// <summary>
        /// The key used to cache this item
        /// </summary>
        public string CacheKey
        {
            get
            {
                return this.ID;
            }
        }

        /// <summary>
        /// Determins if the object came from cache
        /// </summary>
        public bool FromCache
        {
            get
            {
                return this.mFromCache;
            }
            set
            {
                this.mFromCache = true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerNode">The Xml Node which defines the Customer</param>        
        public Customer(XmlNode customerNode)
            : this()
        {
            if (customerNode == null)
                throw new ArgumentNullException("customerNode", "The Xml definition may not be null");

            this.PopulateInstance(customerNode);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Customer()
        {
            this.ID = Guid.NewGuid().ToString();
            this.Details = new Details();
            this.Address = new Address();
        }

        /// <summary>
        /// Creates an Customer instance from an xml representation
        /// </summary>
        /// <param name="node">The xml document which represents the Customer</param>
        /// <returns>An Customer instance</returns>
        public static Customer FromXml(XmlNode node)
        {
            return Inaugura.Xml.Helper.GetIXmlableFromXml(node) as Customer;
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml node may not be null");

            Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());            

            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);
            Inaugura.Xml.Helper.SetAttribute(node, "companyId", this.CompanyID);

            if (this.CustomerNumber != null && this.CustomerNumber != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "customerNumber", this.CustomerNumber);

            Inaugura.Xml.Helper.SetAttribute(node, "email", this.Email);
            Inaugura.Xml.Helper.SetAttribute(node, "firstName", this.FirstName);
            Inaugura.Xml.Helper.SetAttribute(node, "lastName", this.LastName);
            Inaugura.Xml.Helper.SetAttribute(node, "password", this.Password);

            if (this.PhoneNumber != null && this.PhoneNumber != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "phoneNumber", this.PhoneNumber);

            XmlNode addressNode = node.OwnerDocument.CreateElement("Address");
            this.Address.PopulateNode(addressNode);
            node.AppendChild(addressNode);

            if (this.Details.Count > 0)
            {
                XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
                this.Details.PopulateNode(detailsNode);
                node.AppendChild(detailsNode);
            }
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public void PopulateInstance(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node", "The Xml definition may not be null");

            if (node.Attributes["id"] == null)
                throw new ArgumentException("The xml does not contain a street attribute");

            if (node.Attributes["companyId"] == null)
                throw new ArgumentException("The xml does not contain a city attribute");

            if (node.Attributes["firstName"] == null)
                throw new ArgumentException("The xml does not contain a country attribute");

            if (node.Attributes["lastName"] == null)
                throw new ArgumentException("The xml does not contain a zipPostal attribute");

            if (node.Attributes["email"] == null)
                throw new ArgumentException("The xml does not contain a email attribute");

            if (node.Attributes["password"] == null)
                throw new ArgumentException("The xml does not contain a stateProv attribute");

            if (node["Address"] == null)
                throw new ArgumentException("The xml does not contain a Address node");

            this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
            this.CompanyID = Inaugura.Xml.Helper.GetAttribute(node, "companyId");
            this.FirstName = Inaugura.Xml.Helper.GetAttribute(node, "firstName");
            this.LastName = Inaugura.Xml.Helper.GetAttribute(node, "lastName");
            this.Email = Inaugura.Xml.Helper.GetAttribute(node, "email");
            this.Password = Inaugura.Xml.Helper.GetAttribute(node, "password");

            if (node.Attributes["customerNumber"] != null)
                this.PhoneNumber = Inaugura.Xml.Helper.GetAttribute(node, "phoneNumber");

            if (node.Attributes["phoneNumber"] != null)
                this.PhoneNumber = Inaugura.Xml.Helper.GetAttribute(node, "phoneNumber");

            this.Address = new Address(node["Address"]);

            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            if (this.mAddress != null)
                hashCode ^= this.mAddress.GetHashCode();
            if (this.mCompanyID != null)
                hashCode ^= this.mCompanyID.GetHashCode();
            if (this.mCustomerNumber != null)
                hashCode ^= this.mCustomerNumber.GetHashCode();
            if (this.mDetails != null)
                hashCode ^= this.mDetails.GetHashCode();
            if (this.mEmail != null)
                hashCode ^= this.mEmail.GetHashCode();
            if (this.mFirstName != null)
                hashCode ^= this.mFirstName.GetHashCode();
            if (this.mID != null)
                hashCode ^= this.mID.GetHashCode();
            if (this.mLastName != null)
                hashCode ^= this.mLastName.GetHashCode();
            if (this.mPassword != null)
                hashCode ^= this.mPassword.GetHashCode();
            if (this.mPhoneNumber != null)
                hashCode ^= this.mPhoneNumber.GetHashCode();

            return hashCode;        
        }

        #endregion
    }
}
