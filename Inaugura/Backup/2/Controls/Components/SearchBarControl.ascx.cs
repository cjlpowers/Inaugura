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

public partial class SearchBarControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.mDiv.Attributes["style"] = string.Format("width: 599; height: 100; background-image: url('{0}')", this.ResolveUrl("~/Content/en/images/searchbar.gif"));
        this.mTxtCode.Attributes["onKeyPress"] = string.Format("if ((event.which ? event.which : event.keyCode)==13) {{document.getElementById('{0}').click(); return false;}}", this.mBtnGo.ClientID);
        this.mBtnGo.ImageUrl = Helper.Content.LocalizedContentPath("images/buttons/go.gif");
        this.mLnkSearch.NavigateUrl = "~/Search.aspx";
        this.mLbError.Visible = false;
    }

    protected void mBtnGo_Click(object sender, ImageClickEventArgs e)
    {
        if (!Inaugura.Validation.ValidateNumeric(this.mTxtCode.Text, 4))
        {
            this.mLbError.Text = "Invalid listing code. Must be a 4 digit number";
            this.mLbError.Visible = true;
        }
        else
            this.Response.Redirect(string.Format("~/Listing.aspx?code={0}", this.mTxtCode.Text));
    }
}
