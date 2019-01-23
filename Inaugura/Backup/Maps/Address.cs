#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
#endregion

namespace Inaugura.Maps
{
	/// <summary>
	/// An Address Class representing a physical address
	/// </summary>
	public sealed class Address : Inaugura.Xml.IXmlable, IGeocode
    {       
        #region Variables
        private string mStreet;
		private string mCity;
		private string mCountry;
		private string mStateProv;
		private string mZipPostal;
        private double mLatitude;
        private double mLongitude;
        
		private string mCountryID;
		private string mProvinceID;
		private string mRegionID;
		private string mCityID;
		private string mDistrictID;
        private string mLabel;
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Address
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get 
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Address");
				this.PopulateNode(node);
				return node;
			}
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
        public void PopulateNode(XmlNode node)
        {
            if (this.Street != null && this.Street != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "street", this.Street);
            
            if (this.City != null && this.City != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "city", this.City);
            
            if (this.StateProv != null && this.StateProv != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "stateProv", this.StateProv);

            if (this.Country != null && this.Country != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "country", this.Country);
            
            if (this.ZipPostal != null && this.ZipPostal != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "zipPostal", this.ZipPostal);
          

            if (this.Latitude != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "latitude", this.Latitude.ToString());
            if (this.Longitude != 0)
                Inaugura.Xml.Helper.SetAttribute(node, "longitude", this.Longitude.ToString());

            if (this.CountryID != null && this.CountryID != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "countryID", this.CountryID);

            if (this.StateProvID != null && this.StateProvID != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "stateProvID", this.StateProvID);

            if (this.RegionID != null && this.RegionID != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "regionID", this.RegionID);

            if (this.CityID != null && this.CityID != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "cityID", this.CityID);

            if (this.DistrictID != null && this.DistrictID != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "districtID", this.DistrictID);
        }

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node"></param>
        public void PopulateInstance(XmlNode node)
        {
            if (node.Attributes["street"] != null)
                this.Street = Inaugura.Xml.Helper.GetAttribute(node, "street");
            if (node.Attributes["city"] != null)
                this.City = Inaugura.Xml.Helper.GetAttribute(node, "city");
            if (node.Attributes["country"] != null)
                this.Country = Inaugura.Xml.Helper.GetAttribute(node, "country");
            if (node.Attributes["zipPostal"] != null)
                this.ZipPostal = Inaugura.Xml.Helper.GetAttribute(node, "zipPostal");
            if (node.Attributes["stateProv"] != null)
                this.StateProv = Inaugura.Xml.Helper.GetAttribute(node, "stateProv");

            if (node.Attributes["countryID"] != null)
                this.CountryID = Inaugura.Xml.Helper.GetAttribute(node, "countryID");

            if (node.Attributes["stateProvID"] != null)
                this.StateProvID = Inaugura.Xml.Helper.GetAttribute(node, "stateProvID");

            if (node.Attributes["regionID"] != null)
                this.RegionID = Inaugura.Xml.Helper.GetAttribute(node, "regionID");

            if (node.Attributes["cityID"] != null)
                this.CityID = Inaugura.Xml.Helper.GetAttribute(node, "cityID");

            if (node.Attributes["districtID"] != null)
                this.DistrictID = Inaugura.Xml.Helper.GetAttribute(node, "districtID");

            if (node.Attributes["latitude"] != null)
                double.TryParse(Inaugura.Xml.Helper.GetAttribute(node, "latitude"), out this.mLatitude);

            if (node.Attributes["longitude"] != null)
                double.TryParse(Inaugura.Xml.Helper.GetAttribute(node, "longitude"), out this.mLongitude);
        }
		#endregion

		#region Properties
        /// <summary>
        /// The label for this address
        /// </summary>
        /// <remarks>Not persisted to XML</remarks>
        /// <value></value>
        [Description("A the description of this address")]
        public string Label
        {
            get
            {
                if (!string.IsNullOrEmpty(this.mLabel))
                    return this.mLabel;
                else if (!string.IsNullOrEmpty(this.Street))
                    return this.Street;
                else if (!string.IsNullOrEmpty(this.ZipPostal))
                    return "Postal Code " + this.ZipPostal;
                else if (!string.IsNullOrEmpty(this.City))
                    return this.City;
                else
                    return string.Empty;
            }
            set
            {
                this.mLabel = value;
            }
        }
        
		/// <summary>
		/// The Street Address
		/// </summary>
		/// <value></value>
		[Description("The street address")]
		public string Street
		{
			get
			{
				return this.mStreet;
			}
			set
			{
				this.mStreet = value;
			}
		}

		/// <summary>
		/// The City Name
		/// </summary>
		/// <value></value>
		[Description("The city")]
		public string City
		{
			get
			{
				return this.mCity;
			}
			set
			{
				this.mCity = value;
			}
		}

		/// <summary>
		/// The Country Name
		/// </summary>
		/// <value></value>
		[Description("The country")]
		public string Country
		{
			get
			{
				return this.mCountry;	
			}
			set
			{
				this.mCountry = value;
			}
		}

		/// <summary>
		/// The Zip/Postal Code
		/// </summary>
		/// <value></value>
		[Description("The zip/postal code")]
		public string ZipPostal
		{
			get
			{
				return this.mZipPostal;
			}
			set
			{
				this.mZipPostal = value;
			}
		}

		/// <summary>
		/// The State/Province Name
		/// </summary>
		/// <value></value>
		[Description("The state or provence")]
		public string StateProv
		{
			get
			{
				return this.mStateProv;
			}
			set
			{
				this.mStateProv = value;
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

		/// <summary>
		/// The ID of the country
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
		/// The ID of the state/province
		/// </summary>
		public string StateProvID
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
		/// The ID of the region
		/// </summary>
		public string RegionID
		{
			get
			{
				return this.mRegionID;
			}
			set
			{
				this.mRegionID = value;
			}
		}

		/// <summary>
		/// The ID of the city
		/// </summary>
		public string CityID
		{
			get
			{
				return this.mCityID;
			}
			set
			{
				this.mCityID = value;
			}
		}

		/// <summary>
		/// The ID of the district
		/// </summary>
		public string DistrictID
		{
			get
			{
				return this.mDistrictID;
			}
			set
			{
				this.mDistrictID = value;
			}
		}

        /// <summary>
        /// The google maps address of this url
        /// </summary>
        public string GoogleMapsUrl
        {
            get
            {
                return "http://maps.google.com/maps?q=" + string.Format("{0},{1},{2}", System.Web.HttpUtility.UrlEncode(this.Street), System.Web.HttpUtility.UrlEncode(this.City), System.Web.HttpUtility.UrlEncode(this.StateProv));
            }
        }

        /// <summary>
        /// The google maps address of this url
        /// </summary>
        public string YahooMapsUrl
        {
            get
            {
                return string.Format("http://maps.yahoo.com/beta/#maxp=search&q1={0}&mvt=m&trf=0&lon={1}&lat={2}", string.Format("{0}, {1}, {2}", System.Web.HttpUtility.UrlEncode(this.Street), System.Web.HttpUtility.UrlEncode(this.City), System.Web.HttpUtility.UrlEncode(this.StateProv)).Trim(','), this.Longitude, this.Latitude);
            }
        }

		#endregion	

        #region Methods
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="addressNode">The Xml Node which defines the Address</param>
		public Address(XmlNode addressNode) : this()
		{
			this.PopulateInstance(addressNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public Address() 
		{
			this.CountryID = string.Empty;
			this.StateProvID = string.Empty;
			this.RegionID = string.Empty;
			this.CityID = string.Empty;
			this.DistrictID = string.Empty;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="street">The Street name</param>
		/// <param name="city">The City name</param>
		/// <param name="stateProv">The State/Province name</param>
		/// <param name="country">The Country name</param>
		/// <param name="zipPostal">The Zip/Postal code</param>
		public Address(string street, string city, string stateProv, string country, string zipPostal) : this()
		{
			this.Street = street;
			this.City = city;
			this.StateProv = stateProv;
			this.Country = country;
			this.ZipPostal = zipPostal;			
		}

		/// <summary>
		/// Converts the Address to a string representation
		/// </summary>
		/// <returns>The Address in string form</returns>
		public override string ToString()
		{
            string str = string.Empty;
            if(this.Street != string.Empty)
			    str += this.Street + ", ";
            if (this.City != string.Empty)
			    str += this.City + ", ";
            if (this.StateProv != string.Empty)
			    str += this.StateProv + ", ";
            if (this.Country != string.Empty)
			    str += this.Country + ", ";
            if (this.ZipPostal != string.Empty)
			    str += this.ZipPostal;
			return str.TrimEnd(',',' ');
		}

        public override int GetHashCode()
        {
            int hashCode = 0;

            if (this.mCity != null)
                hashCode ^= this.mCity.GetHashCode();
            if (this.mCityID != null)
                hashCode ^= this.mCityID.GetHashCode();
            if (this.mCountry != null)
                hashCode ^= this.mCountry.GetHashCode();
            if (this.mCountryID != null)
                hashCode ^= this.mCountryID.GetHashCode();
            if (this.mDistrictID != null)
                hashCode ^= this.mDistrictID.GetHashCode();
            if (this.mProvinceID != null)
                hashCode ^= this.mProvinceID.GetHashCode();
            if (this.mRegionID != null)
                hashCode ^= this.mRegionID.GetHashCode();
            if (this.mStateProv != null)
                hashCode ^= this.mStateProv.GetHashCode();
            if (this.mStreet != null)
                hashCode ^= this.mStreet.GetHashCode();
            if (this.mZipPostal != null)
                hashCode ^= this.mZipPostal.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Calculates the distance in kilometers to another location
        /// </summary>
        /// <param name="location">The location</param>
        /// <returns>The distance in kilometers </returns>
        public double Distance(Address location)
        {
            return Address.CalculateDistance(this, location);
        }

        #region Static Methods
        /// <summary>
        /// Calculates the distance between to lat/long points and returns the approximate distance in kilometers
        /// </summary>
        /// <param name="from">A geocode representing the from location</param>
        /// <param name="to">A geocode representing the to location</param>
        /// <returns>Distance in kilometers</returns>
        public static double CalculateDistance(IGeocode from, IGeocode to)
        {
            return CalculateDistance(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
        }

        /// <summary>
        /// Calculates the distance between to lat/long points and returns the approximate distance in kilometers
        /// </summary>
        /// <param name="latitude1">The latitude of location 1</param>
        /// <param name="longitude1">The longitude of location 1</param>
        /// <param name="latitude2">The latitude of location 2</param>
        /// <param name="longitude2">The longitude of location 2</param>
        /// <returns>Distance in kilometers</returns>
        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            const double rad = 6371; //Earth radius in Km
            //Convert to radians
            double p1Longitude = longitude1 / 180 * Math.PI;
            double p1Latitude = latitude1 / 180 * Math.PI;
            double p2Longitude = longitude2 / 180 * Math.PI;
            double p2Latitude = latitude2 / 180 * Math.PI;

            return Math.Acos(Math.Sin(p1Latitude) * Math.Sin(p2Latitude) +
                Math.Cos(p1Latitude) * Math.Cos(p2Latitude) * Math.Cos(p2Longitude - p1Longitude)) * rad;
        }
        #endregion
        #endregion
    }
}
