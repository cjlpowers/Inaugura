using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which holds the search criteria
	/// </summary>
    public class RealEstateSearch : PropertySearch
	{
		#region Variables
       	private float mPriceUpper;
		private float mPriceLower;

        private int mMinBathrooms;
        private int mMinBedrooms;
        
		private PropertyType mPropertyType;
		#endregion

		#region Properties
      	/// <summary>
		/// The upper price limit (0 if not specified)
		/// </summary>
		public float PriceUpper
		{
			get
			{
				return this.mPriceUpper;
			}
			set
			{
				this.mPriceUpper = value;
			}
		}

		/// <summary>
		/// The lower price limit (0 if not specified)
		/// </summary>
		public float PriceLower
		{
			get
			{
				return this.mPriceLower;
			}
			set
			{
				this.mPriceLower = value;
			}
		}

		/// <summary>
		/// The property type (null if not specified)
		/// </summary>
		public PropertyType PropertyType
		{
			get
			{
				return this.mPropertyType;
			}
			set
			{
				this.mPropertyType = value;
			}
		}		

        /// <summary>
        /// The minimum number of bathrooms
        /// </summary>
        public int MinBathrooms
        {
            get
            {
                return this.mMinBathrooms;
            }
            set
            {
                this.mMinBathrooms = value;
            }
        }

        /// <summary>
        /// The minimum number of bedrooms
        /// </summary>
        public int MinBedrooms
        {
            get
            {
                return this.mMinBedrooms;
            }
            set
            {
                this.mMinBedrooms = value;
            }
        }

		#endregion
              
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RealEstateSearch()
        {
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node);               

             if (this.PriceLower != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "priceLower", this.PriceLower.ToString());        

            if(this.PriceUpper != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "priceUpper", this.PriceUpper.ToString());

            if (this.MinBedrooms != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "minBedrooms", this.MinBedrooms.ToString());

            if (this.MinBathrooms != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "minBathrooms", this.MinBathrooms.ToString());
            
            if(this.PropertyType != null)
            {
                XmlNode propertyTypeNode = node.OwnerDocument.CreateElement("PropertyType");
                this.PropertyType.PopulateNode(propertyTypeNode);
                node.AppendChild(propertyTypeNode);
            }                
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

            if (node.Attributes["priceLower"] != null)
                this.PriceLower = float.Parse(node.Attributes["priceLower"].Value);

            if (node.Attributes["priceUpper"] != null)
                this.PriceUpper = float.Parse(node.Attributes["priceUpper"].Value);

            if (node.Attributes["minBedrooms"] != null)
                this.MinBedrooms = int.Parse(node.Attributes["minBedrooms"].Value);

            if (node.Attributes["minBathrooms"] != null)
                this.MinBathrooms = int.Parse(node.Attributes["minBathrooms"].Value);

            if (node["PropertyType"] != null)
                this.PropertyType = PropertyType.FromXml(node["PropertyType"]);            
        }       
        #endregion
    }
}
