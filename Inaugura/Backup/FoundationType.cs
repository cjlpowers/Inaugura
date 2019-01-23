using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of foundations
	/// </summary>
	public class FoundationType : Types, Inaugura.Xml.IXmlable
	{
		public static FoundationType NotSpecified = new FoundationType(0, "Not Specified");
		public static FoundationType PouredConcrete = new FoundationType(1, "Poured Concrete");
		public static FoundationType Block = new FoundationType(2, "Block");
		public static FoundationType Stone = new FoundationType(3, "Stone");
		public static FoundationType Post = new FoundationType(4, "Post");

		public static FoundationType[] All = new FoundationType[] { NotSpecified, PouredConcrete, Block, Stone, Post};
				
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

		private FoundationType(int value, string name) : base(value,name)
		{
		}
		
		/// <summary>
		/// Creates a FloorMaterial instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A Listing instance</returns>
		public static FoundationType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the floor material associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the floor material</param>
		/// <returns>The floor material matching the integer value</returns>
		public static FoundationType FromValue(int value)
		{
			foreach (FoundationType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
