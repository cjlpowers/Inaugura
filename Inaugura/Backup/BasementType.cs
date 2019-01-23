using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of heating
	/// </summary>
	public class BasementType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// The basement type is not specified
		/// </summary>
		public static BasementType NotSpecified = new BasementType(0, "Not Specified");
		/// <summary>
		/// The basement is fully finished
		/// </summary>
		public static BasementType Finished = new BasementType(1, "Finished");
		/// <summary>
		/// The basement is partly finished
		/// </summary>
		public static BasementType PartlyFinished = new BasementType(2, "Partly Finished");
		/// <summary>
		/// The basement is unfinished
		/// </summary>
		public static BasementType Unfinished = new BasementType(3, "Unfinished");

		/// <summary>
		/// All Heating Types
		/// </summary>
        public static BasementType[] All = new BasementType[] { NotSpecified, Finished, PartlyFinished, Unfinished };

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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("BasementType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private BasementType(int value, string name)
			: base(value, name)
		{
		}

		/// <summary>
		/// Creates a basement type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A basement type instance</returns>
		public static BasementType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the heating type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the heating type</param>
		/// <returns>The basement type matching the integer value</returns>
		public static BasementType FromValue(int value)
		{
			foreach (BasementType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
