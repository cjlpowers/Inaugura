using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different property types
	/// </summary>
	public class PropertyType : Types, Inaugura.Xml.IXmlable
	{
		public static PropertyType NotSpecified = new PropertyType(0, "Not Specified");
		public static PropertyType Bungalow = new PropertyType(1, "Bungalow");
		public static PropertyType OneAndOneHalfStorey = new PropertyType(2, "1.5 Storey");
		public static PropertyType TwoStoreySplit = new PropertyType(3, "2 Storey Split");
		public static PropertyType TwoStorey = new PropertyType(4, "2 Storey");
		public static PropertyType FourLevelSplit = new PropertyType(5, "4 Level Split");
		public static PropertyType TwoAndOneHalfStorey = new PropertyType(6, "2.5 Storey");
		public static PropertyType BiLevel = new PropertyType(7, "Bi-Level");
		public static PropertyType ThreeLevelSplit = new PropertyType(8, "3 Level Split");
		public static PropertyType FiveLevelSplit = new PropertyType(9, "5 Level Split");
		public static PropertyType Other = new PropertyType(-1, "Other");

		public static PropertyType[] All = new PropertyType[] { NotSpecified, Bungalow, OneAndOneHalfStorey, TwoStoreySplit, TwoStorey, FourLevelSplit, TwoAndOneHalfStorey, BiLevel, ThreeLevelSplit, FiveLevelSplit, Other};
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("PropertyType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private PropertyType(int value, string name) : base(value,name)
		{
		}
		
		/// <summary>
		/// Creates a property type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A Listing instance</returns>
		public static PropertyType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the property type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the floor material</param>
		/// <returns>The property type matching the integer value</returns>
		public static PropertyType FromValue(int value)
		{
			foreach (PropertyType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
