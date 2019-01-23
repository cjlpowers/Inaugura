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

public partial class Default_aspx : System.Web.UI.Page
{
    // Page events are wired up automatically to methods 
    // with the following names:
    // Page_Load, Page_AbortTransaction, Page_CommitTransaction,
    // Page_DataBinding, Page_Disposed, Page_Error, Page_Init, 
    // Page_Init Complete, Page_Load, Page_LoadComplete, Page_PreInit
    // Page_PreLoad, Page_PreRender, Page_PreRenderComplete, 
    // Page_SaveStateComplete, Page_Unload

    private string Action
    {
        get
        {
            if (this.Request.Params["action"] != null)
                return this.Request.Params["action"].ToLower();
            else
                return null;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {        
        this.Master.SetBodyContent("Default.html");
        this.mTxtSearch.Attributes["onKeyPress"] = string.Format("if ((event.which ? event.which : event.keyCode)==13) {{document.getElementById('{0}').click(); return false;}}", this.mBtnSearch.ClientID);

        if (this.Action == "logout")
        {
            Helper.Session.Logout();
            this.Response.Redirect("~/Default.aspx");
        }
        else if (this.Action == "timeout")
        {
            Helper.Session.Logout();
            this.Response.Redirect("~/Content.aspx?target=sessiontimeout");
        }
        else if (this.Action == "paid")
        {
            Helper.Session.Logout();
            this.Response.Redirect("~/Content.aspx?target=activation");
        }

        Inaugura.RealLeads.Listing featuredListing = GetFeaturedListing();
        this.ListingViewPanel1.Listing = featuredListing;
        this.ListingViewPanel1.Mode = "Feature";

        if (!this.IsPostBack)
        {
            this.mDlSchool.Items.Clear();
            this.mDlSchool.Items.Add(new ListItem("Please select a school...", string.Empty));
            Inaugura.Maps.Locale[] locales = Helper.API.AddressManager.GetLocales(Inaugura.Maps.Locale.LocaleType.AllEducation);
            foreach (Inaugura.Maps.Locale locale in locales)
                this.mDlSchool.Items.Add(new ListItem(locale.Name, locale.ID));
        }
    }

    protected void mBtnSearch_Click(object sender, EventArgs e)
    {

        if (this.mTxtSearch.Text == string.Empty)
        {
            this.Response.Redirect("~/Search.aspx");
            return;
        }

        this.mLbSearchError.Text = string.Empty;
           

        // if the search text is all numeric then treat it as a listing number
        bool allNumeric = true;
        foreach (char c in this.mTxtSearch.Text)
        {
            if (!char.IsNumber(c))
            {
                allNumeric = false;
                break;
            }
        }

        if (allNumeric)
        {
            int code = 0;
            if (int.TryParse(this.mTxtSearch.Text, out code))
            {
                Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListingByCode(this.mTxtSearch.Text);
                if (listing != null)
                {
                    this.Response.Redirect(string.Format("~/Listing.aspx?id={0}", listing.ID), true);
                    return;
                }
                else
                {
                    this.mLbSearchError.Text = "The listing could not be found";
                    this.mUpdateSearch.Update();
                    return;
                }
            }
        }

        Inaugura.RealLeads.PropertySearch search = null;
        if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RealEstateListings)
            search = new Inaugura.RealLeads.RealEstateSearch();
        else if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
            search = new Inaugura.RealLeads.RentalPropertySearch();
        else
            throw new NotSupportedException("The website mode was not supported");

        search.Status = Inaugura.RealLeads.Listing.ListingStatus.Active;
        
        if (Inaugura.Validation.RegexPostalCode.IsMatch(this.mTxtSearch.Text))
        {
            Inaugura.Maps.Address address = Helper.API.AddressManager.LocatePostal(this.mTxtSearch.Text.Replace(" ", ""));
            if (address == null)
            {
                this.mLbSearchError.Text = "The postal code you entered could not be located";
                this.mUpdateSearch.Update();
                return;
            }
            search.Address.Label = address.Label;
            search.Address.Latitude = address.Latitude;
            search.Address.Longitude = address.Longitude;       
        }
        else
        {
            string errorMsg = string.Empty;
            Inaugura.Maps.Address address = GeocodeHelper.Geocode(this.mTxtSearch.Text, out errorMsg);
            if (address == null || address.Latitude == 0)
            {
                this.mLbSearchError.Text = errorMsg;
                return;
            }                  
            search.Address.Latitude = address.Latitude;
            search.Address.Longitude = address.Longitude;         
        }

        CachedSearch cachedSearch = new CachedSearch(search);
        Helper.Session.Search = cachedSearch;
        this.Response.Redirect(string.Format("~/Search.aspx?search={0}", cachedSearch.Key),true);
    }


    private Inaugura.RealLeads.Listing GetFeaturedListing()
    {
        Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetFeaturedListing(typeof(Inaugura.RealLeads.RentalPropertyListing));
        return listing;
    }

    protected void mDlSchool_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Response.Redirect(this.ResolveUrl(string.Format("~/Search.aspx?locale={0}", this.mDlSchool.SelectedValue)), true);
    }
}