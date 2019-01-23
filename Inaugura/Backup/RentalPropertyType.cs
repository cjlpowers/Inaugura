using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    // <summary>
    /// Represents the different property types
    /// </summary>
    public class RentalPropertyType : Types, Inaugura.Xml.IXmlable
    {
        public static RentalPropertyType NotSpecified = new RentalPropertyType(0, "Not Specified");

        public static RentalPropertyType Other = new RentalPropertyType(-1, "Other");
        public static RentalPropertyType House = new RentalPropertyType(1, "House");
        public static RentalPropertyType Apartment = new RentalPropertyType(2, "Apartment");
        public static RentalPropertyType Condominium = new RentalPropertyType(3, "Condominium");

        public static RentalPropertyType[] All = new RentalPropertyType[] { NotSpecified, House, Apartment, Condominium, Other };

        #region Properties
        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Listing
        /// </summary>
        /// <value></value>
        public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("RentalPropertyType");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion
        #endregion

        private RentalPropertyType(int value, string name)
            : base(value, name)
        {
        }

        /// <summary>
        /// Creates a property type instance from an xml node representation
        /// </summary>
        /// <param name="xml">The xml which represents the item</param>
        /// <returns>A Listing instance</returns>
        public static RentalPropertyType FromXml(XmlNode xml)
        {
            return FromValue(Types.GetTypeValue(xml));
        }


        /// <summary>
        /// Gets the property type associated with a specific value
        /// </summary>
        /// <param name="value">The interger value of the floor material</param>
        /// <returns>The property type matching the integer value</returns>
        public static RentalPropertyType FromValue(int value)
        {
            foreach (RentalPropertyType type in All)
            {
                if (type.Value == value)
                    return type;
            }            
            throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
        }
    }
}
