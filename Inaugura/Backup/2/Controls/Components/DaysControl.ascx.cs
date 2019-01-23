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

public partial class DaysControl : System.Web.UI.UserControl
{
    #region Properties
    public DaysOfWeek DaysOfWeek
    {
        get
        {
            DaysOfWeek days = DaysOfWeek.None;

            if (mChkSun.Checked)
                days |= DaysOfWeek.Sunday;
            if (mChkMon.Checked)
                days |= DaysOfWeek.Monday;
            if (mChkTues.Checked)
                days |= DaysOfWeek.Tuesday;
            if (mChkWed.Checked)
                days |= DaysOfWeek.Wednesday;
            if (mChkThur.Checked)
                days |= DaysOfWeek.Thursday;
            if (mChkFri.Checked)
                days |= DaysOfWeek.Friday;
            if (mChkSat.Checked)
                days |= DaysOfWeek.Saturday;

            return days;
        }
        set
        {
            this.mChkSun.Checked = (value & DaysOfWeek.Sunday) != 0;
            this.mChkMon.Checked =(value & DaysOfWeek.Monday) != 0;
            this.mChkTues.Checked = (value & DaysOfWeek.Tuesday) != 0;
            this.mChkWed.Checked = (value & DaysOfWeek.Wednesday) != 0;
            this.mChkThur.Checked = (value & DaysOfWeek.Thursday) != 0;
            this.mChkFri.Checked = (value & DaysOfWeek.Friday) != 0;
            this.mChkSat.Checked = (value & DaysOfWeek.Saturday) != 0;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.mChkSun.Enabled;
        }
        set
        {
            this.mChkSun.Enabled = value;
            this.mChkMon.Enabled = value;
            this.mChkTues.Enabled = value;
            this.mChkWed.Enabled = value;
            this.mChkThur.Enabled = value;
            this.mChkFri.Enabled = value;
            this.mChkSat.Enabled = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
