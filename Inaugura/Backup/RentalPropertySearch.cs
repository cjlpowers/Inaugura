using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which holds the search criteria
	/// </summary>
    public class RentalPropertySearch : ResidentialPropertySearch
	{
		#region Variables
       	private float mRentUpper;
		private float mRentLower;
        
        private int mMinBedrooms;
        private int mMinParkingSpaces;

        private bool mIncludesElectricity;
        private bool mIncludesHeating;
        private bool mPets;
        private bool mLaundryService;
        private bool mInternetService;
        private bool mTelevisionService;

		private RentalPropertyType mPropertyType;

        private DateTime mAvailabilityStart;
        private DateTime mAvailabilityEnd;
		#endregion

		#region Properties
      	/// <summary>
		/// The upper rent limit (0 if not specified)
		/// </summary>
		public float RentUpper
		{
			get
			{
				return this.mRentUpper;
			}
			set
			{
				this.mRentUpper = value;
			}
		}

		/// <summary>
		/// The lower rent limit (0 if not specified)
		/// </summary>
		public float RentLower
		{
			get
			{
				return this.mRentLower;
			}
			set
			{
				this.mRentLower = value;
			}
		}

		/// <summary>
		/// The property type (null if not specified)
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

          /// <summary>
        /// The minimum number of bedrooms
        /// </summary>
        public int MinParkingSpaces
        {
            get
            {
                return this.mMinParkingSpaces;
            }
            set
            {
                this.mMinParkingSpaces = value;
            }
        }

        /// <summary>
        /// Includes Electricity as part of rent
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
        /// Includes Heating as part of rent
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


        /// <summary>
        /// The property contains laundry services
        /// </summary>
        public bool LaundryService
        {
            get
            {
                return this.mLaundryService;
            }
            set
            {
                this.mLaundryService = value;
            }
        }

        /// <summary>
        /// The property contains internet services
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
        /// The property contains internet services
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
        /// The property allows pets
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
        /// The start date of availability
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
        /// The end date of availability
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
		#endregion
              
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RentalPropertySearch()
        {
            this.AvailabilityEnd = DateTime.MaxValue;
            this.AvailabilityStart = DateTime.MinValue;
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node); 

            if (this.RentUpper != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "rentUpper", this.RentUpper.ToString());        

            if(this.RentLower != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "rentLower", this.RentLower.ToString());

            if (this.MinBedrooms != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "minBedrooms", this.MinBedrooms.ToString());

            if (this.MinParkingSpaces != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "minParkingSpaces", this.MinParkingSpaces.ToString());

            if (this.AvailabilityStart != DateTime.MinValue)
                Inaugura.Xml.Helper.SetAttribute(node, "availabilityStart", this.AvailabilityStart.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

            if (this.AvailabilityEnd != DateTime.MaxValue)
                Inaugura.Xml.Helper.SetAttribute(node, "availabilityEnd", this.AvailabilityEnd.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

            if (this.mIncludesElectricity)
                Inaugura.Xml.Helper.SetAttribute(node, "includesElectricity", this.mIncludesElectricity.ToString());
           
            if (this.mIncludesHeating)
                Inaugura.Xml.Helper.SetAttribute(node, "includesHeating", this.mIncludesHeating.ToString());

            if (this.mInternetService)
                Inaugura.Xml.Helper.SetAttribute(node, "internetService", this.mInternetService.ToString());

            if (this.mTelevisionService)
                Inaugura.Xml.Helper.SetAttribute(node, "televisionService", this.mTelevisionService.ToString());

            if (this.mLaundryService)
                Inaugura.Xml.Helper.SetAttribute(node, "laundryService", this.mLaundryService.ToString());

            if (this.mPets)
                Inaugura.Xml.Helper.SetAttribute(node, "pets", this.mPets.ToString());
            
            if(this.PropertyType != null)
            {
                XmlNode propertyTypeNode = node.OwnerDocument.CreateElement("RentalPropertyType");
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

            if (node.Attributes["availabilityStart"] == null || !DateTime.TryParseExact(node.Attributes["availabilityStart"].Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out this.mAvailabilityStart))
                this.AvailabilityStart = DateTime.MinValue;

               if (node.Attributes["availabilityEnd"] == null || !DateTime.TryParseExact(node.Attributes["availabilityEnd"].Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out this.mAvailabilityEnd))
                    this.AvailabilityEnd = DateTime.MaxValue;

            if (node.Attributes["rentUpper"] != null)
                this.RentUpper = float.Parse(node.Attributes["rentUpper"].Value);

            if (node.Attributes["rentLower"] != null)
                this.RentLower = float.Parse(node.Attributes["rentLower"].Value);

            if (node.Attributes["minBedrooms"] != null)
                this.MinBedrooms = int.Parse(node.Attributes["minBedrooms"].Value);

            if (node.Attributes["minParkingSpaces"] != null)
                this.MinParkingSpaces = int.Parse(node.Attributes["minParkingSpaces"].Value);

            if (node.Attributes["includesElectricity"] != null)
                this.mIncludesElectricity = bool.Parse(node.Attributes["includesElectricity"].Value);

            if (node.Attributes["includesHeating"] != null)
                this.mIncludesHeating = bool.Parse(node.Attributes["includesHeating"].Value);

            if (node.Attributes["internetService"] != null)
                this.mInternetService = bool.Parse(node.Attributes["internetService"].Value);

            if (node.Attributes["televisionService"] != null)
                this.mTelevisionService = bool.Parse(node.Attributes["televisionService"].Value);

            if (node.Attributes["laundryService"] != null)
                this.mLaundryService = bool.Parse(node.Attributes["laundryService"].Value);

            if (node.Attributes["pets"] != null)
                this.mPets = bool.Parse(node.Attributes["pets"].Value);

            if (node["RentalPropertyType"] != null)
                this.PropertyType = RentalPropertyType.FromXml(node["RentalPropertyType"]);            
        }       
        #endregion
    }
}
