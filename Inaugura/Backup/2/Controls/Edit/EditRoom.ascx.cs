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

public partial class Controls_Edit_EditRoom : ResidentialPropertyListingEditControl
{
    public override void Initialize()
    {
        // get the room
        Inaugura.RealLeads.Room room = this.Room;
        if (room == null)
        {
            this.legend.InnerText = "New Room";
            return;
        }

        this.legend.InnerText = string.Format("Edit Room '{0}'", room);

        this.mTxtDescription.Text = room.Description;
        this.mTxtName.Text = room.Name;
        this.mWidth.Measurement = room.Dimensions.Width;
        this.mDepth.Measurement = room.Dimensions.Depth;
        Helper.UI.SetItems(room.Features.ToArray(), this.mTxtFeatures);
        Helper.SelectListItem(this.mLstType, room.Type.Value.ToString());
        Helper.SelectListItem(this.mLstFlooring, room.FloorMaterial.Value.ToString());

        base.Initialize();
    }

    protected override void OnOk()
    {
        Inaugura.RealLeads.RentalPropertyListing listing = this.Listing as Inaugura.RealLeads.RentalPropertyListing;
        if (listing == null)
            throw new ApplicationException("The listing could not be found");

        listing.EnforceEditPolicy();

        Inaugura.RealLeads.Level level = listing.Levels[this.Arguments.LevelID];
        if (level == null)
            throw new ApplicationException("The level could not be found");

        Guid roomID = this.Arguments.RoomID;
        Inaugura.RealLeads.Room room = null;
        // are we working with an existing room?
        if(roomID != Guid.Empty)
        {
            room = level.Rooms[roomID];
            if(room == null)
                throw new ApplicationException("The room could not be found");
        }
        else
            room = new Inaugura.RealLeads.Room();

        room.Description = this.mTxtDescription.Text;
        
        room.Name = this.mTxtName.Text;
        room.Dimensions.Width = this.mWidth.Measurement.Convert(Inaugura.Measurement.Unit.Feet);
        room.Dimensions.Depth = this.mDepth.Measurement.Convert(Inaugura.Measurement.Unit.Feet); 
        
        room.Features = new Inaugura.RealLeads.StringCollection(Helper.UI.GetItems(this.mTxtFeatures));
        room.FloorMaterial = Inaugura.RealLeads.FloorMaterial.FromValue(int.Parse(this.mLstFlooring.SelectedValue));
        room.Type = Inaugura.RealLeads.RoomType.FromValue(int.Parse(this.mLstType.SelectedValue));
        
        if(roomID == Guid.Empty)
            level.Rooms.Add(room);

        Helper.API.ListingManager.UpdateListing(listing);
        base.OnOk();
    }


    protected void mLstType_Init(object sender, EventArgs e)
    {
        Helper.FillTypes(this.mLstType, Inaugura.RealLeads.RoomType.All);        
    }

    protected void mLstFlooring_Init(object sender, EventArgs e)
    {
        Helper.FillTypes(this.mLstFlooring, Inaugura.RealLeads.FloorMaterial.All);
    }
}
