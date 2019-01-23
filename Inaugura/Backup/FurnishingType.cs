using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    // <summary>
    /// Represents the different furnish types
    /// </summary>
    public class FurnishingType : Types, Inaugura.Xml.IXmlable
    {
        public static FurnishingType NotSpecified = new FurnishingType(0, "Not Specified");
        /// <summary>
        /// Unfurnished 
        /// </summary>
        public static FurnishingType Unfurnished = new FurnishingType(1, "Unfurnished");
        /// <summary>
        /// Semi Furnished
        /// </summary>
        public static FurnishingType SemiFurnished = new FurnishingType(2, "Semi-furnished");
        /// <summary>
        /// Fully Furnished
        /// </summary>
        public static FurnishingType Furnished = new FurnishingType(3, "Furnished");

        public static FurnishingType[] All = new FurnishingType[] { NotSpecified, Unfurnished, SemiFurnished, Furnished };

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

        private FurnishingType(int value, string name)
            : base(value, name)
        {
        }

        /// <summary>
        /// Creates a furnish type instance from an xml node representation
        /// </summary>
        /// <param name="xml">The xml which represents the item</param>
        /// <returns>A Listing instance</returns>
        public static FurnishingType FromXml(XmlNode xml)
        {
            return FromValue(Types.GetTypeValue(xml));
        }


        /// <summary>
        /// Gets the furnish type associated with a specific value
        /// </summary>
        /// <param name="value">The interger value of the floor material</param>
        /// <returns>The property type matching the integer value</returns>
        public static FurnishingType FromValue(int value)
        {
            foreach (FurnishingType type in All)
            {
                if (type.Value == value)
                    return type;
            }
            throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
        }
    }
}
