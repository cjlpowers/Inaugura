using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class representing a rental listing
    /// </summary>
    public class RentalPropertyListing : ResidentialPropertyListing
    {
        #region Variables
        private RentalPropertyType mPropertyType;
        private FurnishingType mFurnishingType;
        private float mRent;        
        private DateTime mAvailabilityStart;
        private DateTime mAvailabilityEnd;
        private int mParkingSpaces;
        private bool mParkingIncluded;
        private bool mPets;
        private bool mIncludesElectricity;
        private bool mIncludesHeating;
        private bool mLaundryServices;
        private bool mInternetService;
        private bool mTelevisionService;
        #endregion

        #region Properties
        /// <summary>
        /// The rental property type
        /// </summary>
        public RentalPropertyType PropertyType
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
        /// The furnishing type
        /// </summary>
        public FurnishingType FurnishingType
        {
            get
            {
                return this.mFurnishingType;
            }
            set
            {
                this.mFurnishingType = value;
            }
        }

        
        /// <summary>
        /// The monthly rent cost
        /// </summary>
        public float MonthlyRent
        {
            get
            {
                return this.mRent;
            }
            set
            {
                this.mRent = value;
            }
        }

        /// <summary>
        /// The starting date of availability
        /// </summary>
        public DateTime AvailabilityStart
        {
            get
            {
                return this.mAvailabilityStart;
            }
            set
            {
                this.mAvailabilityStart = value;
            }
        }

        /// <summary>
        /// The ending date of availability
        /// </summary>
        public DateTime AvailabilityEnd
        {
            get
            {
                return this.mAvailabilityEnd;
            }
            set
            {
                this.mAvailabilityEnd = value;
            }
        }

        /// <summary>
        /// The number of parking spaces
        /// </summary>
        public int ParkingSpaces
        {
            get
            {
                return this.mParkingSpaces;
            }
            set
            {
                this.mParkingSpaces = value;
            }
        }

        /// <summary>
        /// A flag which indicates if parking is included with rent
        /// </summary>
        public bool ParkingIncluded
        {
            get
            {
                return this.mParkingIncluded;
            }
            set
            {
                this.mParkingIncluded = value;
            }
        }

        /// <summary>
        /// A flag which indicates the property has laundry services
        /// </summary>
        public bool LaundryServices
        {
            get
            {
                return this.mLaundryServices;
            }
            set
            {
                this.mLaundryServices = value;
            }
        }

        /// <summary>
        /// A flag which indicates the property has internet service
        /// </summary>
        public bool InternetService
        {
            get
            {
                return this.mInternetService;
            }
            set
            {
                this.mInternetService = value;
            }
        }

        /// <summary>
        /// The flag which indicates the property has television service
        /// </summary>
        public bool TelevisionService
        {
            get
            {
                return this.mTelevisionService;
            }
            set
            {
                this.mTelevisionService = value;
            }
        }

        /// <summary>
        /// A flag which indicates if pets are allowed
        /// </summary>
        public bool Pets
        {
            get
            {
                return this.mPets;
            }
            set
            {
                this.mPets = value;
            }
        }

        /// <summary>
        /// A flag which indicates if electricity is included in rent
        /// </summary>
        public bool IncludesElectricity
        {
            get
            {
                return this.mIncludesElectricity;
            }
            set
            {
                this.mIncludesElectricity = value;
            }
        }

        /// <summary>
        /// A flag which indicates if heating is included in rent
        /// </summary>
        public bool IncludesHeating
        {
            get
            {
                return this.mIncludesHeating;
            }
            set
            {
                this.mIncludesHeating = value;
            }
        }        
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public RentalPropertyListing() : this(RentalPropertyType.NotSpecified)
        {
        }

        /// <summary>
        /// Constructor
        /// <param name="type">The property type</param>
        /// </summary>
        public RentalPropertyListing(RentalPropertyType type)
        {
            this.PropertyType = type;
            this.mFurnishingType = FurnishingType.NotSpecified;
            this.AvailabilityStart = DateTime.MinValue;
            this.AvailabilityEnd = DateTime.MaxValue;
        }

        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="xml">The xml representation of the listing</param>
        public RentalPropertyListing(XmlNode listingNode)
            : base(listingNode)
		{           
		}

        	/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

            if (node.Attributes["monthlyRent"] != null)
                this.MonthlyRent = float.Parse(Inaugura.Xml.Helper.GetAttribute(node, "monthlyRent"));

            try
            {
                if (node.Attributes["availabilityStart"] == null || !DateTime.TryParseExact(node.Attributes["availabilityStart"].Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out this.mAvailabilityStart))
                    this.AvailabilityStart = DateTime.MinValue;
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format("Date Format exception {'0'}", node.Attributes["availabilityStart"].Value), ex);
            }

            try
            {
                if (node.Attributes["availabilityEnd"] == null || !DateTime.TryParseExact(node.Attributes["availabilityEnd"].Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out this.mAvailabilityEnd))
                    this.AvailabilityEnd = DateTime.MaxValue;
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format("Date Format exception {'0'}", node.Attributes["availabilityEnd"].Value), ex);
            }


            if (node.Attributes["parkingSpaces"] != null)
                this.ParkingSpaces = int.Parse(Inaugura.Xml.Helper.GetAttribute(node, "parkingSpaces"));

            if (node.Attributes["parkingIncluded"] != null)
                this.ParkingIncluded = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "parkingIncluded"));
                        
            if (node.Attributes["pets"] != null)
                this.mPets = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "pets"));

            if (node.Attributes["includesElectricity"] != null)
                this.IncludesElectricity = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "includesElectricity"));

            if (node.Attributes["includesHeating"] != null)
                this.IncludesHeating = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "includesHeating"));            

            if (node.Attributes["laundryService"] != null)
                this.LaundryServices = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "laundryService"));

            if (node.Attributes["internetService"] != null)
                this.InternetService = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "internetService"));

            if (node.Attributes["televisionService"] != null)
                this.TelevisionService = bool.Parse(Inaugura.Xml.Helper.GetAttribute(node, "televisionService"));

            if (node["RentalPropertyType"] != null)
                this.PropertyType = RentalPropertyType.FromXml(node["RentalPropertyType"]);
            else
                this.PropertyType = RentalPropertyType.NotSpecified;

            if (node["FurnishingType"] != null)
                this.FurnishingType = FurnishingType.FromXml(node["FurnishingType"]);
            else
                this.FurnishingType = FurnishingType.NotSpecified;
        }

        	/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node);
            if (this.MonthlyRent != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "monthlyRent", this.MonthlyRent.ToString());

            if (this.AvailabilityStart != DateTime.MinValue)
                Inaugura.Xml.Helper.SetAttribute(node, "availabilityStart", this.AvailabilityStart.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

            if (this.AvailabilityEnd != DateTime.MaxValue)
                Inaugura.Xml.Helper.SetAttribute(node, "availabilityEnd", this.AvailabilityEnd.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

            if (this.ParkingSpaces != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "parkingSpaces", this.ParkingSpaces.ToString());

            if (this.ParkingIncluded)
                Inaugura.Xml.Helper.SetAttribute(node, "parkingIncluded", this.ParkingIncluded.ToString());
                        
            if (this.mPets)
                Inaugura.Xml.Helper.SetAttribute(node, "pets", this.mPets.ToString());

            if (this.IncludesElectricity)
                Inaugura.Xml.Helper.SetAttribute(node, "includesElectricity", this.IncludesElectricity.ToString());
            
            if (this.IncludesHeating)
                Inaugura.Xml.Helper.SetAttribute(node, "includesHeating", this.IncludesHeating.ToString());

            if (this.LaundryServices)
                Inaugura.Xml.Helper.SetAttribute(node, "laundryService", this.LaundryServices.ToString());

            if (this.InternetService)
                Inaugura.Xml.Helper.SetAttribute(node, "internetService", this.InternetService.ToString());

            if (this.TelevisionService)
                Inaugura.Xml.Helper.SetAttribute(node, "televisionService", this.TelevisionService.ToString());

            if (this.PropertyType != RentalPropertyType.NotSpecified)
            {
                XmlNode propertyTypeNode = node.OwnerDocument.CreateElement("RentalPropertyType");
                this.PropertyType.PopulateNode(propertyTypeNode);
                node.AppendChild(propertyTypeNode);
            }

            if (this.FurnishingType != FurnishingType.NotSpecified)
            {
                XmlNode furnishingTypeNode = node.OwnerDocument.CreateElement("FurnishingType");
                this.FurnishingType.PopulateNode(furnishingTypeNode);
                node.AppendChild(furnishingTypeNode);
            }
        }
					
        #endregion
    }
}
