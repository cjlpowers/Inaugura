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

public partial class Controls_Edit_EditDescription : ListingEditControl
{
    public override void Initialize()
    {
        // update the listing address
        Inaugura.RealLeads.Listing listing = this.Listing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        this.mTxtDescription.Text = listing.Description;

        base.Initialize();
    }

    protected override void OnOk()
    {
        // update the listing description
        Inaugura.RealLeads.Listing listing = this.Listing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        listing.EnforceEditPolicy();

        listing.Description = this.mTxtDescription.Text;

        Helper.API.ListingManager.UpdateListing(listing);

        base.OnOk();
    }
}
