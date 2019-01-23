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

public partial class Controls_Edit_EditLevel : ResidentialPropertyListingEditControl
{
    public override void Initialize()
    {
        if (this.Arguments.LevelID == Guid.Empty)
        {
            this.legend.InnerText = "New Level";
            return;
        }

        // get the room
        Inaugura.RealLeads.Level level = this.Level;
        this.legend.InnerText = string.Format("Edit Level '{0}'", level);

        Inaugura.Measurement.Measurement totalArea = level.Rooms.Area;
        this.mTxtDescription.Text = level.Description;
        this.mTxtName.Text = level.Name;
        this.mSize.Measurement = level.Size;
        this.mChkAboveGrade.Checked = level.AboveGrade;
        Helper.UI.SetItems(level.Features.ToArray(), this.mTxtFeatures);

        base.Initialize();
    }

    protected override void OnOk()
    {
        Inaugura.RealLeads.RentalPropertyListing listing = this.Listing as Inaugura.RealLeads.RentalPropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        listing.EnforceEditPolicy();

        Inaugura.RealLeads.Level level = null;
        if (this.Arguments.LevelID != Guid.Empty)
            level = listing.Levels[this.Arguments.LevelID];
        else
            level = new Inaugura.RealLeads.Level();

        level.Name = this.mTxtName.Text;
        level.Description = this.mTxtDescription.Text;
        level.Size = this.mSize.Measurement;
        level.Features = new Inaugura.RealLeads.StringCollection(Helper.UI.GetItems(this.mTxtFeatures));
        level.AboveGrade = this.mChkAboveGrade.Checked;

        if (this.Arguments.LevelID == Guid.Empty)
            listing.Levels.Add(level);

        Helper.API.ListingManager.UpdateListing(listing);
        base.OnOk();
    }
}
