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

public partial class Controls_Edit_EditLot : ListingEditControl
{
    public override void Initialize()
    {
        Inaugura.RealLeads.Listing l = this.Listing;
        if (l == null)
            throw new ApplicationException("The listing could not be found");

        Inaugura.RealLeads.PropertyListing listing = l as Inaugura.RealLeads.PropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing was not a property listing");

        this.mTxtLotDescription.Text = listing.Lot.Description;
        this.mLotSize.Measurement = listing.Lot.Size;
        Helper.UI.SetItems(listing.Lot.Features.ToArray(), this.mTxtFeatures);

        base.Initialize();
    }

    protected override void OnOk()
    {
        Inaugura.RealLeads.Listing l = this.Listing;
        if (l == null)
            throw new ApplicationException("The listing could not be found");

        Inaugura.RealLeads.PropertyListing listing = l as Inaugura.RealLeads.PropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing was not a property listing");

        listing.EnforceEditPolicy();

        listing.Lot.Description = this.mTxtLotDescription.Text;
        listing.Lot.Size = this.mLotSize.Measurement;
        listing.Lot.Features = new Inaugura.RealLeads.StringCollection(Helper.UI.GetItems(this.mTxtFeatures));
        Helper.API.ListingManager.UpdateListing(listing);        

        base.OnOk();
    }
}
