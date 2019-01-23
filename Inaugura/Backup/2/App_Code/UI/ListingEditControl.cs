using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// The base class for all listing edit controls
/// </summary>
public class ListingEditControl : System.Web.UI.UserControl
{
    #region Events
    public event EventHandler Ok;
    protected virtual void OnOk()
    {
        if (Ok != null)
            Ok(this, EventArgs.Empty);
    }

    public event EventHandler Cancel;
    protected virtual void OnCancel()
    {
        if (Cancel != null)
            Cancel(this, EventArgs.Empty);
    }
    #endregion

    #region Variables
    private Arguments mArguments;
    #endregion

    #region Properties
    /// <summary>
    /// The control arguments
    /// </summary>
    public Arguments Arguments
    {
        get
        {
            return this.mArguments;
        }
        set
        {
            this.mArguments = value;
        }
    }
    
    /// <summary>
    /// The listing
    /// </summary>
    protected Inaugura.RealLeads.Listing Listing
    {
        get
        {
            Guid id = this.Arguments.ListingID;
            if (id != Guid.Empty)
                return Helper.API.ListingManager.GetListing(id);
            return null;
        }
    }
    #endregion

    #region Methods
    public ListingEditControl() : base()
    {
    }
    
    public virtual void Initialize()
    {
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        HtmlGenericControl div = new HtmlGenericControl("div");
        div.Attributes["class"] = "listingEditButtonRow";
        Button okBtn = new Button();
        okBtn.ID = "mBtnOk";
        okBtn.Text = "OK";
        okBtn.Click += new EventHandler(okBtn_Click);
        div.Controls.Add(okBtn);

        Button cancelBtn = new Button();
        cancelBtn.ID = "mBtnCancel";
        cancelBtn.Text = "Cancel";
        cancelBtn.Click += new EventHandler(cancelBtn_Click);
        div.Controls.Add(cancelBtn);

        this.Controls.Add(div);
    }

    protected override void LoadViewState(object savedState)
    {        
        base.LoadViewState(savedState);
        System.Collections.Generic.Dictionary<string, string> paras = this.ViewState["arguments"] as System.Collections.Generic.Dictionary<string, string>;
        if (paras != null)
            this.mArguments = new Arguments(paras);        
    }
   
    void okBtn_Click(object sender, EventArgs e)
    {
        this.OnOk();
    }

    void cancelBtn_Click(object sender, EventArgs e)
    {
        this.OnCancel();
    }

    protected override object SaveViewState()
    {
        if (this.mArguments.Count > 0)
        {
            System.Collections.Generic.Dictionary<string, string> list = new System.Collections.Generic.Dictionary<string, string>(this.mArguments);
            this.ViewState["arguments"] = list;
        }
        return base.SaveViewState();
    }
    #endregion
}
