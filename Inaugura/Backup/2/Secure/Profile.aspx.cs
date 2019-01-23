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

public partial class Profile_aspx : System.Web.UI.Page
{
    // Page events are wired up automatically to methods 
    // with the following names:
    // Page_Load, Page_AbortTransaction, Page_CommitTransaction,
    // Page_DataBinding, Page_Disposed, Page_Error, Page_Init, 
    // Page_Init Complete, Page_Load, Page_LoadComplete, Page_PreInit
    // Page_PreLoad, Page_PreRender, Page_PreRenderComplete, 
    // Page_SaveStateComplete, Page_Unload

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Profile.html");
    }
     
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.mTxtFirstName.Text = SessionHelper.User.FirstName;
            this.mTxtLastName.Text = SessionHelper.User.LastName;
            this.mTxtPhone.Text = SessionHelper.User.PhoneNumber;
            this.mChkHidePhoneNumber.Checked = SessionHelper.User.PhoneNumberPrivate;
        }
    }
    
    protected void mBtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Inaugura.RealLeads.User user = SessionHelper.User;
            user.FirstName = this.mTxtFirstName.Text;
            user.LastName = this.mTxtLastName.Text;
            user.PhoneNumber = this.mTxtPhone.Text;
            user.PhoneNumberPrivate = this.mChkHidePhoneNumber.Checked;
            Helper.API.UserManager.UpdateUser(user);
        }
        catch (Inaugura.Security.SecurityException ex)
        {
            Master.ShowMessage(ex.Message);
        }
    }
}
