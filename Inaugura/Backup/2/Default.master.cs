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

public partial class _Default : System.Web.UI.MasterPage
{

    public ScriptManager ScriptManager
    {
        get
        {
            return this.ScriptManager2;
        }
    }
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if(this.mContentFooter != null)
            this.mContentFooter.ContentPath = "Footer.html";

        this.mMessageBox.Visible = false;
    }

    protected override void OnPreRender(EventArgs e)
    {
        Label welcomeLabel = ((Label)this.LoginView2.FindControl("mLbWelcome"));
        if (SessionHelper.User != null && welcomeLabel != null)
            welcomeLabel.Text = string.Format("Welcome {0}", SessionHelper.User.FirstName);

        base.OnPreRender(e);
    }

    protected void Page_Init(object sender, EventArgs e)
    {        
        //this.mContentHeader.ContentKey = "HeaderContent";
        //this.mContentHeader.ContentPath = "Header.html";        
        
    }


    private void RegisterAutoLogoutScript()
    {
        // Redirect the browser to the root page if the Session timeout passes
        //string script = String.Format("<script language=\"javascript\">window.setTimeout(\"window.location='{0}';\", {1})</script>", Page.ResolveClientUrl("~/Default.aspx?action=timeout"), Session.Timeout * 60 * 1000 + 500);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "TimeoutLogout", script);
    }
   
    //protected void mBtnGo_Click(object sender, ImageClickEventArgs e)
    //{
    //    Inaugura.RealLeads.Listing listing = DataHelper.RealLeadsDataStore.ListingStore.GetListingByCode(this.mTxtCode.Text);
    //    if (listing != null)
    //        this.Response.Redirect(string.Format("~/Listing.aspx?id={0}", listing.ID));
    //}

    protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
    {
        Helper.Session.Logout();
        Response.Redirect("~/default.aspx");
    }

    public void ShowMessage(string message)
    {
        this.mLbMessage.Text = message;        
        this.mMessageBox.Visible = true;
    }

    /// <summary>
    /// Sets the body content
    /// </summary>
    /// <param name="contentPath">The content path</param>
    /// <returns>True if success, false otherwise</returns>
    public bool SetBodyContent(string contentPath)
    {
        string target = contentPath;
        string fullPath;

        // see if the target exists
        if (!string.IsNullOrEmpty(target))
        {           
            fullPath = Helper.Content.FullPath(target);
            if (!System.IO.File.Exists(fullPath))
                throw new HttpException(404, string.Empty);

            string content = Helper.Content.LoadContent(target);

            // set the title
            this.SetTitle(content);

            // get the content
            this.mContentBody.ContentPath = target;
            return true;
        }
        return false;
    }

    private void SetTitle(string content)
    {
        int startIndex = content.IndexOf("<!--");
        int endIndex = content.IndexOf("-->");
        if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
        {
            string[] titleStrings = content.Substring(startIndex + 4, endIndex - (startIndex +4)).Split(',');
            Page.Title = Helper.UI.Title(titleStrings);
        }
        else
            Page.Title = Helper.UI.Title(Helper.Configuration.ApplicationSlogan);
    }
}

