using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which holds the listing search criteria
	/// </summary>
    public abstract class PropertySearch : ListingSearch
	{
		#region Variables
        public Inaugura.Maps.Address mAddress;
        private double mRadius;
		#endregion

		#region Properties

        public Inaugura.Maps.Address Address
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
        /// The search radius (in km)
        /// </summary>
        public double Radius
        {
            get
            {
                return this.mRadius;
            }
            set
            {
                this.mRadius = value;
            }
        }
        #endregion
              
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertySearch()
        {
            this.mAddress = new Inaugura.Maps.Address();
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node);               

            if (this.Radius != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "radius", this.Radius.ToString());

            XmlNode addressNode = node.OwnerDocument.CreateElement("Address");
            this.Address.PopulateNode(addressNode);
            node.AppendChild(addressNode);
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

            if (node.Attributes["radius"] != null)
                double.TryParse(Inaugura.Xml.Helper.GetAttribute(node, "radius"), out this.mRadius);

            if (node["Address"] != null)
                this.mAddress = new Inaugura.Maps.Address(node["Address"]);
        }       
        #endregion
    }
}
