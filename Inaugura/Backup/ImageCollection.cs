using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents a collection of images
	/// </summary>
	public class ImageCollection : System.Collections.CollectionBase, Inaugura.Xml.IXmlable
	{
		#region Accessors
		/// <summary>
		/// Returns the image at the specified zero based index
		/// </summary>
		/// <param name="index">The zero based index into the collection</param>
		/// <returns>The image at the specified index</returns>
		public Image this[int index]
		{
			get
			{
				return this.List[index] as Image;
			}
		}

		/// <summary>
		/// Returns the image at the specified zero based index
		/// </summary>
		/// <param name="imageID">The ID of the image</param>
		/// <returns>The image with the specified ID, otherwise null</returns>
		public Image this[string imageID]
		{
			get
			{
                Guid guid = new Guid(imageID);
                return this[guid];
			}
		}

        public Image this[Guid imageID]
        {
            get
            {                
                foreach (Image image in this)
                {
                    if (image.ID == imageID)
                        return image;
                }
                return null;
            }
        }
		#endregion

		#region IXmlable Members
		/// <summary>
		/// The xml representation of the the instance
		/// </summary>
		public XmlNode Xml
		{
			get
			{
				XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Images");
				this.PopulateNode(node);
				return node;
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which represents the instance</param>
		public ImageCollection(XmlNode node)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ImageCollection()
		{
		}

		/// <summary>
		/// Adds a image to the collection
		/// </summary>
		/// <param name="image">The image to add</param>
		public void Add(Image image)
		{
			this.List.Add(image);
		}

		/// <summary>
		/// Removes a image from the collection
		/// </summary>
		/// <param name="image">The image to remove</param>
		public virtual void Remove(Image image)
		{
			this.List.Remove(image);
		}

		/// <summary>
		/// Moves an image up in the list
		/// </summary>
		/// <param name="image">The image in the list</param>
		public void MoveUp(Image image)
		{
			int index = this.List.IndexOf(image);
			if (index > 0)
			{
				this.List.RemoveAt(index);
				index--;
				this.List.Insert(index, image);
			}
		}

		/// <summary>
		/// Moves an image down in the list
		/// </summary>
		/// <param name="image">The image in the list</param>
		public void MoveDown(Image image)
		{
			int index = this.List.IndexOf(image);
			if (index < this.List.Count-1)
			{
				this.List.RemoveAt(index);
				index++;
				this.List.Insert(index, image);
			}
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			foreach (Image img in this)
			{
				XmlNode imgNode = node.OwnerDocument.CreateElement("Image");
				img.PopulateNode(imgNode);
				node.AppendChild(imgNode);
			}
		}

		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			XmlNodeList nodes = node.SelectNodes("Image");
			foreach (XmlNode imgNode in nodes)
			{
				Image img = new Image(imgNode);
				this.Add(img);
			}			
		}

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (Image img in this)
                hashCode ^= img.GetHashCode();

            return hashCode;
        }
		#endregion			
	}
}
