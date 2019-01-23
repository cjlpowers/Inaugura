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

public partial class Secure_Admin_Stats : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Admin.html");
        if (!this.IsPostBack)
        {
            this.GetListingStats();
            this.mCacheKilobytesAvaliable.Text = CacheHelper.Cache.EffectivePrivateBytesLimit.ToString();
            this.mCount.Text = Cache.Count.ToString();
        }
    }

    private void GetListingStats()
    {
        
            // get the featured listing
            Inaugura.RealLeads.ListingSearch search = null;
            if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RealEstateListings)
                search = new Inaugura.RealLeads.RealEstateSearch();
            else if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
                search = new Inaugura.RealLeads.RentalPropertySearch();
            else
                throw new NotSupportedException("The website mode was not supported");

        search.CalculateResultCount = true;
        Helper.API.ListingManager.SearchListings(search);
        this.mLbTotalListings.Text = search.ResultCount.ToString();


        search.CalculateResultCount = true;
        search.Status = Inaugura.RealLeads.Listing.ListingStatus.Active;
        Helper.API.ListingManager.SearchListings(search);
        this.mLbActiveListings.Text = search.ResultCount.ToString();

        search.CalculateResultCount = true;
        search.Status = Inaugura.RealLeads.Listing.ListingStatus.Inactive;
        Helper.API.ListingManager.SearchListings(search);
        this.mLbInactiveListings.Text = search.ResultCount.ToString();

        search.CalculateResultCount = true;
        search.Status = Inaugura.RealLeads.Listing.ListingStatus.Suspended;
        Helper.API.ListingManager.SearchListings(search);
        this.mLbSuspendedListings.Text = search.ResultCount.ToString();

        search.CalculateResultCount = true;
        search.Status = Inaugura.RealLeads.Listing.ListingStatus.Sold;
        Helper.API.ListingManager.SearchListings(search);
        this.mLbSoldListings.Text = search.ResultCount.ToString();

    }
}
