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

public partial class ContactListingOwner : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PopupTitle = "Contact Listing Owner";
        if (!this.IsPostBack)
        {
            Inaugura.RealLeads.User user = Helper.Request.User;
            if (user != null)
            {
                this.mFieldsetPhone.Visible = !user.PhoneNumberPrivate;
                this.mLbPhoneNumber.Text = user.PhoneNumber;
            }
        }
    }

    protected void mBtnSend_Click(object sender, EventArgs e)
    {
        // make sure the users email address is valid
        if (!Inaugura.Validation.ValidateEmail(this.mTxtEmailAddress.Text))
            Master.ShowMessage("Please provide a valid email address.");

        if (string.IsNullOrEmpty(this.mTxtMessage.Text))
            Master.ShowMessage("Please enter your message in the text field below.");

        Inaugura.RealLeads.Listing listing = Helper.Request.Listing;
        Inaugura.RealLeads.User user = Helper.API.UserManager.GetUser(listing.UserID);

        string htmlContent = Helper.Content.LoadContent("Mailouts/ContactListingOwnerRentleads.htm");
        htmlContent = htmlContent.Replace("[emailAddress]",this.mTxtEmailAddress.Text);
        htmlContent = htmlContent.Replace("[emailMessage]", this.mTxtMessage.Text);
        htmlContent = htmlContent.Replace("[listingID]", listing.ID.ToString());
        htmlContent = htmlContent.Replace("[listingTitle]", listing.Title);        

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        message.Sender = new System.Net.Mail.MailAddress(Helper.Configuration.SystemEmail);
                
        message.Subject = string.Format("Question regarding listing {0}",listing.Code);
        message.To.Add(user.Email);
        
        message.IsBodyHtml = true;
        message.Priority = System.Net.Mail.MailPriority.High;
        message.ReplyTo = new System.Net.Mail.MailAddress(this.mTxtEmailAddress.Text);
        message.From = message.ReplyTo;
        message.Body = htmlContent;
        
        Helper.SendMessage(message);

        this.mPanelEmailMessage.Visible = false;
        this.mPanelSent.Visible = true;
    }
}

