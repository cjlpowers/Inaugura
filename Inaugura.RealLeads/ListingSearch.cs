using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which holds the listing search criteria
	/// </summary>
    public class ListingSearch : Search
	{
		#region Variables
        private Listing.ListingStatus mStatus;
        private Guid mUserID;
        private DateTime mExpirationStart;
        private DateTime mExpirationEnd;
		#endregion

		#region Properties
         /// <summary>
        /// The status of the listing
        /// </summary>
        public Listing.ListingStatus Status
        {
            get
            {
                return this.mStatus;
            }
            set
            {
                this.mStatus = value;
            }
        }

        /// <summary>
        /// The id of the user who owns the listing(s)
        /// </summary>
        public Guid UserID
        {
            get
            {
                return this.mUserID;
            }
            set
            {
                this.mUserID = value;
            }
        }

        /// <summary>
        /// The expiration start
        /// </summary>
        public DateTime ExpirationStart
        {
            get
            {
                return this.mExpirationStart;
            }
            set
            {
                this.mExpirationStart = value;
            }
        }

        /// <summary>
        /// The expiration end
        /// </summary>
        public DateTime ExpirationEnd
        {
            get
            {
                return this.mExpirationEnd;
            }
            set
            {
                this.mExpirationEnd = value;
            }
        }
        #endregion
              
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ListingSearch()
        {
            this.mUserID = Guid.Empty;
            this.mStatus = Listing.ListingStatus.All;
            this.mExpirationStart = DateTime.MinValue;
            this.mExpirationEnd = DateTime.MaxValue;
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node);               
                       
            Inaugura.Xml.Helper.SetAttribute(node, "status", ((int)this.Status).ToString());

            if (this.UserID != null && this.UserID != Guid.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "userId", this.mUserID.ToString());
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

             if (node.Attributes["status"] != null)
                this.Status = (Listing.ListingStatus)Enum.Parse(typeof(Listing.ListingStatus),node.Attributes["status"].Value);

            if (node.Attributes["userId"] != null)
                this.mUserID = new Guid(node.Attributes["userId"].Value);            
        }       
        #endregion
    }
}
