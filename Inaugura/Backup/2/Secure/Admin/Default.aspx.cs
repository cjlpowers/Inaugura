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

public partial class Secure_Admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Admin.html");
        this.mChkContentEditing.Enabled = Inaugura.Security.Role.IsInRole(Inaugura.RealLeads.UserRoles.WebsiteEditor);
        if(!this.IsPostBack)
            this.mChkContentEditing.Checked = Helper.Content.CanEdit;        
    }

    protected void mChkContentEditing_CheckedChanged(object sender, EventArgs e)
    {
        Helper.Content.CanEdit = this.mChkContentEditing.Checked;
    }

    #region Email
    protected void mBtnSend_Click(object sender, EventArgs e)
    {
        try
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.Body = this.mTxtMessage.Text;
            message.To.Add(this.mTxtEmailAddress.Text);
            message.Subject = this.mTxtSubject.Text;
            Helper.SendMessage(message);
        }
        catch (Exception ex)
        {
            Master.ShowMessage(ex.Message);
        }
    }
    #endregion
}