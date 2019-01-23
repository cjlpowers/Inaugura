#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Maps
{
    /// <summary>
    /// A class wich represents a locale
    /// </summary>
	public class Locale : Inaugura.Xml.IXmlable, IGeocode
	{
        #region Internal Constructs
        /// <summary>
        /// The locale types
        /// </summary>
        [Flags]
        public enum LocaleType
        {
            /// <summary>
            /// An unspecified locale
            /// </summary>
            NotSpecified = 1,
            /// <summary>
            /// The regional districts
            /// </summary>
            District = 2,
            /// <summary>
            /// University
            /// </summary>
            University = 4,
            /// <summary>
            /// College
            /// </summary>
            College = 8,
            /// <summary>
            /// All educational locales
            /// </summary>
            AllEducation = University | College,
            /// <summary>
            /// Employer
            /// </summary>
            Employer = 16,
            /// <summary>
            /// All Types
            /// </summary>
            All = NotSpecified | District | AllEducation | Employer         
        }
        #endregion

		#region Variables
        private string mID;
        private LocaleType mType;
        private Address mAddress;
        private string mName;
		private Details mDetails;
        private double mRadius;
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
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Locale");
                this.PopulateNode(node);
                return node;
    		}
		}
		#endregion

        #region IGeocode Members

        public double Latitude
        {
            get
            {
                return this.mAddress.Latitude;
            }
            set
            {
                this.mAddress.Latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return this.mAddress.Longitude;
            }
            set
            {
                this.mAddress.Longitude = value;
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
        /// The locale type
        /// </summary>
        public LocaleType Type
        {
            get
            {
                return this.mType;
            }
            set
            {
                this.mType = value;
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
        /// The address of the locale
        /// </summary>
        public Address Address
        {
            get
            {
                if (this.mAddress != null)
                {
                    // make sure the label on of the address is the name of this local
                    this.mAddress.Label = this.Name;
                }
                return this.mAddress;                
            }
            set
            {
                this.mAddress = value;
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
        /// The radius of the locale in km
        /// </summary>
        public double Radius
        {
            get
            {
                return this.mRadius;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("radius", "The radius must be greater then zero");

                this.mRadius = value;
            }
        }
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="localeNode">The xml representation of the Locale</param>
		public Locale(XmlNode localeNode) : this(string.Empty, new Address(), 5)
		{
            if (localeNode == null)
                throw new ArgumentNullException("localeNode", "The Xml definition may not be null");

            this.PopulateInstance(localeNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
        /// <param name="name">The city name</param>
        /// <param name="address">The address of the locale</param>
        /// <param name="radius">The radius of the locale</param>
        public Locale(string name, Address address, double radius)
		{                 
            this.ID = Guid.NewGuid().ToString();
            this.Address = address;
            this.Radius = radius;
            this.Details = new Details();
            this.Type = LocaleType.NotSpecified;
            this.Name = name;
		}

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {            
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID);            
            Inaugura.Xml.Helper.SetAttribute(node, "name", this.Name);
            Inaugura.Xml.Helper.SetAttribute(node, "localeType", ((int)this.Type).ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "radius", this.Radius.ToString());

            XmlNode addressNode = node.OwnerDocument.CreateElement("Address");
            this.mAddress.PopulateNode(addressNode);
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
            if (node.Attributes["id"] == null)
                throw new ArgumentException("The xml does not contain a id attribute");

            if (node.Attributes["name"] == null)
                throw new ArgumentException("The xml does not contain a name attribute");

            if (node.Attributes["localeType"] == null)
                throw new ArgumentException("The xml does not contain a localeType attribute");

            if (node.Attributes["radius"] == null)
                throw new ArgumentException("The xml does not contain a radius attribute");

			this.ID = Inaugura.Xml.Helper.GetAttribute(node, "id");
			this.Name = Inaugura.Xml.Helper.GetAttribute(node, "name");
            this.Type = (LocaleType)Enum.Parse(typeof(LocaleType), Inaugura.Xml.Helper.GetAttribute(node, "localeType"));
            double.TryParse(Inaugura.Xml.Helper.GetAttribute(node, "radius"), out this.mRadius);

            if (node["Address"] != null)
                this.Address = new Address(node["Address"]);
        
            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
        }        
    }
}
