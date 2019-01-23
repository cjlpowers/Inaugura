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

public partial class ContactSchedulePanel : System.Web.UI.UserControl
{
	#region Events
	#endregion

	#region Variables
	#endregion

	#region Properties
	/// <summary>
	/// The id of the schedule
	/// </summary>
	public string ScheduleID
	{
		get
		{
			return this.ViewState["ScheduleID"] as string;
		}
		set
		{
			this.ViewState["ScheduleID"] = value;
		}
	}

    public ContactSchedule ContactSchedule
    {
        get
        {
            if (this.ViewState["contactScheduleXml"] == null)
                return null;
            else
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(this.ViewState["contactScheduleXml"] as string);
                return new Inaugura.RealLeads.ContactSchedule(xmlDoc.DocumentElement);
            }
        }
        set
        {
            this.ViewState["contactScheduleXml"] = value.Xml.OuterXml;
        }
    }

    /*
	public ContactSchedule ContactSchedule
	{
		get
		{
			return this.mSchedule;
		}
		set
		{
			if (value == null)
				return;

			this.mHeader.InnerText = value.Name;
			this.mSchedule = value;

			if ((value.Days & DaysOfWeek.Sunday) == DaysOfWeek.Sunday)
				this.mImgSun.ImageUrl = "~/Content/Images/Icons/checked.gif";
			if ((value.Days & DaysOfWeek.Monday) == DaysOfWeek.Monday)
				this.mImgMon.ImageUrl = "~/Content/Images/Icons/checked.gif";
			if ((value.Days & DaysOfWeek.Tuesday) == DaysOfWeek.Tuesday)
				this.mImgTues.ImageUrl = "~/Content/Images/Icons/checked.gif";
			if ((value.Days & DaysOfWeek.Wednesday) == DaysOfWeek.Wednesday)
				this.mImgWed.ImageUrl = "~/Content/Images/Icons/checked.gif";
			if ((value.Days & DaysOfWeek.Thursday) == DaysOfWeek.Thursday)
				this.mImgThur.ImageUrl = "~/Content/Images/Icons/checked.gif";
			if ((value.Days & DaysOfWeek.Friday) == DaysOfWeek.Friday)
				this.mImgFri.ImageUrl = "~/Content/Images/Icons/checked.gif";
			if ((value.Days & DaysOfWeek.Saturday) == DaysOfWeek.Saturday)
				this.mImgSat.ImageUrl = "~/Content/Images/Icons/checked.gif";

			this.ScheduleID = value.ID;
		}
	}
    */

    protected string Time
    {
        get
        {
            if (this.ContactSchedule == null || this.ContactSchedule.StartTime == TimeSpan.Zero || this.ContactSchedule.StopTime == TimeSpan.Zero)
                return "Not Specified";
            else
            {
                DateTime startTime = new DateTime(2000, 1, 1) + this.ContactSchedule.StartTime;
                DateTime stopTime = new DateTime(2000, 1, 1) + this.ContactSchedule.StopTime;
                return string.Format("{0} - {1}", startTime.ToShortTimeString(), stopTime.ToShortTimeString());
            }                
        }
    }

    protected string Date
    {
        get
        {
            if (this.ContactSchedule == null)
                return "Not Specified";
            else if (this.ContactSchedule.StartDate == DateTime.MinValue)
                return "All Dates";
            else
                return string.Format("{0} - {1}", this.ContactSchedule.StartDate.ToShortDateString(), this.ContactSchedule.StopDate.ToShortDateString());
        }
    }	
	
    protected string ContactNumber
	{
		get
		{
			if (this.ContactSchedule == null || this.ContactSchedule.ContactNumber == null || this.ContactSchedule.ContactNumber == string.Empty)
				return "Not Specified";
			else
				return this.ContactSchedule.ContactNumber;
		}
	}

	protected string VoiceMail
	{
		get
		{
            if (this.ContactSchedule == null)
                return "Not Specified";
            else
                return string.Format("Transfer after {0} {1}", this.ContactSchedule.VoiceMailRings, this.ContactSchedule.VoiceMailRings == 1 ? "ring" : "rings");
		}
	}
	#endregion

	private string GetTimeString(TimeSpan span)
	{
		DateTime dt = new DateTime(2000, 1, 1,0,0,0);
		dt = dt.AddHours(span.Hours);
		dt = dt.AddMinutes(span.Minutes);
		return dt.ToShortTimeString();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
        Inaugura.RealLeads.ContactSchedule schedule = this.ContactSchedule;
        this.ShowContactSchedule(schedule);
	}

    private void ShowContactSchedule(Inaugura.RealLeads.ContactSchedule schedule)
    {
        if (schedule == null)
            return;            

        this.mHeader.InnerText = schedule.Name;

        this.mImgDays.ImageUrl = Helper.Content.ContentPath("Images/DaysGrid/" + this.GetImageName(schedule.Days));           

        this.mBtnEdit.NavigateUrl = string.Format("javascript:OpenWindow('{0}','600','400','ScheduleEdit')", this.ResolveUrl(string.Format("~/Secure/PopupControls/EditSchedule.aspx?mode=edit&id={0}", this.ContactSchedule.ID)));        
        this.mLnkDelete.HRef = this.ResolveUrl(string.Format("~/Secure/ContactSchedule.aspx?delete={0}", this.ContactSchedule.ID));
    }

    private string GetImageName(Inaugura.RealLeads.DaysOfWeek days)
    {
        int val = 0;
        if ((days & DaysOfWeek.Saturday) == DaysOfWeek.Saturday)
            val += 1;
        if ((days & DaysOfWeek.Friday) == DaysOfWeek.Friday)
            val += 10;
        if ((days & DaysOfWeek.Thursday) == DaysOfWeek.Thursday)
            val += 100;
        if ((days & DaysOfWeek.Wednesday) == DaysOfWeek.Wednesday)
            val += 1000;
        if ((days & DaysOfWeek.Tuesday) == DaysOfWeek.Tuesday)
            val += 10000;
        if ((days & DaysOfWeek.Monday) == DaysOfWeek.Monday)
            val += 100000;
        if ((days & DaysOfWeek.Sunday) == DaysOfWeek.Sunday)
            val += 1000000;

        string str = val.ToString();
        return str.PadLeft(7, '0') + ".gif";
    }
    
}
