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

public partial class Controls_Edit_EditRentalDetails : ListingEditControl
{
    public override void Initialize()
    {
        // update the listing address
        Inaugura.RealLeads.RentalPropertyListing listing = this.Listing as Inaugura.RealLeads.RentalPropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");
        
        this.mTxtRent.Text = listing.MonthlyRent.ToString();
        this.mTxtAppliances.Text.Replace(" ", "");

        this.mTxtAvailabilityStart.Text = listing.AvailabilityStart.ToString("MMMM d, yyyy");
        if(listing.AvailabilityEnd != DateTime.MaxValue)
            this.mTxtAvailabilityEnd.Text = listing.AvailabilityEnd.ToString("MMMM d, yyyy");

        Helper.SelectListItem(this.mDlPropertyType, listing.PropertyType.Value.ToString());
        Helper.SelectListItem(this.mLstFurnishing, listing.FurnishingType.Value.ToString());

        this.mSize.Measurement = listing.Size;

        Helper.SelectListItem(this.mDlParking, listing.ParkingSpaces.ToString());
        this.mChkParkingIncluded.Checked = listing.ParkingIncluded;
        
        Helper.UI.SetItems(listing.Appliances.ToArray(), this.mTxtAppliances);
        Helper.UI.SetItems(listing.Features.ToArray(), this.mTxtFeatures);

        this.mChkPets.Checked = listing.Pets;
        this.mChkParkingIncluded.Checked = listing.ParkingIncluded;
        this.mChkLaundryService.Checked = listing.LaundryServices;
        this.mChkInternet.Checked = listing.InternetService;
        this.mChkElectricity.Checked = listing.IncludesElectricity;
        this.mChkHeating.Checked = listing.IncludesHeating;
        this.mChkTelevision.Checked = listing.TelevisionService;
        this.mChkPool.Checked = listing.Pool;

        base.Initialize();
    }

    protected override void OnOk()
    {
        // update the listing address
        Inaugura.RealLeads.RentalPropertyListing listing = this.Listing as Inaugura.RealLeads.RentalPropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        listing.EnforceEditPolicy();

        float rent = 0;
        if (!float.TryParse(this.mTxtRent.Text, out rent))
            throw new ApplicationException("Please provide a valid monthly rent");

        listing.MonthlyRent = rent;
        DateTime date;
        if (!string.IsNullOrEmpty(this.mTxtAvailabilityStart.Text) && DateTime.TryParse(this.mTxtAvailabilityStart.Text, out date))
            listing.AvailabilityStart = date;
        else
            listing.AvailabilityStart = DateTime.MinValue;
        if (!string.IsNullOrEmpty(this.mTxtAvailabilityEnd.Text) && DateTime.TryParse(this.mTxtAvailabilityEnd.Text, out date))
        {
            if (date < listing.AvailabilityStart)
                throw new ApplicationException("The starting date for availability must precedef the ending date");
            listing.AvailabilityEnd = date;
        }
        else
            listing.AvailabilityEnd = DateTime.MaxValue;

        listing.PropertyType = Inaugura.RealLeads.RentalPropertyType.FromValue(int.Parse(this.mDlPropertyType.SelectedValue));
        listing.FurnishingType = Inaugura.RealLeads.FurnishingType.FromValue(int.Parse(this.mLstFurnishing.SelectedValue));

        listing.Size = this.mSize.Measurement;

        listing.ParkingSpaces = int.Parse(this.mDlParking.SelectedValue);
        listing.ParkingIncluded = this.mChkParkingIncluded.Checked;

        listing.Appliances = new Inaugura.RealLeads.StringCollection(Helper.UI.GetItems(this.mTxtAppliances));

        listing.Features = new Inaugura.RealLeads.StringCollection(Helper.UI.GetItems(this.mTxtFeatures));
        
        listing.Pets = this.mChkPets.Checked;
        listing.ParkingIncluded = this.mChkParkingIncluded.Checked;
        listing.IncludesElectricity = this.mChkElectricity.Checked;
        listing.IncludesHeating = this.mChkHeating.Checked;
        listing.LaundryServices = this.mChkLaundryService.Checked;
        listing.InternetService = this.mChkInternet.Checked;
        listing.TelevisionService = this.mChkTelevision.Checked;
        listing.Pool = this.mChkPool.Checked;
        

        Helper.API.ListingManager.UpdateListing(listing);
        
        base.OnOk();
    }
    
    protected void mDlPropertyType_Init(object sender, EventArgs e)
    {
        Helper.FillTypes(this.mDlPropertyType, Inaugura.RealLeads.RentalPropertyType.All);        
    }
    protected void mLstFurnishing_Init(object sender, EventArgs e)
    {
        Helper.FillTypes(this.mLstFurnishing, Inaugura.RealLeads.FurnishingType.All);        
    }
}
