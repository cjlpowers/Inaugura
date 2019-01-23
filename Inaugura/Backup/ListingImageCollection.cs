using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	public class ListingImageCollection : ImageCollection
	{
		#region Variables
		private Guid mDefaultImageID;
		#endregion

		#region Properties
		public Image DefaultImage
		{
			get
			{
                Image img = this[this.mDefaultImageID.ToString()];
                if (img != null)
                    return img;
                else if (this.Count > 0)
                    return this[0];
                else
                    return null;
			}
			set
			{
				if (value != null)
				{
					if (!this.List.Contains(value))
						throw new Exception("The list must contain the image");
				}
                this.mDefaultImageID = value.ID;
			}
		}
		#endregion

		public ListingImageCollection() : base()
		{
		}

		public ListingImageCollection(XmlNode node) : base(node)
		{
		}		

		public override void PopulateInstance(XmlNode node)
		{
			base.PopulateInstance(node);

			if (node.Attributes["defaultImage"] != null)
			{
				// see if the image id is in the list
                this.mDefaultImageID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "defaultImage"));				
			}
		}

		public override void PopulateNode(XmlNode node)
		{
			if (this.DefaultImage != null)
                Inaugura.Xml.Helper.SetAttribute(node, "defaultImage", this.DefaultImage.ID.ToString());

			base.PopulateNode(node);
		}

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            if (this.mDefaultImageID != null)
                hashCode ^= this.mDefaultImageID.GetHashCode();
            
            return hashCode;
        }

        public override void Remove(Image image)
        {
            base.Remove(image);            
        }
	}
}
