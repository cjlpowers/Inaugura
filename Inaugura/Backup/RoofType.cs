using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of roofing materials
	/// </summary>
	public class RoofType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// The roof type was somthing other then thoes supported
		/// </summary>
		public static RoofType Other = new RoofType(-1, "Other");
		/// <summary>
		/// The roof type is not specified
		/// </summary>
		public static RoofType NotSpecified = new RoofType(0, "Not Specified");
		/// <summary>
		/// Asphalt Shingles
		/// </summary>
		public static RoofType AsphaltShingles = new RoofType(1, "Asphalt Shingles");
		/// <summary>
		/// Metal
		/// </summary>
		public static RoofType Metal = new RoofType(2, "Metal");
		/// <summary>
		/// Wood Shakes
		/// </summary>
		public static RoofType WoodShakes = new RoofType(3, "Wood Shakes");
		/// <summary>
		/// Slate
		/// </summary>
		public static RoofType Slate = new RoofType(4, "Slate");
		/// <summary>
		/// Ceramic
		/// </summary>
		public static RoofType Ceramic = new RoofType(5, "Ceramic");	
		/// <summary>
		/// All roofing types
		/// </summary>
		public static RoofType[] All = new RoofType[] { NotSpecified, AsphaltShingles, Metal, WoodShakes, Slate, Ceramic, Other };
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("RoofType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private RoofType(int value, string name)
			: base(value, name)
		{
		}
		
		/// <summary>
		/// Creates a roof type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A roof type instance</returns>
		public static RoofType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the roof type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the heating type</param>
		/// <returns>The roof type matching the integer value</returns>
		public static RoofType FromValue(int value)
		{
			foreach (RoofType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
