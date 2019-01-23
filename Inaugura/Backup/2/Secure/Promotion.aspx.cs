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

public partial class Promotion : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Secure/Promotion.html");
        this.mPhSuccess.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void mBtnRedeem_Click(object sender, EventArgs e)
    {
        // try and get the promotion
        Inaugura.RealLeads.Promotion promotion = Helper.API.UserManager.GetPromotionByCode(this.mTxtCode.Text);
        if (promotion == null)
        {
            Master.ShowMessage("The promotion code you have entered does not exist.");
            return;
        }        
        Helper.Session.User.ApplyPromotion(Helper.API, promotion);
        this.mTxtCode.Text = string.Empty;
        this.mPhSuccess.Visible = true;
    }
}
