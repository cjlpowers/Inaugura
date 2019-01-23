using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of water 
	/// </summary>
	public class WaterType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// The roof type was somthing other then thoes supported
		/// </summary>
		public static WaterType Other = new WaterType(-1, "Other");
		/// <summary>
		/// The roof type is not specified
		/// </summary>
		public static WaterType NotSpecified = new WaterType(0, "Not Specified");
		/// <summary>
		/// City Water
		/// </summary>
		public static WaterType City = new WaterType(1, "City");
		/// <summary>
		/// Well Water
		/// </summary>
		public static WaterType Well = new WaterType(2, "Well");		
		public static WaterType[] All = new WaterType[] { NotSpecified, City, Well, Other };
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("WaterType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private WaterType(int value, string name)
			: base(value, name)
		{
		}
		
		/// <summary>
		/// Creates a water type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A water type instance</returns>
		public static WaterType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}

		/// <summary>
		/// Gets the water type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the water type</param>
		/// <returns>The water type matching the integer value</returns>
		public static WaterType FromValue(int value)
		{
			foreach (WaterType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
