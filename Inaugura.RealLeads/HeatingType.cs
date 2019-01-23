using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of heating
	/// </summary>
	public class HeatingType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// The heating type is not specified
		/// </summary>
		public static HeatingType NotSpecified = new HeatingType(0, "Not Specified");
		/// <summary>
		/// Forced Air
		/// </summary>
		public static HeatingType ForcedAir = new HeatingType(1, "Forced Air");
		/// <summary>
		/// Radiant
		/// </summary>
		public static HeatingType Radiant = new HeatingType(2, "Radiant");
		/// <summary>
		/// Hot water
		/// </summary>
		public static HeatingType HotWater = new HeatingType(3, "Hot Water");
		/// <summary>
		/// Base Board Heaters
		/// </summary>
		public static HeatingType BaseBoard = new HeatingType(4, "Base Board Electric");
		/// <summary>
		/// Wood Stove
		/// </summary>
		public static HeatingType WoodStove = new HeatingType(5, "Wood Stove");
		/// <summary>
		/// Other
		/// </summary>
		public static HeatingType Other = new HeatingType(6, "Other");

		/// <summary>
		/// All Heating Types
		/// </summary>
		public static HeatingType[] All = new HeatingType[] { NotSpecified, ForcedAir, Radiant, HotWater, BaseBoard, WoodStove, Other };
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("FoundationType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private HeatingType(int value, string name)
			: base(value, name)
		{
		}
		
		/// <summary>
		/// Creates a heating type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A heating type instance</returns>
		public static HeatingType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the heating type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the heating type</param>
		/// <returns>The heating type matching the integer value</returns>
		public static HeatingType FromValue(int value)
		{
			foreach (HeatingType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
