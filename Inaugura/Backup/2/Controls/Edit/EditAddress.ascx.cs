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

public partial class Controls_Edit_EditAddress : ListingEditControl
{
    public override void Initialize()
    {
        // update the listing address
        Inaugura.RealLeads.PropertyListing listing = this.Listing as Inaugura.RealLeads.PropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        this.mTxtStreet.Text = listing.Address.Street;
        this.mTxtPostal.Text = listing.Address.ZipPostal;
        
        base.Initialize();
    }

    protected override void OnOk()
    {
        // get the new postal
        if (string.IsNullOrEmpty(this.mTxtPostal.Text))
            throw new ApplicationException("Please enter a postal code");

        Inaugura.Maps.Address address = Helper.API.LocatePostal(this.mTxtPostal.Text);
        if (address == null)
            throw new ApplicationException("The postal code could not be found");

        if (string.IsNullOrEmpty(this.mTxtStreet.Text))
            throw new ApplicationException("Please enter a street address");

        address.Street = this.mTxtStreet.Text;

        // update the listing address
        Inaugura.RealLeads.PropertyListing listing = this.Listing as Inaugura.RealLeads.PropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        listing.EnforceEditPolicy();

        listing.Address = address;
        
        Helper.API.ListingManager.UpdateListing(listing);

        base.OnOk();
    }
}
