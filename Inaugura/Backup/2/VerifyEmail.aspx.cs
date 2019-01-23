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

public partial class VerifyEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Guid userID = new Guid(this.Request.Params["userId"]);
            Inaugura.RealLeads.User user = Helper.API.UserManager.GetUser(userID);

            if(user == null)
            {
                this.Master.SetBodyContent("EmailVerificationFail.html");
                Helper.API.LogError(new Inaugura.RealLeads.Administration.ErrorInformation(string.Format("Unable to verify account '{0}'",this.Request.Url.ToString())));
                return;
            }
            if (user.EmailVerified)
            {
                this.Master.SetBodyContent("EmailVerificationSuccess.html");
                return;
            }            
            if (user.Details["emailVerificationKey"] == this.Request.Params["key"])
            {
                user.EmailVerified = true;
                Helper.API.UserManager.UpdateUser(user);
                Helper.Session.Logout();
                this.Master.SetBodyContent("EmailVerificationSuccess.html");
            }
        }
    }
}
