#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Maps
{
    /// <summary>
    /// A class wich represents a Country
    /// </summary>
	public class Country : Inaugura.Xml.IXmlable
	{
		#region Variables
        private string mID;
        private string mName;
		private Details mDetails;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Country
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Country");
                this.PopulateNode(node);
                return node;
    		}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the Country
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
		/// The name of the Country
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
		/// Additional details specific to this Country
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
        /// <param name="regionNode">The xml representation of the Country</param>
		public Country(XmlNode countryNode) : this(string.Empty)
		{
            if (countryNode == null)
                throw new ArgumentNullException("countryNode", "The Xml definition may not be null");

            this.PopulateInstance(countryNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
        /// <param name="name">The country name</param>
        /// <param name="id">The country ID</param>
        public Country(string name, string id)
		{
            this.ID = id;
            this.Name = name;
            this.Details = new Details();
		}

        /// <summary>
        /// Constructor
        /// </summary>		
        /// <param name="name">The country name</param>
        public Country(string name) : this(name, Guid.NewGuid().ToString())
        {            
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);            
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
                      
            if (node.Attributes["name"] == null)
                throw new ArgumentException("The xml does not contain a name attribute");

			this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
        
            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }
	}
}
