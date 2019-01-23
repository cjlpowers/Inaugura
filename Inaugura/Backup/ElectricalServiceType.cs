using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of electrical service
	/// </summary>
	public class ElectricalServiceType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// Other
		/// </summary>
		public static ElectricalServiceType Other = new ElectricalServiceType(-1, "Other");
		/// <summary>
		/// The heating type is not specified
		/// </summary>
		public static ElectricalServiceType NotSpecified = new ElectricalServiceType(0, "Not Specified");
		/// <summary>
		/// Forced Air
		/// </summary>
		public static ElectricalServiceType SixtyAmp = new ElectricalServiceType(1, "60 AMP");
		/// <summary>
		/// Radiant
		/// </summary>
		public static ElectricalServiceType OneHundredAmp = new ElectricalServiceType(2, "100 AMP");
		/// <summary>
		/// Hot water
		/// </summary>
		public static ElectricalServiceType TwoHundredAmp = new ElectricalServiceType(3, "200 AMP");
			

		/// <summary>
		/// All Heating Types
		/// </summary>
		public static ElectricalServiceType[] All = new ElectricalServiceType[] { NotSpecified, SixtyAmp, OneHundredAmp, TwoHundredAmp, Other };
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("ElectricalServiceType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private ElectricalServiceType(int value, string name)
			: base(value, name)
		{
		}
		
		/// <summary>
		/// Creates a heating type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A heating type instance</returns>
		public static ElectricalServiceType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the heating type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the heating type</param>
		/// <returns>The heating type matching the integer value</returns>
		public static ElectricalServiceType FromValue(int value)
		{
			foreach (ElectricalServiceType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
