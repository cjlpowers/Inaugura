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

using Inaugura.RealLeads;

public partial class Activity_aspx : System.Web.UI.Page
{
    #region Internal Constructs
    private enum ActivityMode
    {
        All,
        User,
        Listing,
    }

    #endregion

    #region Properties 
    /// <summary>
    /// The listing ID
    /// </summary>
    private string ListingID
    {
        get
        {
            return this.Request.Params["listingId"];
        }
    }
    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Title = Helper.UI.Title("Listing Activity");        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            throw new NotImplementedException();
            /*
            // do we have an agent object
            if(SessionHelper.Agent.ActiveAgent == null)
                throw new ApplicationException("There is no agent object");

            if (this.ListingID != null)
            {

            Listing listing = DataHelper.RealLeadsDataStore.ListingStore.GetListing(this.ListingID);
             if (listing == null)
                throw new ApplicationException("The listing could not be found");

          
                if (SessionHelper.User.ID != listing.UserID)
                    throw new UnauthorizedAccessException("You are not authoorized to view this page");
                this.LoadVolume(ActivityMode.Listing, listing.ID);
            }
            else
                this.LoadVolume(ActivityMode.User, SessionHelper.User.ID);           
             */
        }
    }

    /// <summary>
    /// Loads the vall and web 
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="id"></param>
    //private void LoadVolume(ActivityMode mode, string id)
    //{  

    //    DateTime start24h = DateTime.Now.AddHours(-23);
    //    DateTime end24h = DateTime.Now.AddHours(1);
    //    DateTime start30Days = DateTime.Now.AddDays(-29);
    //    DateTime end30Days = DateTime.Now.AddDays(1);

    //    if (mode == ActivityMode.User)
    //    {
    //        CallVolume[] callVolume24hrs = DataHelper.RealLeadsDataStore.CallLogStore.GetAgentCallVolume(id, start24h.ToUniversalTime(), end24h.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Hour);
    //        CallVolume[] callVolume30Days = DataHelper.RealLeadsDataStore.CallLogStore.GetAgentCallVolume(id, start30Days.ToUniversalTime(), end30Days.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Day);
    //        Volume[] webVolume24hrs = DataHelper.RealLeadsDataStore.WebLogStore.GetAgentWebVolume(id, start24h.ToUniversalTime(), end24h.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Hour);
    //        Volume[] webVolume30Days = DataHelper.RealLeadsDataStore.WebLogStore.GetAgentWebVolume(id, start30Days.ToUniversalTime(), end30Days.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Day);
    //        CallLog[] recentCalls = DataHelper.RealLeadsDataStore.CallLogStore.GetRecentCalls(null, id, null, false, 25);
    //        this.ShowActivity(callVolume24hrs, webVolume24hrs, callVolume30Days, webVolume30Days, recentCalls);
    //    }
    //    else if (mode == ActivityMode.Listing)
    //    {
    //        CallVolume[] callVolume24hrs = DataHelper.RealLeadsDataStore.CallLogStore.GetListingCallVolume(id, start24h.ToUniversalTime(), end24h.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Hour);
    //        CallVolume[] callVolume30Days = DataHelper.RealLeadsDataStore.CallLogStore.GetListingCallVolume(id, start30Days.ToUniversalTime(), end30Days.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Day);
    //        Volume[] webVolume24hrs = DataHelper.RealLeadsDataStore.WebLogStore.GetListingWebVolume(id, start24h.ToUniversalTime(), end24h.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Hour);
    //        Volume[] webVolume30Days = DataHelper.RealLeadsDataStore.WebLogStore.GetListingWebVolume(id, start30Days.ToUniversalTime(), end30Days.ToUniversalTime(), Inaugura.RealLeads.Volume.Resolution.Day);
    //        CallLog[] recentCalls = DataHelper.RealLeadsDataStore.CallLogStore.GetRecentCalls(null, null, id, false, 25);
    //        this.ShowActivity(callVolume24hrs, webVolume24hrs, callVolume30Days, webVolume30Days, recentCalls);
    //    }
    //}

    //private void ShowActivity(CallVolume[] callVolume24hrs, Volume[] webVolume24hrs, CallVolume[] callVolume30days, Volume[] webVolume30days, CallLog[] recentCalls)
    //{
    //    Inaugura.RealLeads.Graphing.VolumeGraph graph = new Inaugura.RealLeads.Graphing.VolumeGraph(callVolume24hrs,webVolume24hrs, DateTime.Now.AddHours(-24), DateTime.Now.AddHours(1), new TimeSpan(1, 0, 0));         
    //    graph.Title = "Activity over the past 24 hours";
    //    graph.XAxisTitle = "Time (EST)";
    //    System.Drawing.Image img = graph.CreateImage(574, 300, System.Drawing.Imaging.ImageFormat.Png);
    //    string imgID = Guid.NewGuid().ToString();
    //    this.Image2.ImageUrl = "~/ImageHandler.ashx?id=" + imgID;
    //    this.Session.Add(imgID, img);

    //    graph = new Inaugura.RealLeads.Graphing.VolumeGraph(callVolume30days, webVolume30days, DateTime.Now.AddDays(-29), DateTime.Now.AddDays(1));         
    //    graph.Title = "Activity over the past 30 Days";
    //    graph.XAxisTitle = "Date";
    //    img = graph.CreateImage(574, 300, System.Drawing.Imaging.ImageFormat.Png);
    //    imgID = Guid.NewGuid().ToString();
    //    this.Image3.ImageUrl = "~/ImageHandler.ashx?id=" + imgID;
    //    this.Session.Add(imgID, img);
              
    //    DataTable table = new DataTable();
    //    table.Columns.Add("ID", typeof(string));
    //    table.Columns.Add("CallTime", typeof(string));
    //    table.Columns.Add("CallerID", typeof(string));
    //    table.Columns.Add("CallDuration", typeof(string));
    //    table.Columns.Add("AgentAccepted", typeof(bool));
    //    table.Columns.Add("SentToVoiceMail", typeof(bool));        
    //    foreach (CallLog call in recentCalls)
    //    {
    //        table.Rows.Add(call.ID,call.Time.ToLocalTime().ToString(), call.CallerID, call.Duration.ToString(), call.AgentAccepted, call.SentToVoiceMail);
    //    }
    //    this.GridView1.DataSource = table;
    //    this.GridView1.DataBind();
    //}
}
