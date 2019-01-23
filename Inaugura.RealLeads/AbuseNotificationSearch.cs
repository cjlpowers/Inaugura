using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class which represents an abuse notification search
    /// </summary>
    public class AbuseNotificationSearch : Search
    {
        #region Variables
        private Guid mListingID;
        private Guid mUserID;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private SearchCriteria mAddress;
        #endregion

        #region Properties
        /// <summary>
        /// The listing ID
        /// </summary>
        public Guid ListingID
        {
            get
            {
                return this.mListingID;
            }
            set
            {
                this.mListingID = value;
            }
        }

        /// <summary>
        /// The user ID
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
        /// The start date
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return this.mStartDate;
            }
            set
            {
                this.mStartDate = value;
            }
        }

        /// <summary>
        /// The end date
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return this.mEndDate;
            }
            set
            {
                this.mEndDate = value;
            }
        }

        /// <summary>
        /// The address search criteria
        /// </summary>
        public SearchCriteria Address
        {
            get
            {
                return this.mAddress;
            }
            set
            {
                this.mAddress = value;
            }
        }
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            base.PopulateNode(node);
            if(this.mListingID != Guid.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "listingID", mListingID.ToString());
            if(this.mUserID != Guid.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "userID", mUserID.ToString());
            if(this.mStartDate != DateTime.MinValue)
                Inaugura.Xml.Helper.SetAttribute(node, "startDate", StartDate.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if(this.mEndDate != DateTime.MaxValue)
                Inaugura.Xml.Helper.SetAttribute(node, "endDate", EndDate.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if(!string.IsNullOrEmpty(this.mAddress.SearchValue))
                Inaugura.Xml.Helper.SetAttribute(node, "address", this.Address.ToString());
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

            if (Inaugura.Xml.Helper.AttributeExists(node, "listingID"))
                this.ListingID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "listingID"));
            if (Inaugura.Xml.Helper.AttributeExists(node, "userID"))
                this.UserID = new Guid(Inaugura.Xml.Helper.GetAttribute(node, "userID"));
            if (Inaugura.Xml.Helper.AttributeExists(node, "startDate"))
                this.StartDate = Convert.ToDateTime(Inaugura.Xml.Helper.GetAttribute(node, "startDate"));
            if (Inaugura.Xml.Helper.AttributeExists(node, "endDate"))
                this.EndDate = Convert.ToDateTime(Inaugura.Xml.Helper.GetAttribute(node, "endDate"));
            if (Inaugura.Xml.Helper.AttributeExists(node, "address"))
                this.mAddress = SearchCriteria.Parse(Inaugura.Xml.Helper.GetAttribute(node, "address"));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public AbuseNotificationSearch()
        {
            this.mListingID = Guid.Empty;
            this.mUserID = Guid.Empty;
            this.mStartDate = DateTime.MinValue;
            this.mEndDate = DateTime.MaxValue;
            this.mAddress = new SearchCriteria();
        }
        #endregion
    }
}
