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

public partial class TimeControl : System.Web.UI.UserControl
{
    #region Events
    public event EventHandler SelectedTimeChanged;
    #endregion

    #region Variables
    private TimeSpan Interval = TimeSpan.FromMinutes(30);
    private TimeSpan mStartTime = TimeSpan.FromSeconds(0);
    private TimeSpan mStopTime = TimeSpan.FromDays(1);
    private TimeSpan mTime = TimeSpan.MinValue;
    #endregion

    #region Properties
    public Unit Width
    {
        get
        {
            return this.mDlTime.Width;
        }
        set
        {
            this.mDlTime.Width = value;
        }
    }

    public Unit Height
    {
        get
        {
            return this.mDlTime.Height;
        }
        set
        {
            this.mDlTime.Height = value;
        }
    }

    public TimeSpan StartTime
    {
        /*
        get
        {
            return this.mStartTime;
        }
        set
        {
            this.mStartTime = value;
            this.FillDropDownListWithTimes(this.mDlTime, this.StartTime, this.StopTime);
        }
         * */
        get
        {
         if (this.ViewState["startTime"] != null)
             return TimeSpan.FromMinutes(int.Parse(this.ViewState["startTime"].ToString()));
            else
                return TimeSpan.FromSeconds(0);
        }
        set
        {
            this.ViewState["startTime"] = Convert.ToInt32(value.TotalMinutes).ToString();
        }
    }

    public TimeSpan StopTime
    {
        get
        {
            if (this.ViewState["stopTime"] != null)
                return TimeSpan.FromMinutes(int.Parse(this.ViewState["stopTime"].ToString()));
            else
                return TimeSpan.FromHours(24);
        }
        set
        {
            this.ViewState["stopTime"] = Convert.ToInt32(value.TotalMinutes).ToString();
        }
        /*
        get
        {
            return this.mStopTime;
        }
        set
        {
            this.mStopTime = value;
            this.FillDropDownListWithTimes(this.mDlTime, this.StartTime, this.StopTime);
        }
         * */
    }

    public TimeSpan Time
    {
        get
        {
            if (this.ViewState["time"] != null)
                return TimeSpan.FromMinutes(int.Parse(this.ViewState["time"].ToString()));
            else
                return TimeSpan.FromSeconds(0);                
        }
        set
        {
            this.ViewState["time"] = Convert.ToInt32(value.TotalMinutes).ToString();
        }
    }

    public bool AutoPostBack
    {
        get
        {
            return this.mDlTime.AutoPostBack;
        }
        set
        {
            this.mDlTime.AutoPostBack = value;
        }
    }
    #endregion
    
    protected void Page_Init(object sender, EventArgs e)
    {        
            this.FillDropDownListWithTimes(this.mDlTime,TimeSpan.FromSeconds(0), TimeSpan.FromHours(0));
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // set the selected time
        this.FillDropDownListWithTimes(this.mDlTime, this.StartTime, this.StopTime);
        Helper.SelectListItem(this.mDlTime, Convert.ToInt32(this.Time.TotalMinutes).ToString());
    }

    private void FillDropDownListWithTimes(DropDownList dropDown, TimeSpan startTime, TimeSpan stopTime)
    {
        TimeSpan runningTotal = startTime;
        dropDown.Items.Clear();
        DateTime start = DateTime.Today.Add(startTime);
        DateTime stop = DateTime.Today.Add(stopTime);
        DateTime index = start;

        while (index < stop)
        {
            dropDown.Items.Add(new ListItem(index.ToShortTimeString(), ((int)runningTotal.TotalMinutes).ToString()));
            index += Interval;
            runningTotal += Interval;
        }
    }
    protected void mDlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Time = TimeSpan.FromMinutes(int.Parse(this.mDlTime.SelectedValue));
        this.OnSelectedTimeChanged();
    }

    protected void OnSelectedTimeChanged()
    {
        if (this.SelectedTimeChanged != null)
            this.SelectedTimeChanged(this, EventArgs.Empty);
    }
}
