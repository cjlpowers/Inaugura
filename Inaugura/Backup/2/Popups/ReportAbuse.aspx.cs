using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Popups_ReportAbuse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PopupTitle = "Report Abuse";
    }

    protected void mBtnReportAbuse_Click(object sender, EventArgs e)
    {
        // try and get the ID of the listing from the URL
        Guid id = new Guid(this.Request["listingId"]);

        // make sure the user has not already added an abuse notification today
        Inaugura.RealLeads.AbuseNotificationSearch search = new Inaugura.RealLeads.AbuseNotificationSearch();
        search.Address = new Inaugura.RealLeads.SearchCriteria(this.Request.UserHostAddress);
        search.ListingID = id;
        search.StartDate = DateTime.Now.Date;
        search.EndDate = search.StartDate.AddDays(1);

        Helper.API.ListingManager.SearchAbuseNotifications(search);
        if (search.ResultCount > 0)
        {
            Master.ClosePopup();
            return;
        }

        // try and get the listing
        Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(id);
        
        if(listing == null)
            return;

        // create a new abuse notification
        Inaugura.RealLeads.AbuseNotification abuse = new Inaugura.RealLeads.AbuseNotification(listing, this.Request.UserHostAddress);
        Helper.API.ListingManager.AddAbuseNotification(abuse);

        Master.ClosePopup();
        
    }
}
