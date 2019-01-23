using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents a image
	/// </summary>
	public class Image : Inaugura.Xml.IXmlable
	{
		#region Variables
        private Guid mID;
        private Guid mFileID;
		private string mCaption;
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the image
		/// </summary>
        public Guid ID
		{
			get
			{
				return this.mID;
			}
			private set
			{
				this.mID = value;
			}
		}

		/// <summary>
		/// The ID of the file (image)
		/// </summary>
        public Guid FileID 
		{
			get
			{
				return this.mFileID;
			}
			set
			{
				this.mFileID = value;
			}
		}


		/// <summary>
		/// The description of the room
		/// </summary>
		public string Caption
		{
			get
			{
				return this.mCaption;
			}
			set
			{
                this.mCaption = value;
			}
		}		
		#endregion

		#region IXmlable Members

		public XmlNode Xml
		{
			get
			{
				XmlNode roomNode = Inaugura.Xml.Helper.NewNodeDocument("Image");
				this.PopulateNode(roomNode);
				return roomNode;
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="node">The xml node which defines the room</param>
		public Image(XmlNode node)
			: this(Guid.Empty)
		{
			this.PopulateInstance(node);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The name of the image</param>		
		public Image(Guid fileID)
		{
			this.ID = Guid.NewGuid();
			this.FileID = fileID;
		}

		/// <summary>
		/// Populates a xml node with the objects data
		/// </summary>
		/// <param name="node">The  node to populate</param>
		public virtual void PopulateNode(XmlNode node)
		{
			//Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());
			//Inaugura.Xml.Helper.SetAssemblyAttribute(node, this.GetType());

			Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
			Inaugura.Xml.Helper.SetAttribute(node, "fileId", this.FileID.ToString());

			if (!string.IsNullOrEmpty(this.mCaption))
			{
				XmlNode captionNode = node.OwnerDocument.CreateElement("Caption");
                captionNode.InnerText = this.mCaption;
                node.AppendChild(captionNode);
			}
		}


		/// <summary>
		/// Populates the instance with the specifed xml data
		/// </summary>
		/// <param name="node">The xml node</param>
		public virtual void PopulateInstance(XmlNode node)
		{
			if (node.Attributes["id"] == null)
				throw new ArgumentException("The xml does not contain a id attribute");

			if (node.Attributes["fileId"] == null)
				throw new ArgumentException("The xml does not contain a fileId attribute");

			this.ID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));
			this.FileID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "fileId"));

			if (node["Caption"] != null)
				this.mCaption = node["Caption"].InnerText;
		}

        public override int GetHashCode()
        {
            int hashCode = 0;

            if (this.mCaption != null)
                hashCode ^= this.mCaption.GetHashCode();
            if (this.mFileID != null)
                hashCode ^= this.mFileID.GetHashCode();
            if (this.mID != null)
                hashCode ^= this.mID.GetHashCode();
            return hashCode;
        }
		#endregion

	}
}
