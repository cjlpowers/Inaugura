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
using System.Xml;

public partial class ContactScheduleEditPanel : System.Web.UI.UserControl
{
    #region Variables
    private Inaugura.RealLeads.ContactSchedule mContactSchedule;
    #endregion

    #region Properties

    private Inaugura.RealLeads.ContactSchedule ViewStateSchedule
    {
        get
        {
            if (this.ViewState["xml"] != null)
            {
                string xml = this.ViewState["xml"] as string;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return new Inaugura.RealLeads.ContactSchedule(xmlDoc.DocumentElement);
            }
            else
                return null;
        }
        set
        {
            this.ViewState["xml"] = value.Xml.OuterXml;
        }
    }

    public Inaugura.RealLeads.ContactSchedule Schedule
    {
        get
        {
            Inaugura.RealLeads.ContactSchedule schedule = this.ViewStateSchedule;
            if (schedule != null)
                this.PopulateSchedule(schedule);

            return schedule;
        }
        set
        {
            if (value == null)
                return;

            this.ViewStateSchedule = value;
            this.PopluateControl(value);
        }
    }

    public TimeSpan TimeUpperLimit
    {
        get
        {
            if (this.ViewState["timeUpperLimit"] != null)
                return TimeSpan.FromMinutes(int.Parse(this.ViewState["timeUpperLimit"].ToString()));
            else
                return TimeSpan.FromHours(24);
        }
        set
        {
            this.ViewState["timeUpperLimit"] = Convert.ToInt32(value.TotalMinutes).ToString();
        }
    }
  
    public TimeSpan TimeLowerLimit
    {
        get
        {
            if (this.ViewState["timeLowerLimit"] != null)
                return TimeSpan.FromMinutes(int.Parse(this.ViewState["timeLowerLimit"].ToString()));
            else
                return TimeSpan.FromHours(0);
        }
        set
        {
            this.ViewState["timeLowerLimit"] = Convert.ToInt32(value.TotalMinutes).ToString();
        }
    }


    public bool AllowDateChange
    {
        get
        {
            if (this.mAllowDateChange.Value == null || this.mAllowDateChange.Value == string.Empty)
                return true;
            else
                return bool.Parse(this.mAllowDateChange.Value);
        }
        set
        {
            this.mAllowDateChange.Value = value.ToString();
        }
    }
    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {        
    }

    void mTimeStop_SelectedTimeChanged(object sender, EventArgs e)
    {
        Inaugura.RealLeads.ContactSchedule schedule = this.Schedule;
        if (schedule != null && this.mDlVoiceMailRings.SelectedValue != null)
        {
            schedule.StopTime = this.mTimeStop.Time;
            this.Schedule = schedule;
        }
    }

    void mTimeStart_SelectedTimeChanged(object sender, EventArgs e)
    {
        Inaugura.RealLeads.ContactSchedule schedule = this.Schedule;
        if (schedule != null && this.mDlVoiceMailRings.SelectedValue != null)
        {
            schedule.StopTime = this.mTimeStart.Time;
            this.Schedule = schedule;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {       
        this.mDateRow.Visible = this.AllowDateChange;
        this.mDays.Enabled = this.AllowDateChange;
    }

    private void PopluateControl(Inaugura.RealLeads.ContactSchedule schedule)
    {
        if (schedule == null)
            return;

        this.mTxtName.Text = schedule.Name;
        this.mTimeStart.StartTime = this.TimeLowerLimit;
        this.mTimeStart.Time = (schedule.StartTime > this.TimeLowerLimit)? schedule.StartTime : this.TimeLowerLimit;
        this.mTimeStart.StopTime = (schedule.StopTime < this.TimeUpperLimit)? schedule.StopTime: this.TimeUpperLimit;
        this.mTimeStop.Time = (schedule.StopTime < this.TimeUpperLimit) ? schedule.StopTime : this.TimeUpperLimit;
        this.mTimeStop.StartTime = (schedule.StartTime > this.TimeLowerLimit) ? schedule.StartTime : this.TimeLowerLimit;
        this.mTimeStop.StopTime = this.TimeUpperLimit;
        this.mTxtContactNumber.Text = schedule.ContactNumber;
        this.mDays.DaysOfWeek = schedule.Days;
        Helper.SelectListItem(this.mDlVoiceMailRings, schedule.VoiceMailRings.ToString());
        this.AllowDateChange = !(schedule.StartDate == DateTime.MinValue);

        if (this.AllowDateChange && schedule.StartDate != DateTime.MinValue && schedule.StopDate != DateTime.MaxValue)
        {
            this.mCalStartDate.SelectedDate = schedule.StartDate;
            this.mCalStopDate.SelectedDate = schedule.StopDate;
        }
    }

    public void PopulateSchedule(Inaugura.RealLeads.ContactSchedule schedule)
    {
        schedule.Name = this.mTxtName.Text;
        schedule.StartTime = this.mTimeStart.Time;
        schedule.StopTime = this.mTimeStop.Time;
        schedule.VoiceMailRings = int.Parse(this.mDlVoiceMailRings.SelectedValue);
        schedule.ContactNumber = this.mTxtContactNumber.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        schedule.Days = this.mDays.DaysOfWeek;
        
        if (this.AllowDateChange)
        {
            schedule.StartDate = this.mCalStartDate.SelectedDate;
            schedule.StopDate = this.mCalStopDate.SelectedDate;
        }
    }  
}
