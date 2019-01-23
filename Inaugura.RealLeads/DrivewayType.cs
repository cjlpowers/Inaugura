using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of heating
	/// </summary>
	public class DrivewayType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// Other
		/// </summary>
		public static DrivewayType Other = new DrivewayType(-1, "Other");
		/// <summary>
		/// The heating type is not specified
		/// </summary>
		public static DrivewayType NotSpecified = new DrivewayType(0, "Not Specified");
		/// <summary>
		/// Crushed Stone
		/// </summary>
		public static DrivewayType CrushedStone = new DrivewayType(1, "Crushed Stone");
		/// <summary>
		/// Concrete
		/// </summary>
		public static DrivewayType Concrete = new DrivewayType(2, "Concrete");
		/// <summary>
		/// Asphalt
		/// </summary>
		public static DrivewayType Asphault = new DrivewayType(3, "Asphalt");				

		/// <summary>
		/// All Heating Types
		/// </summary>
		public static DrivewayType[] All = new DrivewayType[] { NotSpecified, CrushedStone, Concrete, Asphault, Other };
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("DrivewayType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private DrivewayType(int value, string name)
			: base(value, name)
		{
		}
		
		/// <summary>
		/// Creates a heating type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A heating type instance</returns>
		public static DrivewayType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the heating type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the heating type</param>
		/// <returns>The heating type matching the integer value</returns>
		public static DrivewayType FromValue(int value)
		{
			foreach (DrivewayType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
