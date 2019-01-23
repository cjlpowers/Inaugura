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

public partial class Listing : System.Web.UI.Page
{
    private string[] mPostals;

    #region Properties    
    #endregion
    
    protected void Page_Load(object sender, EventArgs e)
	{
        this.Master.ScriptManager.RegisterAsyncPostBackControl(this.Listing1);
        this.Master.ScriptManager.RegisterAsyncPostBackControl(this.mSearchScroller);
        this.Title = Helper.UI.Title("Listings");

        this.mSearchScroller.SetSearch(Helper.Session.Search);
        this.mSearchScroller.ListingSelected += new Controls_Components_SearchScroller.ListingSelectedHandler(mSearchScroller_ListingSelected);

        if (!this.IsPostBack)
        {
            string code = RequestHelper.Code;
            Guid listingID = RequestHelper.ID;

            // see if we need to do a redirect based on a listing code
            if (code != null)
            {
                Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListingByCode(code);
                if (listing == null)
                {
                    // TODO redirect properly
                    throw new Exception("Listing could not be found");
                }
                this.Response.Redirect(string.Format("~/Listing.aspx?id={0}", listing.ID.ToString()));
            }
            else if (listingID != Guid.Empty)
            {
                Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(listingID);
                if (listing != null)
                {
                    if (listing.Status != Inaugura.RealLeads.Listing.ListingStatus.Active)
                    {
                        // this listing is not active, does the user have the ability to see it anyway?
                        if (Helper.Session.User == null || (listing.UserID != Helper.Session.User.ID && !Helper.Session.User.IsInRole(Inaugura.RealLeads.UserRoles.Administrator)))
                            throw new HttpException(600, "The listing can not be viewed at this time.");
                    }
                    this.ShowListing(listing);
                }
            }
            else
                throw new ApplicationException("Expected param id not found");
        }
	}

    void mSearchScroller_ListingSelected(Guid listingID)
    {
        Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(listingID);
        this.ShowListing(listing);
        this.mUpdateListing.Update();
    }
  
	private void ShowListing(Inaugura.RealLeads.Listing listing)
	{
        // make sure we have all the audio files
        this.Title = Helper.UI.Title(listing.Title);
        //this.GetAudioFiles(listing);
        this.Listing1.ShowListing(listing);
        //this.ListingViewPanel1.Listing = listing;        
	}

    private void GetAudioFiles(Inaugura.RealLeads.Listing listing)
    {
        if (listing.InformationPrompt != null && listing.InformationPrompt != string.Empty)
            this.GetAudioFile(listing.InformationPrompt);

        if (listing is Inaugura.RealLeads.RealEstateListing && ((Inaugura.RealLeads.RealEstateListing)listing).OpenHousePrompt != null && ((Inaugura.RealLeads.RealEstateListing)listing).OpenHousePrompt != string.Empty)
            this.GetAudioFile(((Inaugura.RealLeads.RealEstateListing)listing).OpenHousePrompt);
    }

    private void GetAudioFile(string fileID)
    {
        string filePath = this.GetFilePath(fileID);
        if (!System.IO.File.Exists(filePath))
        {
            Inaugura.RealLeads.File file = Helper.API.ListingManager.GetFile(new Guid(fileID));
            byte[] mp3Data = Inaugura.Media.Helper.ConvertToMP3(file.Data, 16);
            System.IO.File.WriteAllBytes(GetFilePath(fileID), mp3Data);
        }
    }

    private string GetFilePath(string id)
    {
        string path = string.Format("~/Content/Audio/{0}.mp3", id);
        return this.MapPath(path);
    }
    
    private Inaugura.RealLeads.RealEstateListing CreateListing()
    {
        if (mPostals == null)
            mPostals = System.IO.File.ReadAllLines(Server.MapPath("~/Postals.txt"));
        
        Random rand = new Random((int)DateTime.Now.Ticks);
        Inaugura.RealLeads.PropertyType type = GetRandomType(Inaugura.RealLeads.PropertyType.All, rand) as Inaugura.RealLeads.PropertyType;
        Inaugura.RealLeads.RealEstateListing listing = new Inaugura.RealLeads.RealEstateListing(type);
        listing.UserID = SessionHelper.User.ID;
        // geocode the postal        
        listing.Address = Helper.API.LocatePostal(mPostals[rand.Next(0, mPostals.Length)]);
        listing.Address.Street = "123 Someplace Drive";
        listing.YearBuilt = 1992;
        listing.Appraisal = rand.Next(100000, 300000);
        listing.BackyardNeighbors = rand.Next(5) != 1;
        listing.BasementType = GetRandomType(Inaugura.RealLeads.BasementType.All, rand) as Inaugura.RealLeads.BasementType;
        listing.Code = Helper.API.ListingManager.GetUnusedCodes(1)[0];
        listing.Costs.Add(new Inaugura.RealLeads.Cost("Hot Water Tank", "DHW Tank", rand.Next(200, 500)));
        listing.Description = "Fine features of this exquisite executive retreat include an open- concept design with dramatic cathedral ceiling, radiant in-floor heat, great use of ceramic, master-bedroom loft, stunning maple kit.";
        listing.ExpirationDate = DateTime.Now.AddDays(5);
        listing.Features.Add("Orchard");
        listing.ExteriorPrimary = GetRandomType(Inaugura.RealLeads.ExteriorMaterial.All, rand) as Inaugura.RealLeads.ExteriorMaterial;
        listing.ExteriorSecondary = GetRandomType(Inaugura.RealLeads.ExteriorMaterial.All, rand) as Inaugura.RealLeads.ExteriorMaterial;
        listing.Fireplace = rand.Next(2) == 1;
        listing.FoundationType = GetRandomType(Inaugura.RealLeads.FoundationType.All, rand) as Inaugura.RealLeads.FoundationType;
        listing.Features.Add("New carpet");
        listing.Features.Add("Aris Valey Golf Course");
        listing.Pool = rand.Next(4) == 1;
        listing.Price = rand.Next(150000, 400000);
        listing.PropertyTax.Value = rand.Next(1000, 4000);
        listing.PropertyTax.Year = 2006;
        listing.PropertyTax.Estimated = true;
        listing.RentalEquipment.Add("Hot water tank");
        listing.RoofType = GetRandomType(Inaugura.RealLeads.RoofType.All, rand) as Inaugura.RealLeads.RoofType;
        listing.Size = new Inaugura.Measurement.Measurement(rand.Next(1200, 5000), Inaugura.Measurement.Unit.SquareFeet);
        listing.Status = Inaugura.RealLeads.Listing.ListingStatus.Active;
        listing.Title = string.Format("Listing {0}", listing.Code);
        listing.Waterfront = rand.Next(10) == 1;
        listing.WaterType = GetRandomType(Inaugura.RealLeads.WaterType.All, rand) as Inaugura.RealLeads.WaterType;

        int numLevels = rand.Next(1, 3);
        for (int i = 0; i < numLevels; i++)
        {
            Inaugura.RealLeads.Level level = CreateLevel(string.Format("Level {0}", numLevels), rand);
            listing.Levels.Add(level);
        }        

        Helper.API.ListingManager.AddListing(listing);
        CreateImages(listing, rand);
        listing.Images.DefaultImage = listing.Images[0];
        Helper.API.ListingManager.UpdateListing(listing);
        return listing;
    }

    private Inaugura.RealLeads.RentalPropertyListing CreateRentalListing(Random rand)
    {
        if (mPostals == null)
            mPostals = System.IO.File.ReadAllLines(Server.MapPath("~/Postals.txt"));

        Inaugura.RealLeads.RentalPropertyType type = GetRandomType(Inaugura.RealLeads.RentalPropertyType.All, rand) as Inaugura.RealLeads.RentalPropertyType;
        Inaugura.RealLeads.RentalPropertyListing listing = new Inaugura.RealLeads.RentalPropertyListing(type);
        listing.UserID = SessionHelper.User.ID;
        // geocode the postal        
        listing.Address = Helper.API.LocatePostal(mPostals[rand.Next(0, mPostals.Length)]);
        listing.Address.Street = "123 Someplace Drive";
        listing.YearBuilt = 1992;
        listing.Code = Helper.API.ListingManager.GetUnusedCodes(1)[0];
        listing.Costs.Add(new Inaugura.RealLeads.Cost("Electricity", "Average Monthly Electricity Bill", rand.Next(39, 70)));
        listing.Description = "Fine features of this exquisite executive retreat include an open- concept design with dramatic cathedral ceiling, radiant in-floor heat, great use of ceramic, master-bedroom loft, stunning maple kit.";                    
        listing.ExpirationDate = DateTime.Now.AddDays(5);        
        listing.Features.Add("BBQ");
        listing.Fireplace = rand.Next(2) == 1;
        listing.Features.Add("New carpet");
        listing.Features.Add("Near Shopping Centre");
        listing.Pool = rand.Next(4) == 1;        
        listing.Size = new Inaugura.Measurement.Measurement(rand.Next(1200, 5000), Inaugura.Measurement.Unit.SquareFeet);
        listing.Status = Inaugura.RealLeads.Listing.ListingStatus.Active;
        listing.Title = string.Format("Listing {0}", listing.Code);
        listing.Waterfront = rand.Next(10) == 1;

        listing.FurnishingType = GetRandomType(Inaugura.RealLeads.FurnishingType.All, rand) as Inaugura.RealLeads.FurnishingType;
        listing.Pets = rand.Next(3) == 1;
        listing.IncludesElectricity = rand.Next(8) == 1;
        listing.IncludesHeating = rand.Next(4) == 1;
        listing.InternetService = rand.Next(6) == 1;
        listing.TelevisionService = rand.Next(6) == 1;
        listing.LaundryServices = rand.Next(3) == 1;
        listing.Appliances.Add("Stove");
        listing.Appliances.Add("Refridgerator");
        listing.Appliances.Add("Washing Machine");
        if(rand.Next(2) == 1)
            listing.AvailabilityEnd = DateTime.Now.AddDays(rand.Next(120, 240));

        listing.AvailabilityStart = DateTime.Now.AddDays(rand.Next(15, 60));

        listing.MonthlyRent = rand.Next(300, 1400);
        listing.ParkingIncluded = rand.Next(4) == 1;
        listing.ParkingSpaces = rand.Next(0, 4);

        int numLevels = rand.Next(1, 2);
        for (int i = 0; i < numLevels; i++)
        {
            Inaugura.RealLeads.Level level = CreateLevel(string.Format("Level {0}", numLevels), rand);
            listing.Levels.Add(level);
        }

        /*
        string[] addresses = { "280 Phillip, Waterloo, Ontario", "356 Midwood, Waterloo, Ontario", "170 Albert, Waterloo, Ontario", "140 Albert, Waterloo, Ontario", "135 Albert, Waterloo, Ontario", "25 King, Waterloo, Ontario","125 King, Waterloo, Ontario","34 King, Waterloo, Ontario","455 King, Waterloo, Ontario", "150 King St, Waterloo, Ontario" };

        string address = addresses[rand.Next(addresses.Length)];

        string temp=string.Empty;
        Inaugura.Maps.Address add = GeocodeHelper.Geocode(address, out temp);
        if (add != null)
            listing.Address = add;
         */

        Helper.API.ListingManager.AddListing(listing);
        CreateImages(listing, rand);
        listing.Images.DefaultImage = listing.Images[0];
        Helper.API.ListingManager.UpdateListing(listing);
        return listing;
    }

    private Inaugura.RealLeads.Level CreateLevel(string name, Random rand)
    {      
        Inaugura.RealLeads.Level level = new Inaugura.RealLeads.Level(name);
        level.AboveGrade = rand.Next(3) != 1;
        level.Description = "The description of the level";
        if(rand.Next(2) == 1)
            level.Features.Add("Open concept");
        level.Size = new Inaugura.Measurement.Measurement(rand.Next(400, 1400), Inaugura.Measurement.Unit.SquareFeet);
        int numRooms = rand.Next(2, 5);
        for (int i = 0; i < numRooms; i++)
        {
           Inaugura.RealLeads.Room room = CreateRoom(string.Format("Room {0}",i),rand);
           level.Rooms.Add(room);
        }
        return level;
    }

    private Inaugura.RealLeads.Room CreateRoom(string name, Random rand)
    {        
        Inaugura.RealLeads.Room room = new Inaugura.RealLeads.Room(name, GetRandomType(Inaugura.RealLeads.RoomType.All, rand) as Inaugura.RealLeads.RoomType);
        room.Description = "The description of the room";
        room.Dimensions = new Inaugura.RealLeads.Dimensions(new Inaugura.Measurement.Measurement(rand.Next(5, 30), Inaugura.Measurement.Unit.Feet), new Inaugura.Measurement.Measurement(rand.Next(5, 30), Inaugura.Measurement.Unit.Feet));
        room.FloorMaterial = GetRandomType(Inaugura.RealLeads.FloorMaterial.All, rand) as Inaugura.RealLeads.FloorMaterial;
        if (rand.Next(2) == 1)
            room.Features.Add("High ceiling");

        return room;
    }

    private void CreateImages(Inaugura.RealLeads.Listing listing, Random rand)
    {  
        string imageDirectory = "~/Content/ListingImages/480x360";
        string[] f = System.IO.Directory.GetFiles(Server.MapPath(imageDirectory), "*.jpg");
        if (f.Length == 0)
            return;

        int numImages = rand.Next(1, 3);
        int count = 1;
        System.Collections.Generic.List<string> files = new System.Collections.Generic.List<string>(f);
        while (files.Count > 0 && numImages > 0)
        {
            int index = rand.Next(0, files.Count - 1);
            string imagePath = files[index];

            // get the image
            Inaugura.RealLeads.File file = Inaugura.RealLeads.File.Load(imagePath);
            Inaugura.RealLeads.Image img = new Inaugura.RealLeads.Image(file.ID);
            count++;

            // upload the file to the server
            Helper.API.ListingManager.AddFile(listing.ID, file);
            listing.Images.Add(img);
            files.RemoveAt(index);
            numImages--;
        }
    }

    private Inaugura.RealLeads.Types GetRandomType(Inaugura.RealLeads.Types[] types, Random rand)
    {        
        int index = rand.Next(0, types.Length - 1);
        return types[index];
    }
}
