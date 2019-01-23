#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Maps
{
    /// <summary>
    /// A class wich represents the 
    /// </summary>
	public class City : Inaugura.Xml.IXmlable, IGeocode
	{
		#region Variables
        private string mID;
        private string mProvinceID;
        private string mName;
		private Details mDetails;
        private double mLatitude;
        private double mLongitude;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the City
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("City");
                this.PopulateNode(node);
                return node;
    		}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the District
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
        /// The ID of the city which the district is located in
        /// </summary>
        public string ProvinceID
        {
            get
            {
                return this.mProvinceID;
            }
            set
            {
                this.mProvinceID = value;
            }
        }

		/// <summary>
		/// The name of the City
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
		/// Additional details specific to this City
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

        /// <summary>
        /// The longitude
        /// </summary>
        public double Longitude
        {
            get
            {
                return this.mLongitude;
            }
            set
            {
                this.mLongitude = value;
            }
        }

        /// <summary>
        /// The latitude
        /// </summary>
        public double Latitude
        {
            get
            {
                return this.mLatitude;
            }
            set
            {
                this.mLatitude = value;
            }
        }
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="cityNode">The xml representation of the City</param>
		public City(XmlNode cityNode) : this(string.Empty, string.Empty)
		{
            if (cityNode == null)
                throw new ArgumentNullException("cityNode", "The Xml definition may not be null");

            this.PopulateInstance(cityNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
        /// <param name="name">The city name</param>
        /// <param name="provinceID">The parent province ID</param>
        /// <param name="id">The ID of the city</param>
        public City(string name, string provinceID, string id)
		{
            this.ID = id;
            this.ProvinceID = provinceID;
            this.Name = name;
            this.Details = new Details();
		}

        /// <summary>
        /// Constructor
        /// </summary>		
        /// <param name="name">The city name</param>
        /// <param name="provinceID">The parent province ID</param>
        public City(string name, string provinceID) : this(name,provinceID,Guid.NewGuid().ToString())
        {
        }


        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {            
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);
            Inaugura.Xml.Helper.SetAttribute(node, "provinceID", this.ProvinceID);
            
            Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);

            if (this.Latitude != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "latitude", this.Latitude.ToString());
            if (this.Longitude != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "longitude", this.Longitude.ToString());

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

            if (node.Attributes["provinceID"] == null)
                throw new ArgumentException("The xml does not contain a regionId attribute");

            if (node.Attributes["name"] == null)
                throw new ArgumentException("The xml does not contain a name attribute");

			this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
            this.ProvinceID = Inaugura.Xml.Helper.GetAttribute(node, "provinceID");			
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");

            if (node.Attributes["latitude"] != null)
                double.TryParse(Inaugura.Xml.Helper.GetAttribute(node, "latitude"), out this.mLatitude);

            if (node.Attributes["longitude"] != null)
                double.TryParse(Inaugura.Xml.Helper.GetAttribute(node, "longitude"), out this.mLongitude);
        
            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }
	}
}
