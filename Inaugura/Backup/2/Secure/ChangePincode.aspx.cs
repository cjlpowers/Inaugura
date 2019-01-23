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

public partial class ChangePincode : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e)
	{
        this.Title = Helper.UI.Title("Change Pincode");		
	}

    protected void Page_Load(object sender, EventArgs e)
    { 
        if (SessionHelper.User != null)
        {
            this.mPincode.InnerText = string.Format("Your existing pincode is {0}", SessionHelper.User.PinCode);
        }
    }

    protected void mBtnUpdate_Click(object sender, ImageClickEventArgs e)
	{
        Inaugura.RealLeads.User user = SessionHelper.User;
		if (user!= null)
		{
            try
            {
                if (!Inaugura.Validation.ValidatePinCode(this.mTxtPincode.Text, 4))
                {
                    this.mLbError.Text = "Pincode must be 4 numeric digits";
                    return;
                }

                user.PinCode = this.mTxtPincode.Text;
                Helper.API.UserManager.UpdateUser(user);                
                this.Response.Redirect("~/Secure/Default.aspx");
            }
            catch (Inaugura.Security.SecurityException ex)
            {
                Master.ShowMessage(ex.Message);
            }			
		}
		else
		{
			// todo 
			throw new Exception("Session does not contain agent object");
		}
	}  
}
