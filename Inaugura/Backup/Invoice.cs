using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    public class Invoice : Inaugura.Xml.IXmlable
    {
        #region Variables
        private Guid mID;
        private Inaugura.Details mDetails;        
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Listing
        /// </summary>
        /// <value></value>
        public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Invoice");
                this.PopulateNode(node);
                return node;
            }
        }	
        #endregion

        #region Properties
        /// <summary>
        /// The ID of the invoice
        /// </summary>
        public Guid ID
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
        /// The invoice details
        /// </summary>
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
		/// <param name="xml">The xml representation of the invoice</param>
		public Invoice(XmlNode invoiceNode) : this()
		{
            this.PopulateInstance(invoiceNode);
		}

        /// <summary>
		/// Constructor
		/// </summary>		
        public Invoice() 
		{			
			this.ID = Guid.NewGuid();		
			this.Details = new Details();
		}

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());

            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
               
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
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            if (node.Attributes["id"] == null)
                throw new ArgumentException("The xml does not contain a id attribute");
                      
            this.ID = new Guid( Inaugura.Xml.Helper.GetAttribute(node, "id"));

            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }

        /// <summary>
        /// Creates a Invoice instance from an xml node representation
        /// </summary>
        /// <param name="xml">The xml which represents the item</param>
        /// <returns>A Invoice instance</returns>
        public static Invoice FromXml(XmlNode xml)
        {
            return Inaugura.Xml.Helper.GetIXmlableFromXml(xml) as Invoice;
        }
        #endregion

        
}
}
