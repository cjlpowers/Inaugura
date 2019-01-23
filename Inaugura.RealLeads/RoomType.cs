using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents the different types of rooms
	/// </summary>
	public class RoomType : Types, Inaugura.Xml.IXmlable
	{
		/// <summary>
		/// The room type was somthing other then thoes supported
		/// </summary>
		public static RoomType Other = new RoomType(-1, "Other");
		/// <summary>
		/// The room type is not specified
		/// </summary>
		public static RoomType NotSpecified = new RoomType(0, "Not Specified");
		/// <summary>
		/// Bathroom
		/// </summary>
		public static RoomType Bathroom = new RoomType(1, "Bathroom");
		/// <summary>
		/// Bedroom
		/// </summary>
		public static RoomType Bedroom = new RoomType(2, "Bedroom");
		/// <summary>
		/// Kitchen
		/// </summary>
		public static RoomType Kitchen = new RoomType(3, "Kitchen");
		/// <summary>
		/// Dining Room
		/// </summary>
		public static RoomType Dining = new RoomType(4, "Dining Room");
		/// <summary>
		/// Living Room
		/// </summary>
		public static RoomType Living = new RoomType(5, "Living Room");	
		/// <summary>
		/// Family Room
		/// </summary>
		public static RoomType Family = new RoomType(6, "Family Room");	
		/// <summary>
		/// Den
		/// </summary>
		public static RoomType Den = new RoomType(7, "Den");	
		/// <summary>
		/// Office
		/// </summary>
		public static RoomType Office = new RoomType(8, "Office");	
		/// <summary>
		/// Rec Room
		/// </summary>
        public static RoomType Rec = new RoomType(9, "Rec Room");	
		/// <summary>
		/// Laundry Room
		/// </summary>
		public static RoomType Laundry = new RoomType(10, "Laundry");
		/// <summary>
		/// All roofing types
		/// </summary>
		public static RoomType[] All = new RoomType[] { NotSpecified, Bathroom, Bedroom, Kitchen, Dining, Living, Family, Den, Office, Rec, Laundry, Other };
				
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
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("RoomType");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion
		#endregion

		private RoomType(int value, string name)
			: base(value, name)
		{
		}
		
		/// <summary>
		/// Creates a room type instance from an xml node representation
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>A room type instance</returns>
		public static RoomType FromXml(XmlNode xml)
		{
			return FromValue(Types.GetTypeValue(xml));
		}


		/// <summary>
		/// Gets the room type associated with a specific value
		/// </summary>
		/// <param name="value">The interger value of the room type</param>
		/// <returns>The room type matching the integer value</returns>
		public static RoomType FromValue(int value)
		{
			foreach (RoomType type in All)
			{
				if (type.Value == value)
					return type;
			}
			throw new NotSupportedException(string.Format("The type value '{0}' was not supported", value));
		}
	}
}
