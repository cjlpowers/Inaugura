#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Xml
{
    /// <summary>
    /// An xml helper class
    /// </summary>
	public class Helper
	{
        /// <summary>
        /// Gets the document node 
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The document node</returns>
        public static XmlNode GetDocumentNode(string fileName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            return xml.DocumentElement;
        }


		/// <summary>
		/// Creates an instance of IXmlable using the Xml Node, and the base dll and base type
		/// </summary>
		/// <param name="node">The xml node</param>
		/// <returns></returns>
		public static IXmlable GetIXmlableFromXml(XmlNode xml)
		{
			if (xml != null)
			{
				if (xml is XmlDocument)
					xml = ((XmlDocument)xml).DocumentElement;				

                    string strType = Helper.GetTypeAttribute(xml);
                    if(strType == null)
                        throw new ArgumentException("xml node must contain an attribute named 'type' which contains the runtime type information");                    
                    else
                    {
                        Type type = null;
                        #region For backwards compatibility
                        if (xml.Attributes["assembly"] != null)
                        {
					        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(xml.Attributes["assembly"].Value);
                            type = assembly.GetType(strType);
                        }
                        #endregion
                        else
                            type = Type.GetType(strType);
                        IXmlable obj = null;
                        try
                        {
                            obj = (IXmlable)System.Activator.CreateInstance(type, new object[] { xml });
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Failed to create object from xml '{0}'", xml.OuterXml), ex);
                        }
                        return obj;
                    }			
			}
			else
				return null;
    	}

        /// <summary>
        /// Gets an xml attribute value
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="attributeName">The attribute</param>
        /// <returns>The attribute value if found, otherwise null</returns>
        public static string GetAttribute(XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes[attributeName];
            if (attribute != null)
                return attribute.Value;
            return null;                
        }

        /// <summary>
        /// Gets an xml attribute value
        /// </summary>
        /// <param name="xmlNode">The node</param>
        /// <param name="xpath">The xml path</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns>The attribute value if found, otherwise null</returns>
		public static string GetAttribute(XmlNode xmlNode, string xpath, string attributeName)
		{
			XmlNode node = xmlNode.SelectSingleNode(xpath);
			return Inaugura.Xml.Helper.GetAttribute(node, attributeName);
		}

        /// <summary>
        /// Sets an xml attribute value
        /// </summary>
        /// <param name="xmlNode">The xml node</param>
        /// <param name="attribiteName">The attribute name</param>
        /// <param name="attributeValue">The attribute value</param>
		public static void SetAttribute(XmlNode xmlNode, string attribiteName, string attributeValue)
		{
			if (xmlNode != null && xmlNode.Attributes[attribiteName] != null)
				xmlNode.Attributes[attribiteName].Value = attributeValue;
			else
			{
				XmlAttribute attrib = xmlNode.OwnerDocument.CreateAttribute(attribiteName);
				attrib.Value = attributeValue;
				xmlNode.Attributes.Append(attrib);
			}
		}

        /// <summary>
        /// Sets an xml attribute value
        /// </summary>
        /// <param name="xmlNode">The xml node</param>
        /// <param name="xpath">The xml path</param>
        /// <param name="attribiteName">The attribute name</param>
        /// <param name="attributeValue">The attribute value</param>
		public static void SetAttribute(XmlNode xmlNode, string xpath, string attribiteName, string attributeValue)
		{
			XmlNode node = xmlNode.SelectSingleNode(xpath);
			Inaugura.Xml.Helper.SetAttribute(node, attribiteName, attributeValue);
		}

        /// <summary>
        /// Set the type attribute
        /// </summary>
        /// <param name="xmlNode">The xml node</param>
        /// <param name="type">The type</param>
		public static void SetTypeAttribute(XmlNode xmlNode, Type type)
		{
			SetAttribute(xmlNode, "type", type.AssemblyQualifiedName);
		}

        /// <summary>
        /// Gets the type attribute
        /// </summary>
        /// <param name="xmlNode">The xml node</param>
        /// <returns>The type attribute value</returns>
		public static string GetTypeAttribute(XmlNode xmlNode)
		{
			return GetAttribute(xmlNode, "type");
		}
	
		/// <summary>
		/// Creates a new document containing a node with a specified name
		/// </summary>
		/// <param name="nodeName">The name of the node to create</param>
		/// <returns>The created node</returns>
		public static XmlNode NewNodeDocument(string nodeName)
		{
			XmlDocument xmlDoc = new XmlDocument();
			XmlNode node = xmlDoc.CreateElement(nodeName);
			xmlDoc.AppendChild(node);
			return node;
		}

        /// <summary>
        /// Determins if a attribute exits
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="attributeName">The attribute name</param>
        /// <returns>True if the attribute exists, false otherwise</returns>
        public static bool AttributeExists(XmlNode node, string attributeName)
        {
            return (node.Attributes[attributeName] != null);
        }

        /// <summary>
        /// Determins if a node exists
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="nodeName">The node name</param>
        /// <returns>True if the node exists, false otherwise</returns>
        public static bool NodeExists(XmlNode node, string nodeName)
        {
            return (node[nodeName] != null);
        }

        /// <summary>
        /// Throws an exception if the node does not contain a child with a give name
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="name">The node name</param>
        public static void EnsureNodeExists(XmlNode node, string name)
        {
            if(!NodeExists(node,name))
                throw new Inaugura.Xml.XmlMissingException(string.Format("The node '{0}' was not present in the xml",name));
        }

        /// <summary>
        /// Throws an exception if the node does not contain an attribute with a give name
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="name">The attribute name</param>
        public static void EnsureAttributeExists(XmlNode node, string name)
        {
            if(!AttributeExists(node, name))
                throw new Inaugura.Xml.XmlMissingException(string.Format("The attribute '{0}' was not present in the xml", name));
        }        
	}
}
