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

public partial class ListingSearchResultPanel : System.Web.UI.UserControl
{
    public void SetSearch(CachedSearch search, int pageIndex)
    {
        int pageSize = Helper.Configuration.SearchPageSize;
        int startIndex = (pageIndex - 1) * pageSize+1;
        int endIndex = startIndex + pageSize - 1;        
       
        this.PopulateControl(search.GetResults(startIndex,endIndex));

        this.ListingSearchPagePanel1.SearchKey = search.Key;
        this.ListingSearchPagePanel1.ResultCount = search.Search.ResultCount;
        this.ListingSearchPagePanel1.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(search.Search.ResultCount) / Convert.ToDouble(pageSize)));
        this.ListingSearchPagePanel1.PageIndex = pageIndex;
        this.ListingSearchPagePanel1.PageSize = pageSize;

        this.ListingSearchPagePanel2.SearchKey = this.ListingSearchPagePanel1.SearchKey;
        this.ListingSearchPagePanel2.ResultCount = this.ListingSearchPagePanel1.ResultCount;
        this.ListingSearchPagePanel2.PageCount = this.ListingSearchPagePanel1.PageCount;
        this.ListingSearchPagePanel2.PageIndex = this.ListingSearchPagePanel1.PageIndex;
        this.ListingSearchPagePanel2.PageSize = this.ListingSearchPagePanel1.PageSize;
    }  
        
    private void PopulateControl(Inaugura.RealLeads.Listing[] listings)
    {
        this.mRepeater.DataSource = listings;
        this.mRepeater.DataBind();
    }

    protected string GetContent(Inaugura.RealLeads.Listing listing)
    {
        return WebServices.ListingService.TransformListing(listing, "SearchResult");
    }
}
