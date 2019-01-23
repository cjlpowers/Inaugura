using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{

    /// <summary>
    /// A class which represents a collection of strings
    /// </summary>
	public class StringCollection : List<string>, Inaugura.Xml.IXmlable
	{
		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines the instance</param>
		public StringCollection(XmlNode node)
		{
			this.PopulateInstance(node);
		}


		/// <summary>
		/// Constructor
		/// </summary>
		public StringCollection()
		{
		}
                
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collection"></param>
        public StringCollection(IEnumerable<string> collection)
            : base(collection)
        {

        }

        /// <summary>
        /// Merges two collections into one single collection
        /// </summary>
        /// <param name="collection1">Collection 1</param>
        /// <param name="collection2">Collection 2</param>
        /// <returns>A collection containing the unique items from collections</returns>
        public static StringCollection Merge(StringCollection collection1, StringCollection collection2)
        {
            StringCollection collection = new StringCollection(collection1);
            foreach (string str in collection2)
                if (!collection.Contains(str))
                    collection.Add(str);

            return collection;
        }
		#endregion

		#region IXmlable Members

		/// <summary>
		/// The xml representation of this instance
		/// </summary>
		public XmlNode Xml
		{
			get 
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("StringCollection");
				this.PopulateNode(node);
				return node;
			}
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public void PopulateNode(XmlNode node)
		{
			foreach (string item in this)
			{
				XmlNode itemNode = node.OwnerDocument.CreateElement("Item");
				itemNode.InnerText = item;
				node.AppendChild(itemNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node"></param>
		public void PopulateInstance(XmlNode node)
		{
			this.Clear();
			XmlNodeList nodes = node.SelectNodes("Item");
			foreach (XmlNode itemNode in nodes)
			{
				this.Add(itemNode.InnerText);
			}
		}

        /// <summary>
        /// The hashcode method
        /// </summary>
        /// <returns>The hashcode</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (string str in this)
                hashCode ^= str.GetHashCode();
            
            return hashCode;
        }
		#endregion
}
}
