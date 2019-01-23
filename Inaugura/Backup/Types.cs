using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public abstract class Types
	{
		#region Variables
		private int mValue;
		private string mName;
		#endregion

		#region Properties
		/// <summary>
		/// An interger value representing the roof material
		/// </summary>
		public int Value
		{
			get
			{
				return this.mValue;
			}
		}

		/// <summary>
		/// The string representation of the roof material
		/// </summary>
		public string Name
		{
			get
			{
				return this.mName;
			}
		}	
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">The value</param>
		/// <param name="name">The name</param>
		protected Types(int value, string name)
		{
			this.mName = name;
			this.mValue = value;
		}
		
		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
            node.InnerText = this.mValue.ToString();
			//Inaugura.Xml.Helper.SetAttribute(node, "value", this.mValue.ToString());
		}

		/// <summary>
		/// Gets the type value from the xml
		/// </summary>
		/// <param name="xml">The xml which represents the item</param>
		/// <returns>The type value</returns>
		internal static int GetTypeValue(XmlNode xml)
		{
			//if (xml.Attributes["value"] == null)
			//	throw new ArgumentException("The xml node does not contain a value attribute");
            if (xml.Attributes["value"] != null)
            {
                int value = int.Parse(Inaugura.Xml.Helper.GetAttribute(xml, "value"));
                return value;
            }
            int val = int.Parse(xml.InnerText);
            return val;
		}

		/// <summary>
		/// The name of the type
		/// </summary>
		/// <returns>The name of the type</returns>
		public override string ToString()
		{
			return this.Name;
		}

        public override int GetHashCode()
        {
            return this.mValue.GetHashCode();
        }
	}
}
