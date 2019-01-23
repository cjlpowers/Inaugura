#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Maps
{
    /// <summary>
    /// A class wich represents a Province
    /// </summary>
	public class Province : Inaugura.Xml.IXmlable
	{
		#region Variables
        private string mID;
        private string mCountryID;
        private string mName;
		private Details mDetails;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Province
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Province");
                this.PopulateNode(node);
                return node;
    		}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the Region
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
        /// The ID of the Country which the province is located in
        /// </summary>
        public string CountryID
        {
            get
            {
                return this.mCountryID;
            }
            set
            {
                this.mCountryID = value;
            }
        }

		/// <summary>
		/// The name of the Province
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
		/// Additional details specific to this Province
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

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="provinceNode">The xml representation of the Province</param>
		public Province(XmlNode provinceNode) : this(string.Empty, string.Empty)
		{
            if (provinceNode == null)
                throw new ArgumentNullException("provinceNode", "The Xml definition may not be null");

            this.PopulateInstance(provinceNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
        /// <param name="name">The name of the province</param>
        /// <param name="countryID">The id of the country that the province is located in</param>
        /// <param name="id">The ID of the province</param>
        public Province(string name, string countryID, string id)
		{
            this.ID = id;
            this.CountryID = countryID;
            this.Name = name;
            this.Details = new Details();
		}

        /// <summary>
        /// Constructor
        /// </summary>		
        /// <param name="name">The name of the province</param>
        /// <param name="countryID">The id of the country that the province is located in</param>
        public Province(string name, string countryID) : this(name,countryID,Guid.NewGuid().ToString())
        {            
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {            
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);
            Inaugura.Xml.Helper.SetAttribute(node, "countryId", this.CountryID);
            
            Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);

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
            if (node.Attributes["id"] == null)
                throw new ArgumentException("The xml does not contain a id attribute");

            if (node.Attributes["countryId"] == null)
                throw new ArgumentException("The xml does not contain a countryId attribute");

            if (node.Attributes["name"] == null)
                throw new ArgumentException("The xml does not contain a name attribute");

			this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
            this.CountryID = Inaugura.Xml.Helper.GetAttribute(node, "countryId");			
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
        
            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }
	}
}
