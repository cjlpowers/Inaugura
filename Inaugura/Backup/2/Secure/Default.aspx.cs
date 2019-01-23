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
using System.Drawing;

using Inaugura;
using Inaugura.RealLeads;
using ZedGraph;


public partial class Default_aspx : System.Web.UI.Page
{
    // Page events are wired up automatically to methods 
    // with the following names:
    // Page_Load, Page_AbortTransaction, Page_CommitTransaction,
    // Page_DataBinding, Page_Disposed, Page_Error, Page_Init, 
    // Page_Init Complete, Page_Load, Page_LoadComplete, Page_PreInit
    // Page_PreLoad, Page_PreRender, Page_PreRenderComplete, 
    // Page_SaveStateComplete, Page_Unload

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Secure\\Default.html");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Search.aspx?search=mylistings",true);

        if (!this.IsPostBack)
        {
            // get the listing 
			Inaugura.RealLeads.Listing listing = SessionHelper.Agent.ActiveListing;
            if (listing != null)
            {                
                this.LoadZoneDetails(listing);
                this.LoadVoiceMailDetails();
                this.LoadContactDetails();
                this.LoadActivityDetails();
            }
        }
    }

    private void LoadZoneDetails(Listing listing)
    {
        this.mLbListingNumber.Text = listing.Code;
        this.mLbExpirationDate.Text = listing.ExpirationDate.ToLongDateString();
        /*
        // Get the zones that the listing is playing in
        Inaugura.Zone[] zones = DataHelper.RealLeadsDataStore.ListingStore.GetZonesForListing(listing.ID);
        foreach (Inaugura.Zone zone in zones)
        {
            string code = DataHelper.RealLeadsDataStore.ListingStore.GetCodeForListing(listing.ID, zone.ID);
            listing.Code = code;            
            DataHelper.RealLeadsDataStore.ListingStore.Update(listing);
			this.mLbListingNumber.Text = string.Format("{0} ({1})", code, zone.Name);
            break;           
        }

        if (listing.ID == "b82ab797-8c0d-46a0-a631-c355f408cde1")
        {
            listing.Code = "1000";
            DataHelper.RealLeadsDataStore.ListingStore.Update(listing);
        }
         */
    }

    private void LoadVoiceMailDetails()
    {
        throw new NotImplementedException();
        /*
		Inaugura.RealLeads.Agent agent = SessionHelper.Agent.ActiveAgent;

        Inaugura.RealLeads.VoiceMail[] mails = DataHelper.RealLeadsDataStore.VoiceMailStore.GetVoiceMails(agent.ID);
        Inaugura.RealLeads.VoiceMailCollection messages = new VoiceMailCollection();
        messages.AddRange(mails);

		if (messages.Count > 0)
		{			
			this.mImgVoiceMail.ImageUrl = "~/Content/Images/VoiceMail_Full.gif";
		}

        int newMessages = messages.GetVoiceMailOfStatus(VoiceMail.VoiceMailStatus.New).Length;
        int oldMessages = messages.GetVoiceMailOfStatus(VoiceMail.VoiceMailStatus.Old).Length;

		if (newMessages > 0)
		{
			this.PopupWin1.Message = string.Format("You have {0} new {1}. Click here to go to your voice mail inbox...", newMessages, newMessages > 1 ? "Messages" : "Message");
			this.PopupWin1.Visible = true;
		}

        this.mLbNewMessages.Text = newMessages.ToString();
        this.mLbOldMessages.Text = oldMessages.ToString();

        if (messages.Count > 0)
        {
            VoiceMail latestMessage = messages[0];
            string callersName = "Unknown";
            if (latestMessage.CallerID != null && latestMessage.CallerID != string.Empty)
                callersName = latestMessage.CallerID;
            this.mLbLatestMessage.Text = string.Format("Message from {0} received {1}", callersName, latestMessage.Date.ToString());
        }
        else
            this.mLbLatestMessage.Text = "No Messages";
        */
    }

    private void LoadContactDetails()
    {
        throw new NotImplementedException();
        /*
		Inaugura.RealLeads.Agent agent = SessionHelper.Agent.ActiveAgent;

        // TODO handle different timezones
        Inaugura.RealLeads.ContactSchedule[] activeSchedules = agent.ContactSchedules[DateTime.Now];
        if (activeSchedules.Length == 0)
            this.mLbContactStatus.Text = string.Format("You are unavaliable. Calls will be sent to voice mail");
        else if (activeSchedules.Length == 1)
        {
            DateTime startTime = DateTime.Now.Date + activeSchedules[0].StartTime;
            DateTime stopTime = DateTime.Now.Date + activeSchedules[0].StopTime;
            this.mLbContactStatus.Text = string.Format("You can be reached at {0} from {1} until {2}", activeSchedules[0].ContactNumber, startTime.ToShortTimeString(), stopTime.ToShortTimeString());
        }
        else
        {
            this.mLbContactStatus.Text = string.Empty;
            foreach (Inaugura.RealLeads.ContactSchedule schedule in activeSchedules)
            {
                DateTime startTime = DateTime.Now.Date + schedule.StartTime;
                DateTime stopTime = DateTime.Now.Date + schedule.StopTime;
                this.mLbContactStatus.Text += string.Format("You can be reached at {0} from {1} until {2}\n", schedule.ContactNumber, startTime.ToShortTimeString(), stopTime.ToShortTimeString());
            }
        }
        */
    }

    private void LoadActivityDetails()
    {
        throw new NotImplementedException();

        /*
		Inaugura.RealLeads.Agent agent = SessionHelper.Agent.ActiveAgent;      

        CallLog[] callDetailToday = DataHelper.RealLeadsDataStore.CallLogStore.GetCalls( null, agent.ID, null, false, DateTime.Today, DateTime.Today.AddHours(24));

        if (callDetailToday.Length > 0)
        {
            // create the table
            DataTable table = new DataTable();
            table.Columns.Add("CallTime", typeof(string));
            table.Columns.Add("CallerID", typeof(string));
            table.Columns.Add("CallDuration", typeof(string));
            table.Columns.Add("AgentAccepted", typeof(bool));
            table.Columns.Add("SentToVoiceMail", typeof(bool));
            foreach (CallLog call in callDetailToday)
            {                
                table.Rows.Add(call.Time.ToLocalTime().ToString(), call.CallerID, call.Duration.ToString(), call.AgentAccepted, call.SentToVoiceMail);
            }
            this.mGridActivity.DataSource = table;
            this.mGridActivity.DataBind();
            this.mGridActivity.Visible = true;

            CallVolume[] callVolumeToday = DataHelper.RealLeadsDataStore.CallLogStore.GetAgentCallVolume(agent.ID, DateTime.Today, DateTime.Today.AddHours(24), Inaugura.RealLeads.Volume.Resolution.Hour);
            Volume[] webVolumeToday = DataHelper.RealLeadsDataStore.WebLogStore.GetAgentWebVolume(agent.ID, DateTime.Today, DateTime.Today.AddHours(24), Inaugura.RealLeads.Volume.Resolution.Hour);

            //create the graph
            Inaugura.RealLeads.Graphing.VolumeGraph graph = new Inaugura.RealLeads.Graphing.VolumeGraph(callVolumeToday, webVolumeToday, DateTime.Today, DateTime.Today.AddDays(1), new TimeSpan(1, 0, 0));
            graph.Title = "Todays Activity";
            graph.XAxisTitle = "Time (EST)";
            System.Drawing.Image img = graph.CreateImage(574, 300, System.Drawing.Imaging.ImageFormat.Png);
            string imgID = Guid.NewGuid().ToString();
            this.mImgActivity.ImageUrl = "~/ImageHandler.ashx?id=" + imgID;
            this.Session.Add(imgID, img);
        }
        else
            this.mGridActivity.Visible = false;

        this.mImgActivity.Visible = this.mGridActivity.Visible;
        this.mLbNoCallsToday.Visible = !this.mGridActivity.Visible;
         */

    }

	protected void PopupWin1_LinkClicked(object sender, EventArgs e)
	{
		this.Response.Redirect("~/Secure/Inbox.aspx");
	}
}

