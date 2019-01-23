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

public partial class PopupControl : System.Web.UI.MasterPage
{
    #region Properties
    /// <summary>
    /// The title of the popup
    /// </summary>
    public string PopupTitle
    {
        get
        {
            return this.popupTitle.InnerText;
        }
        set
        {
            this.popupTitle.InnerText = value;
        }
    }
    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        this.mErrorBox.Visible = false;       
    }

    protected void Page_Load(object sender, EventArgs e)
	{
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
	}

    public void ShowMessage(string message)
    {
        this.mLbError.Text = message;
        this.mErrorBox.Visible = true;
    }

    public void ShowError(Exception ex)
    {
        if (!(ex is ApplicationException))
        {
            try
            {
                Helper.API.LogError(ex);
            }
            catch (Exception innerEx)
            {
                // nothing can be done at this point, just continue on...
            }
        }
        this.ShowMessage(ex.Message);
    }

    public void ClosePopup()
    {
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"close","function pageLoad() { ClosePopup() }",true);
        //this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "closerefresh", ScriptHelper.CreateAtlasScriptBlock("<components><application id=\"application\" load=\"ClosePopup\" /></components>"));
    }

    public void ClosePopup(Guid listingID)
    {
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", string.Format("function pageLoad() {{ ClosePopupAndRefresh('{0}') }}", listingID), true);
        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "closerefresh", ScriptHelper.CreateAtlasScriptBlock(string.Format("<components><application id=\"application\" load=\"ClosePopupAndRefresh('{0}')\" /></components>", listingID.ToString())));
    }
}
