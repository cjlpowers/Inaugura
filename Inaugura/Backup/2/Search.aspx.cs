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

public partial class Search : System.Web.UI.Page
{
    #region Variables
    private const int PageSize = 4;
    #endregion

    #region Properties
    public string SearchKey
    {
        get
        {
            return this.Request.Params["search"];
        }
    }

    public int PageIndex
    {
        get
        {
            string index = this.Request.Params["pindex"];
            int page = 1;
            if (!int.TryParse(index, out page))
                return 1;
            return page;
        }
    }
    #endregion

    private bool IsSearch
    {
        get
        {
            return (this.Request.Params["search"] != null);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {        
        if (this.IsSearch)
        {
            // is this a my listings search
            if (this.Request.Params["search"].ToLower() == "mylistings" && SessionHelper.User != null)
            {
                Inaugura.RealLeads.ListingSearch search;
                if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RealEstateListings)
                    search = new Inaugura.RealLeads.RealEstateSearch();
                else if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
                    search = new Inaugura.RealLeads.RentalPropertySearch();
                else
                    throw new NotSupportedException("The web site mode was not supported");
                
                search.UserID = SessionHelper.User.ID;                
                this.ExecuteSearch(search);              
            }
            this.Master.SetBodyContent("ListingSearchResults.html");
        }
        else
        {
            this.Master.SetBodyContent("ListingSearch.html");
        }
        if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RealEstateListings)
            PlaceHolder1.Controls.Add(Page.LoadControl("~/Controls/Components/RealEstateSearchPanel.ascx"));
        else if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
            PlaceHolder1.Controls.Add(Page.LoadControl("~/Controls/Components/RentalPropertySearchPanel.ascx"));
        else
            throw new NotSupportedException("The web site mode was not supported");            
    }
    
    private void ExecuteSearch(Inaugura.RealLeads.ListingSearch search)
    {
        CachedSearch cachedSearch = new CachedSearch(search);
        Helper.Session.Search = cachedSearch;
        
        if (cachedSearch.Search.CalculateResultCount)
            cachedSearch.GetResults(1, 20);

        if (cachedSearch.Search.ResultCount == 1)
            this.Response.Redirect(string.Format("~/Listing.aspx?id={0}", cachedSearch.CachedResults[0].ID));
        else
            this.Response.Redirect(string.Format("~/Search.aspx?search={0}", cachedSearch.Key));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(Helper.Request.SearchKey != null)
        {
            CachedSearch search = Helper.Session.Search;
            if (search != null)
            {
                if(search.Search.CalculateResultCount)
                    search.GetResults(1, 20);

                if (search.Search.ResultCount > 0)
                {
                    this.mSearchResultsPanel.SetSearch(search, this.PageIndex);
                    this.mSearchResultsPanel.Visible = true;
                    this.PlaceHolder1.Visible = false;
                    this.mNoResults.Visible = false;
                }
                else
                {
                    this.mSearchResultsPanel.Visible = false;
                    this.mNoResults.Visible = true;
                }
                return;
            }
        }
        this.mSearchResultsPanel.Visible = false;
        this.PlaceHolder1.Visible = true;
    }

    private Inaugura.RealLeads.Listing[] GetListings()
    {
        if (SearchKey == null)
            return new Inaugura.RealLeads.Listing[0];

        CachedSearch search = Helper.Session.Search;
        if (search == null)
            return new Inaugura.RealLeads.Listing[0];

        int startIndex = (this.PageIndex - 1) * Search.PageSize;
        int endINdex = startIndex + Search.PageSize - 1;

        return search.GetResults(startIndex, endINdex);
    }
}
