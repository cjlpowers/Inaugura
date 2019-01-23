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

public partial class ToolButton : System.Web.UI.UserControl
{
    #region Variables
    private string mImageUrl;
    private string mNavigateUrl;
    private string mOnClientClick;
    private string mText;
    #endregion
    
    #region Properties
    public string ImageUrl
    {
        get
        {
            return this.mImageUrl;
        }
        set
        {
            this.mImageUrl = value;
        }
    }

    public string NavigateUrl
    {
        get
        {
            return this.mNavigateUrl;
        }
        set
        {
            this.mNavigateUrl = value;
        }
    }

    public string OnClientClick
    {
        get
        {
            return this.mOnClientClick;
        }
        set
        {
            this.mOnClientClick = value;
        }
    }

    public string Text
    {
        get
        {
            return this.mText;
        }
        set
        {
            this.mText = value;
        }
    }
    #endregion

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.EnableViewState = false;
        this.mImg.Alt = this.Text;
        this.mImg.Src = this.ResolveUrl(this.ImageUrl);
        this.mLnk.HRef = this.ResolveUrl(this.NavigateUrl);
        if (this.OnClientClick != null)
            this.mLnk.Attributes["onclick"] = this.OnClientClick;
    }
}
