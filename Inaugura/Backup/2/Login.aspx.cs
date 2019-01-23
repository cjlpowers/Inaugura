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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Login.html");
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", string.Format("function pageLoad() {{ PopupLoginRedirect('{0}') }}", this.ResolveUrl("~/Login.aspx")), true);
    }    

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        Inaugura.RealLeads.User user = Helper.API.UserManager.GetUserByEmail(this.Login1.UserName);

        if (user == null)
            throw new Exception("User was not found");

        // enable role based security
        Helper.API.Login(user);
        
        // save the user
        SessionHelper.User = user;
    }
}
