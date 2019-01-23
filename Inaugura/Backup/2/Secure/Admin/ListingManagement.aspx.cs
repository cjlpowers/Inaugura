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

public partial class Secure_Admin_ListingManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.SetBodyContent("Admin.html");
    }
    protected void mBtnClean_Click(object sender, EventArgs e)
    {
        Helper.API.ListingManager.ArchiveExpiredListings(Convert.ToDateTime(this.mTxtStartDate.Text));
    }

    protected void mBtnGetListingXml_Click(object sender, EventArgs e)
    {
        try
        {
            // see if we can get the listing
            Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(new Guid(this.mTxtListingXmlID.Text));

            if (listing == null)
                throw new ApplicationException("The listing was not found");

            this.mTxtListingXml.Text = listing.Xml.OuterXml;
        }
        catch (Exception ex)
        {
            Master.ShowMessage(ex.Message);
        }
    }
}
