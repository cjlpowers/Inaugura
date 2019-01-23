using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class representing an abuse notification
    /// </summary>
    public class AbuseNotification : Inaugura.Xml.IXmlable
    {
        #region Variables
        private Guid mID;
        private Guid mListingID;
        private Guid mUserID;
        private DateTime mDateTime;
        private string mAddress;
        #endregion

        #region Properties
        /// <summary>
        /// The ID of this object
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
        /// The ID of the user who owns the listing in question.
        /// </summary>
        public Guid UserID
        {
            get
            {
                return this.mUserID;
            }
            private set
            {
                this.mUserID = value;
            }
        }

        /// <summary>
        /// The ID of the listing in question.
        /// </summary>
        public Guid ListingID
        {
            get
            {
                return this.mListingID;
            }
            private set
            {
                this.mListingID = value;
            }
        }

        /// <summary>
        /// The date and time of the notification
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this.mDateTime;
            }
            private set
            {
                this.mDateTime = value;
            }
        }

        /// <summary>
        /// The address of the user who issued the notification
        /// </summary>
        public string Address
        {
            get
            {
                return this.mAddress;
            }
            private set
            {
                this.mAddress = value;
            }
        }
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Listing
        /// </summary>
        /// <value></value>
        public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("AbuseNotification");
                this.PopulateNode(node);
                return node;
            }
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
            if(this.mListingID != Guid.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "listingID", this.ListingID.ToString());
            if (this.mUserID != Guid.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "userID", this.UserID.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "date", this.Date.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if(!string.IsNullOrEmpty(this.mAddress))
                Inaugura.Xml.Helper.SetAttribute(node, "address", this.mAddress);
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
            this.mID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "id"));
            this.mListingID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "listingID"));
            this.mUserID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "userID"));
            this.mDateTime = Convert.ToDateTime(Inaugura.Xml.Helper.GetAttribute(node, "date"));
            this.mAddress = Convert.ToString(Inaugura.Xml.Helper.GetAttribute(node, "address"));
        }     
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="address">The address of the source of the notification</param>
        public AbuseNotification(Listing listing, string address)
        {
            this.mID = Guid.NewGuid();
            this.mListingID = listing.ID;
            this.mUserID = listing.UserID;
            this.mDateTime = DateTime.Now;
            this.mAddress = address;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xml">The xml</param>
        internal AbuseNotification(System.Xml.XmlNode xml)
        {
            this.PopulateInstance(xml);
        }
        #endregion
    }
}
