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

public partial class Controls_Components_SearchScroller : System.Web.UI.UserControl, IPostBackEventHandler
{
    #region Events    
    public delegate void ListingSelectedHandler(Guid listingID);
    public event ListingSelectedHandler ListingSelected;

    private void OnListingSelected(Guid id)
    {
        if (this.ListingSelected != null)
            this.ListingSelected(id);
    }
    #endregion

    #region Variables
    private const int ResultSetSize = 20;
    private string mSearchKey;
    private string mInitialResults;
    #endregion

    #region Properties    
    protected string SearchKey
    {
        get
        {
            return this.mSearchKey;
        }
    }

    public string InitialResults
    {
        get
        {
            return this.mInitialResults;
        }
    }

    public double SearchLongitude
    {
        get
        {
            if (Helper.Session.Search != null)
            {
                Inaugura.RealLeads.PropertySearch search = Helper.Session.Search.Search as Inaugura.RealLeads.PropertySearch;
                if (search != null)
                    return search.Address.Longitude;
            }
            return 0;
        }
    }

    public double SearchLatitude
    {
        get
        {
            if (Helper.Session.Search != null)
            {
                Inaugura.RealLeads.PropertySearch search = Helper.Session.Search.Search as Inaugura.RealLeads.PropertySearch;
                if (search != null)
                    return search.Address.Latitude;
            }
            return 0;
        }
    } 
    #endregion

    #region Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Visible)
        {
            //ScriptHelper.RegisterScriptReference(this.Page, "~/ScriptLibrary/scroll.js");            
            System.Web.UI.ScriptManager scriptManager = System.Web.UI.ScriptManager.GetCurrent(Page);
            System.Web.UI.ScriptReference sref = new System.Web.UI.ScriptReference("~/ScriptLibrary/scroll.js");
            scriptManager.Scripts.Add(sref);
        }
    }

    public void SetSearch(CachedSearch search)
    {
        this.Visible = false;
        if (search != null)
        {
            this.mSearchKey = search.Key;
            if (search.Search.ResultCount > 1)
                this.Visible = true;

            this.mInitialResults = this.GetInitalResultSet(search);
        }
    }

    private string GetInitalResultSet(CachedSearch search)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder();        
        string[] results = WebServices.ListingService.GetSearchResults(search, 1, ResultSetSize, "Mini");
        if (results.Length > 4)
        {
            searchLeft.Attributes["onclick"] = "myNewEffect.ScrollLeft();";
            searchRight.Attributes["onclick"] = "myNewEffect.ScrollRight();";
        }

        foreach (string s in results)
            str.Append(s);
        return str.ToString();            
    }

    #region IPostBackEventHandler Members

    public void RaisePostBackEvent(string eventArgument)
    {
        Guid id = new Guid(eventArgument);
        this.OnListingSelected(id);
    }
    #endregion

    #endregion
}
