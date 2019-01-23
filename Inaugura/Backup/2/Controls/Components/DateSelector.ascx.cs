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

public partial class Controls_Components_DateSelector : System.Web.UI.UserControl
{

    #region Properties
    /// <summary>
    /// The selected date
    /// </summary>
    public DateTime SelectedDate
    {
        get
        {
            return this.mCal.SelectedDate;
        }
        set
        {
            this.mCal.SelectedDate = value;
        }
    }

    /// <summary>
    /// Specifies if the control is enabled or not
    /// </summary>
    public bool Enabled
    {
        get
        {
            return this.mCal.Enabled;
        }
        set
        {
            this.mCal.Enabled = value;
            this.mDlYear.Enabled = value;
            this.mDlMonth.Enabled = value;
        }
    }

    /// <summary>
    /// The visible date
    /// </summary>
    public DateTime VisibleDate
    {
        get
        {
            return this.mCal.VisibleDate;
        }
        set
        {
            this.mCal.VisibleDate = value;
            this.mDlMonth.SelectedIndex = value.Month - 1;

            foreach (ListItem item in this.mDlYear.Items)
            {
                if (item.Value == value.Year.ToString())
                {
                    item.Selected = true;
                    return;
                }
            }
            ListItem li = new ListItem(value.Year.ToString());
            this.mDlYear.Items.Add(li);
            li.Selected = true;
        }
    }
    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.FillYears();
            this.FillMonths();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {        
        }
    }

    private void FillYears()
    {
        this.mDlYear.Items.Clear();
        for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 5; i++)
            this.mDlYear.Items.Add(i.ToString());
    }

    private void FillMonths()
    {
        this.mDlMonth.Items.Clear();
        DateTime date = new DateTime(2000, 1, 1);
        for (int i = 1; i <= 12; i++)
        {
            this.mDlMonth.Items.Add(new ListItem(date.ToString("MMMM"), i.ToString()));
            date=date.AddMonths(1);
        }
    }

    protected void mDlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ChangeVisibleDate();
    }
    protected void mDlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ChangeVisibleDate();
    }

    private void ChangeVisibleDate()
    {
        this.mCal.VisibleDate = new DateTime(int.Parse(this.mDlYear.SelectedValue), int.Parse(this.mDlMonth.SelectedValue), 1);
    }    
}
