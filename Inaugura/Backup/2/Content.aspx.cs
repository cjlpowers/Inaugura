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

public partial class Content : System.Web.UI.Page
{
    private string Target
    {
        get
        {
            if (this.Request.Params["target"] != null)
                return this.Request.Params["target"].ToLower();
            return null;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {        
        string target = this.Target;
        string fullPath;

        // see if the target exists
        if (!string.IsNullOrEmpty(target))
        {
            target = target + ".html";
            Master.SetBodyContent(target);
        }
        else
        {
            this.Response.Redirect("~/Default.aspx");
        }
    }
}
