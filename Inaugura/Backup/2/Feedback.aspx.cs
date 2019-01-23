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

public partial class Feedback_aspx : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Contact.html");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Inaugura.RealLeads.User user = SessionHelper.User;
            if (user != null)
            {
                this.mTxtFrom.Text = user.FirstName + " " + user.LastName;
                this.mTxtEmail.Text = user.Email;
            }
        }
    }

    protected void mBtnSend_Click(object sender, EventArgs e)
    {
        string fromName = this.mTxtFrom.Text;
        string fromEmail = this.mTxtEmail.Text;
        string message = this.mTxtMessage.Text;

        if (!Inaugura.Validation.ValidateEmail(fromEmail))
        {
            Master.ShowMessage("Please provide a valid email address.");
            return;
        }

        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(Helper.Configuration.SystemEmail, Helper.Configuration.SupportEmail);
        mail.ReplyTo = new System.Net.Mail.MailAddress(fromEmail);
        mail.Subject = "Feedback from " + fromName;
        mail.Body = message;
        
        Helper.SendMessage(mail);

        this.mLbResult.Visible = true;
        this.mBtnSend.Visible = false;
    }
}
