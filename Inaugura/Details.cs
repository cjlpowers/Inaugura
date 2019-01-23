using System;
using System.Collections;
using System.Xml;

using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Inaugura
{
	/// <summary>
	/// Summary description for UserDetails.
	/// </summary>
	[Editor(typeof(DetailsEditor), typeof(UITypeEditor))]
	public sealed class Details : Inaugura.Xml.IXmlable
	{
		#region Variables
		private Hashtable mHashtable = new Hashtable();
		#endregion

		#region Properties
		/// <summary>
		/// The numder of details in the collection
		/// </summary>
		public int Count
		{
			get
			{
				return this.mHashtable.Count;
			}
		}
		#endregion

		#region IXmlable Members
		/// <summary>
		/// Gets the xml representation of the Details
		/// </summary>
		/// <value></value>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Details");
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
			IDictionaryEnumerator enumerator = this.GetEnumerator();
			while (enumerator.MoveNext())
			{
				XmlNode detailNode = node.OwnerDocument.CreateElement("Detail");
				Inaugura.Xml.Helper.SetAttribute(detailNode, "key", enumerator.Key.ToString());
				detailNode.InnerText = enumerator.Value.ToString();
				node.AppendChild(detailNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node"></param>
		public void PopulateInstance(XmlNode node)
		{
			this.Clear();
			XmlNodeList nodes = node.SelectNodes("Detail");
			foreach (XmlNode detailNode in nodes)
			{
				if (detailNode.Attributes["key"] == null)
					throw new Exception("The detail node does not contain a key attribute");

				this.Add(Inaugura.Xml.Helper.GetAttribute(detailNode, "key"), detailNode.InnerText);
			}
		}
		#endregion


        #region Methods
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="detailsNode">The Xml Node which defines the Details</param>
		/// <param name="initializeXmlNode">Determines if the xml node will be populated with the objects required xml structure</param>
		public Details(XmlNode detailsNode) : this()
		{
			this.PopulateInstance(detailsNode);
		}

		/// <summary>
		/// Constructor
		/// </summary>		
		public Details()
		{
		}

		/// <summary>
		/// Initializes the xml node with the objects required structure
		/// </summary>
		/// <param name="node">The xml node</param>
		private static void InitializeXmlNode(XmlNode node)
		{
			// no specific items required		
		}

        public override int GetHashCode()
        {
            int hashCode = 0;
            
            IDictionaryEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key != null)
                    hashCode ^= enumerator.Key.GetHashCode();
                if (enumerator.Value != null)
                    hashCode ^= enumerator.Value.GetHashCode();
            }
            return hashCode;
        }

		#region IDictionary Members
		/// <summary>
		/// Checks for the existance of a key
		/// </summary>
		/// <param name="key">The key value</param>
		/// <returns>True if the key exists in the collection, otherwise false</returns>
		public bool ContainsKey(string key)
		{
			return this.mHashtable.ContainsKey(key);
		}

		/// <summary>
		/// Adds a new key/value pair to the of details
		/// </summary>
		/// <param name="key">The key value</param>
		/// <param name="value">The value</param>
		public void Add(string key, string value)
		{
			if (this.mHashtable.ContainsKey(key))
			{
				this.mHashtable[key] = value;
			}
			else
			{
				this.mHashtable.Add(key, value);
			}			
		}

		/// <summary>
		/// Clears all key/value pairs from the collection of details
		/// </summary>
		public void Clear()
		{
			this.mHashtable.Clear();
		}
		
		/// <summary>
		/// Removes a specific key from the collection of details
		/// </summary>
		/// <param name="key">The key to remove</param>
		public void Remove(string key)
		{
			this.mHashtable.Remove(key);			
		}
		
		/// <summary>
		/// Gets an Enumerator used to interate through the collection
		/// </summary>
		/// <returns></returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return this.mHashtable.GetEnumerator();
		}

		/// <summary>
		/// Gets/Sets a detail. If the key does not exist it will be created
		/// </summary>
		/// <param name="key">The detail key</param>
		/// <returns>The detail value</returns>
		public string this[string key]
		{
			get
			{
				return this.mHashtable[key] as string;
			}
			set
			{
				this.mHashtable[key] = value;
			}
		}
		#endregion		
        #endregion
    }
}
