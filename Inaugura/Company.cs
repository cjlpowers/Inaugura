using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Inaugura.Maps;

namespace Inaugura
{
	/// <summary>
	/// A class representing a company
	/// </summary>
	public class Company : Inaugura.Xml.IXmlable, Inaugura.Data.Caching.ICacheable
	{
		#region Variables
		private string mID;
		private Details mDetails;
		private Address mAddress;
		private string mName;
        private bool mFromCache;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// The xml representation of the instance
		/// </summary>
		public System.Xml.XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Company");
				this.PopulateNode(node);
				return node;
			}			
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the Company
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
		/// Gets the ID of the Inaugura Company
		/// </summary>
		/// <value></value>
		public static string InauguraID
		{
			get
			{
				return "INAUGURA";
			}
		}

		/// <summary>
		/// The name of the Company
		/// </summary>
		/// <value></value>
		public string Name
		{
			get
			{
				return this.mName;				
			}
			set
			{
				this.mName = value;
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

		/// <summary>
		/// Additional details specific to this listing
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

        #region Methods
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name"></param>
		public Company(string name) 
		{
			this.ID = Guid.NewGuid().ToString();
			this.Name = name;
			this.Address = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
			this.Details = new Details();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public Company(XmlNode companyNode) : this(string.Empty)
		{
			if (companyNode == null)
				throw new ArgumentNullException("companyNode", "The Xml definition may not be null");

			this.PopulateInstance(companyNode);
		}
		
		/// <summary>
		/// Creates an company instance from an xml representation
		/// </summary>
		/// <param name="node">The xml node representation</param>
		/// <returns>An company instance</returns>
		public static Company FromXml(XmlNode node)
		{
			return Inaugura.Xml.Helper.GetIXmlableFromXml(node) as Company;
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public void PopulateNode(XmlNode node)
		{
			Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());

			Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);
			Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);

			XmlNode addressNode = node.OwnerDocument.CreateElement("Address");
			this.Address.PopulateNode(addressNode);
			node.AppendChild(addressNode);

			XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
			this.Details.PopulateNode(detailsNode);
			node.AppendChild(detailsNode);			
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node"></param>
		public void PopulateInstance(XmlNode node)
		{
			if (node.Attributes["id"] == null)
				throw new ArgumentException("The xml does not contain a id attribute");

			if (node["Address"] == null)
				throw new ArgumentException("The xml does not contain a Address node");

			if (node["Details"] != null)
			{
				this.Details = new Details(node["Details"]);
			}

			this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            if (this.mAddress != null)
                hashCode ^= this.mAddress.GetHashCode();
            if (this.mDetails != null)
                hashCode ^= this.mDetails.GetHashCode();
            if (this.mID != null)
                hashCode ^= this.mID.GetHashCode();
            if (this.mName != null)
                hashCode ^= this.mName.GetHashCode();

            return hashCode;
        }
        #endregion

        #region ICacheable Members

        /// <summary>
        /// The key used to cache this object
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
                this.mFromCache = value;
            }
        }

        #endregion
}
}