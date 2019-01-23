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

public partial class Editor_ascx : System.Web.UI.UserControl
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
        this.FCKeditor1.BasePath = this.Page.ResolveClientUrl("~/Controls/FCKEditor/");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.PlaceHolder1.SetRenderMethodDelegate(new RenderMethod(this.TestRender));
        if (this.Editable)
        {            
            this.FCKeditor1.Visible = true;
            this.PlaceHolder1.Visible = false;
        }
        else
        {            
            this.FCKeditor1.Visible = false;
            this.PlaceHolder1.Visible = true;
        }

        this.mBtnSubmit.Visible = this.Editable;
        this.mBtnCancel.Visible = this.Editable;
        this.mBtnEdit.Visible = this.HasPermission & !this.Editable;        
    }

    private void TestRender(HtmlTextWriter writer,Control control)
    {
        writer.Write(this.mHtml);
    }

    public event EventHandler SubmitClicked;

    #region Variables
    private string mHtml;
    #endregion

    #region Properties
    public string Html
    {
        get
        {
            return this.mHtml;
        }
        set
        {
            this.FCKeditor1.Value = value;
            this.mHtml = value;
        }
    }

    public string ImagePaths
    {
        get
        {
            return string.Empty;
        }
        set
        {
           
        }
    }

    public bool Editable
    {
        get
        {
            if (this.ViewState["Editable"] == null)
                return false;
            else
                return (bool)this.ViewState["Editable"];
        }
        set
        {
            this.ViewState["Editable"] = value;
        }
    }

    public bool HasPermission
    {
        get
        {
            if (this.ViewState["HasPermission"] == null)
                return false;
            else
                return (bool)this.ViewState["HasPermission"];
        }
        set
        {
            this.ViewState["HasPermission"] = value;
        }
    }

    #endregion

    protected void mBtnEdit_Click(object sender, EventArgs e)
    {
       
    }

    private void OnSubmitClicked()
    {
        if (this.SubmitClicked != null)
            this.SubmitClicked(this, EventArgs.Empty);
    }
    protected void mBtnSubmit_Click(object sender, EventArgs e)
    {
        this.Html = this.FCKeditor1.Value;
        this.Editable = false;

        // save the content
        this.OnSubmitClicked();
    }
    protected void mBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        this.Editable = true;
    }
    protected void mBtnCancel_Click(object sender, EventArgs e)
    {
        if (this.Editable)
            this.Editable = false;
    }
}
