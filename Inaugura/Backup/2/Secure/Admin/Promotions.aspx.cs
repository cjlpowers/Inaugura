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

public partial class Secure_Admin_Promotions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Admin.html");

        if (!this.IsPostBack)
        {
            this.mTxtCode.Text = Inaugura.RealLeads.Promotion.GenerateCode(5);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // fill the list of promotions
        Inaugura.RealLeads.Promotion[] promotions = Helper.API.UserManager.GetPromotions();
        this.mGridPromotions.DataSource = promotions;
        this.mGridPromotions.DataBind();
    }


    protected void mBtn_Click(object sender, EventArgs e)
    {
        try
        {
            Inaugura.RealLeads.Promotion promotion = new Inaugura.RealLeads.Promotion();
            promotion.Description = this.mTxtDescription.Text;
            promotion.Count = int.Parse(this.mTxtCount.Text);
            promotion.Code = this.mTxtCode.Text;

            string[] actions = this.mTxtActions.Text.Split(';');
            foreach (string action in actions)
            {
                string[] items = action.Split(',');
                if (items.Length != 2)
                    throw new ApplicationException("The action text was not well formed.");

                promotion.Actions.Add(new Inaugura.RealLeads.Promotion.Action(items[0], items[1]));
            }
            Helper.API.UserManager.AddPromotion(promotion);
        }
        catch (Exception ex)
        {
            Master.ShowMessage(ex.Message);
        }       
    }
}
