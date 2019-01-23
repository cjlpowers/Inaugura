using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for ResidentialPropertyListingEditControl
/// </summary>
public class ResidentialPropertyListingEditControl: ListingEditControl
{
    #region Variables
    public const string LevelIDParam = "level";
    public const string RoomIDParam = "room";
    #endregion

    #region Properties
    /// <summary>
    /// The level specified by the arguments
    /// </summary>
    protected Inaugura.RealLeads.Level Level
    {
        get
        {
            // get the listing
            Inaugura.RealLeads.ResidentialPropertyListing listing = this.Listing as Inaugura.RealLeads.ResidentialPropertyListing;
            if (listing == null)
                throw new ApplicationException("The listing could not be found");

            // get the level
            Guid levelID = this.Arguments.LevelID;
            if (levelID == Guid.Empty)
                throw new ApplicationException("The level id was not specified");

            Inaugura.RealLeads.Level level = listing.Levels[levelID];
            return level;
        }
    }

    /// <summary>
    /// The room specified by the arguments
    /// </summary>
    protected Inaugura.RealLeads.Room Room
    {
        get
        {
            Inaugura.RealLeads.Level level = this.Level;
            if (level == null)
                throw new ApplicationException("The level could not be found");

            // get the room
            Guid roomID = this.Arguments.RoomID;
            if (roomID == Guid.Empty)
                return null;

            Inaugura.RealLeads.Room room = level.Rooms[roomID];
            if (room == null)
                throw new ApplicationException("The room could not be found");
            return room;
        }
    }
    #endregion
}
