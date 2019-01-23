using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Net;

using Inaugura.RealLeads;
using Inaugura.Maps;

public class Pin
{
    public string id;
    public double longitude;
    public double latitude;
    public string title;
    public string description;
    public string icon;

    public Pin()
    {
    }
}

namespace WebServices
{
    /// <summary>
    /// Summary description for ListingService
    /// </summary>
    [System.Web.Script.Services.ScriptService()]
    public class ListingService : System.Web.Services.WebService
    {
        #region Lisiting Methods
        /// <summary>
        /// Gets a listing
        /// </summary>
        /// <param name="id">The listing ID</param>
        /// <param name="mode">The mode</param>
        /// <returns>The listing content</returns>
        [WebMethod(true)]
        public string GetListingContent(string id, string mode)
        {
            try
            {
                Guid guid = new Guid(id);
                Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(guid);

                if (listing == null)
                    throw new Exception("The listing was not found");

                return ListingService.TransformListing(listing, mode);
            }
            catch (Exception ex)
            {
                Helper.API.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Performs a listing XSL transformation
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="mode">The mode</param>
        /// <returns>The transformed listing</returns>
        static public string TransformListing(Listing listing, string mode)
        {
            if (string.IsNullOrEmpty(mode))
                mode = "Details";

            bool adminMode = listing.CheckEditPolicy(SessionHelper.User);

            #region See if we hace a cached copy
            TransformKey key;
            if (mode == "Details")
                key = new TransformKey(mode, adminMode);
            else
                key = new TransformKey(mode);

            string content = listing.Objects[key] as string;
            #endregion

            if (content == null)
            {
                System.Xml.Xsl.XslCompiledTransform transform;

                if (listing is RentalPropertyListing)
                    transform = Helper.Content.LoadTransform("XSLT/RentalPropertyListing.xsl");
                else if (listing is RealEstateListing)
                    transform = Helper.Content.LoadTransform("XSLT/RealEstateListing.xsl");
                else
                    throw new NotSupportedException("No XSLT transform exists for the current listing type");

                if (transform == null)
                    throw new Exception("Could not find the transform");

                System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();
                args.AddParam("mode", string.Empty, mode == null ? string.Empty : mode);

                System.Xml.XmlNode xmlNode = listing.Xml;

                if (adminMode)
                    args.AddParam("admin", string.Empty, bool.TrueString);
                //else
                //{
                //    if (string.IsNullOrEmpty(mode))
                //    {
                //        Log log = new Log(DateTime.Now);
                //        log.UserID = listing.UserID.ToString();
                //        log.ListingID = listing.ID.ToString();
                //        Helper.API.ListingManager.LogWebView(log);
                //    }
                //}

                PropertyListing propertyListing = listing as PropertyListing;
                if (propertyListing != null)
                {
                    double distance = propertyListing.Distance;
                    if (distance != 0)
                        Inaugura.Xml.Helper.SetAttribute(xmlNode, "distance", distance.ToString());
                    //if(propertyListing.Address.Latitude != 0)
                    //    Inaugura.Xml.Helper.SetAttribute(xmlNode, "mapUrl", propertyListing.Address.YahooMapsUrl);
                }

                System.IO.TextWriter stringWriter = new System.IO.StringWriter();
                transform.Transform(xmlNode.CreateNavigator(), args, stringWriter);
                content =  stringWriter.ToString();
                //content = content.Replace("[Time]", DateTime.Now.ToLongTimeString());
                listing.Objects[key] = content;
            }

            // add the distance
            CachedSearch search = Helper.Session.Search;
            if (search != null)
            {
                PropertySearch ps = search.Search as PropertySearch;
                if (ps != null && ps.Address.Latitude != 0)
                {
                    PropertyListing pl = listing as PropertyListing;
                    if (pl != null)
                    {
                        double distance = ps.Address.Distance(pl.Address);
                        if(mode == "Mini" || string.IsNullOrEmpty(ps.Address.Label))
                            return content.Replace("[distance]", string.Format(" {0:0.00} km",distance));
                        else // also show the search location label
                            return content.Replace("[distance]", string.Format(" {0:0.00} km from {1}",distance, ps.Address.Label));
                    }                    
                }
            }
            return content.Replace("[distance]",string.Empty);
            //return content.Replace("[distance]", "12.56 km from University of Waterloo");
        }

        ///// <summary>
        ///// Performs a listing XSL transformation
        ///// </summary>
        ///// <param name="listing">The listing</param>
        ///// <param name="mode">The mode</param>
        ///// <returns>The transformed listing</returns>
        //static public string TransformListing(System.Xml.XmlNode xml, string mode)
        //{
        //    System.Xml.Xsl.XslCompiledTransform transform;

        //    transform = Helper.Content.LoadTransform("XSLT/RentalPropertyListing.xsl");
            
        //    if (transform == null)
        //        throw new Exception("Could not find the transform");

        //    System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();
        //    args.AddParam("mode", string.Empty, mode == null ? string.Empty : mode);

        //    System.Xml.XmlNode xmlNode = xml;

        //    if (true)
        //        args.AddParam("admin", string.Empty, bool.TrueString);
                             
        //    System.IO.TextWriter stringWriter = new System.IO.StringWriter();
        //    transform.Transform(xmlNode.CreateNavigator(), args, stringWriter);
        //    return stringWriter.ToString();
        //}

        [WebMethod(true)]
        public string[] GetScrollSearchResults(string searchKey, int startIndex, int endIndex)
        {
           if (endIndex - startIndex > 40)
                throw new ArgumentException("The requested result set exceeds the max result set size");

            return WebServices.ListingService.GetSearchResults(searchKey, startIndex, endIndex, "Mini");
        }

        /// <summary>
        /// Gets a search result set
        /// </summary>
        /// <param name="searchKey">The search key</param>
        /// <param name="startIndex">The start index</param>
        /// <param name="endIndex">The end index</param>
        /// <param name="mode">The xsl transform mode</param>
        /// <returns>The transformed results</returns>
        public static string[] GetSearchResults(string searchKey , int startIndex, int endIndex, string mode)
        {
            // does the session contain the search
            CachedSearch search = Helper.Session.Search;
            if (search == null)
                throw new ArgumentException("The requested search does not exist");

            return GetSearchResults(search, startIndex, endIndex, mode);
        }

        /// <summary>
        /// Gets a search result set
        /// </summary>
        /// <param name="search">The search</param>
        /// <param name="startIndex">The start index</param>
        /// <param name="endIndex">The end index</param>
        /// <param name="mode">The xsl transform mode</param>
        /// <returns>The transformed results</returns>
        public static string[] GetSearchResults(CachedSearch search, int startIndex, int endIndex, string mode)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

            string terminator = "<!---->";
            string blockTerminator = "<!--block-->";
            if (startIndex > endIndex)
                throw new ArgumentException("The start index must preceed the end index");            

            // try and get the listing
            if (search == null)
                throw new ArgumentNullException("search","The search object cannot be null");            

            if (search != null)
            {
                // get the requested results
                Inaugura.RealLeads.Listing[] listings = search.GetResults(startIndex, endIndex);

                // write the results            
                if (startIndex == 1 && mode.ToLower() == "mini") // do we need a terminator
                    list.Add(terminator);

                foreach (Inaugura.RealLeads.Listing listing in listings)
                    list.Add(ListingService.TransformListing(listing, mode));

                if (mode.ToLower() == "mini")
                {
                    // do we need a terminator
                    if (endIndex >= search.Search.ResultCount - 1)
                        list.Add(terminator);
                    else
                        list.Add(blockTerminator);
                }
            }
            return list.ToArray();
        }

        #region Listing Commands
        [WebMethod(true)]
        public string ListingOpperation(string id, string target, string opperation)
        {
            // see if we can get the listing
            Inaugura.RealLeads.Listing listing = Helper.API.ListingManager.GetListing(new Guid(id));
            if(listing == null)
                throw new ApplicationException("The listing does not exist");

            if (!listing.CheckEditPolicy(Helper.Session.User))
                throw new Inaugura.Security.SecurityException("You do not have the required permission to perform this operation. This can occur when your session expires. Try logging in again.");                
            
            opperation = opperation.ToLower();
            switch (opperation)
            {
                case "levelup":
                    // todo handle the cases when the listing is not a Residential one
                    this.LevelUp(listing as ResidentialPropertyListing, target);
                    break;
                case "leveldown":
                    this.LevelDown(listing as ResidentialPropertyListing, target);
                    break;
                case "leveldelete":
                    this.LevelDelete(listing as ResidentialPropertyListing, target);
                    break;
                case "roomup":
                    // todo handle the cases when the listing is not a Residential one
                    this.RoomUp(listing as ResidentialPropertyListing, target);
                    break;
                case "roomdown":
                    // todo handle the cases when the listing is not a Residential one
                    this.RoomDown(listing as ResidentialPropertyListing, target);
                    break;
                case "roomdelete":
                    // todo handle the cases when the listing is not a Residential one
                    this.RoomDelete(listing as ResidentialPropertyListing, target);
                    break;
                case "imageup":
                    this.ImageUp(listing, target);
                    break;
                case "imagedown":
                    this.ImageDown(listing, target);
                    break;
                case "imagedefault":
                    this.ImageDefault(listing, target);
                    break;
                case "imagedelete":
                    this.ImageDelete(listing, target);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The opperation '{0}' is not supported",opperation));
            }

            #region Remove Cached Transforms
            System.Collections.Generic.List<object> keysToRemove = new System.Collections.Generic.List<object>();
            foreach (object obj in listing.Objects.Keys)
                if (obj is TransformKey)
                    keysToRemove.Add(obj);

            foreach (object obj in keysToRemove)
                listing.Objects.Remove(obj);
            #endregion

            return ListingService.TransformListing(listing, string.Empty);
        }

        #region Opperations

        #region Room Opperations
        /// <summary>
        /// Moves a level up in the list
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="levelID">The ID of the level to move</param>
        private void RoomUp(Inaugura.RealLeads.ResidentialPropertyListing listing, string roomID)
        {
            Guid guid = new Guid(roomID);
            Level level = listing.Levels.GetLevelByRoom(guid);
            if (level == null)
                throw new ApplicationException("The room could not be found");

            level.Rooms.MoveUp(guid);
            Helper.API.ListingManager.UpdateListing(listing);            
        }

        /// <summary>
        /// Moves a level down in the list
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="levelID">The ID of the level to move</param>
        private void RoomDown(Inaugura.RealLeads.ResidentialPropertyListing listing, string roomID)
        {
            Guid guid = new Guid(roomID);
            Level level = listing.Levels.GetLevelByRoom(guid);
            if (level == null)
                throw new ApplicationException("The room could not be found");

            level.Rooms.MoveDown(guid);
            Helper.API.ListingManager.UpdateListing(listing);
        }

        /// <summary>
        /// Deletes a level
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="levelID">The ID of the level to move</param>
        private void RoomDelete(Inaugura.RealLeads.ResidentialPropertyListing listing, string roomID)
        {
            Guid guid = new Guid(roomID);
            Level level = listing.Levels.GetLevelByRoom(guid);
            if (level == null)
                throw new ApplicationException("The room could not be found");

            level.Rooms.Remove(level.Rooms[guid]);
            Helper.API.ListingManager.UpdateListing(listing);
        }       
        #endregion

        #region Level Opperations
        /// <summary>
        /// Moves a level up in the list
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="levelID">The ID of the level to move</param>
        private void LevelUp(Inaugura.RealLeads.ResidentialPropertyListing listing, string levelID)
        {
            Level level = listing.Levels[levelID];
            if (level == null)
                throw new ApplicationException("The level could not be found");

            listing.Levels.MoveUp(level);
            Helper.API.ListingManager.UpdateListing(listing);
        }        

        /// <summary>
        /// Moves a level up in the list
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="levelID">The ID of the level to move</param>
        private void LevelDown(Inaugura.RealLeads.ResidentialPropertyListing listing, string levelID)
        {
            Level level = listing.Levels[levelID];
            if (level == null)
                throw new ApplicationException("The level could not be found");

            listing.Levels.MoveDown(level);
            Helper.API.ListingManager.UpdateListing(listing);
        }

        /// <summary>
        /// Deletes a level
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="levelID">The ID of the level to move</param>
        private void LevelDelete(Inaugura.RealLeads.ResidentialPropertyListing listing, string levelID)
        {
            Level level = listing.Levels[levelID];
            if (level == null)
                throw new ApplicationException("The level could not be found");

            listing.Levels.Remove(level);
            Helper.API.ListingManager.UpdateListing(listing);
        }
        #endregion

        #region Images Opperations
        /// <summary>
        /// Moves a image up in the list
        /// </summary>
        /// <param name="listing">The image</param>
        /// <param name="levelID">The ID of the image to move</param>
        private void ImageUp(Inaugura.RealLeads.Listing listing, string imageID)
        {
            Image image = listing.Images[imageID];
            if (image == null)
                throw new ApplicationException("The image could not be found");

            listing.Images.MoveUp(image);
            Helper.API.ListingManager.UpdateListing(listing);
        }

        /// <summary>
        /// Moves a image down in the list
        /// </summary>
        /// <param name="listing">The image</param>
        /// <param name="levelID">The ID of the image to move</param>
        private void ImageDown(Inaugura.RealLeads.Listing listing, string imageID)
        {
            Image image = listing.Images[imageID];
            if (image == null)
                throw new ApplicationException("The image could not be found");

            listing.Images.MoveDown(image);
            Helper.API.ListingManager.UpdateListing(listing);
        }

        /// <summary>
        /// Moves a image down in the list
        /// </summary>
        /// <param name="listing">The image</param>
        /// <param name="levelID">The ID of the image to move</param>
        private void ImageDefault(Inaugura.RealLeads.Listing listing, string imageID)
        {
            Image image = listing.Images[imageID];
            if (image == null)
                throw new ApplicationException("The image could not be found");

            listing.Images.DefaultImage = image;
            Helper.API.ListingManager.UpdateListing(listing);
        }

        /// <summary>
        /// Removes an image from the listing
        /// </summary>
        /// <param name="listing">The listing</param>
        /// <param name="imageID">The image ID</param>
        private void ImageDelete(Inaugura.RealLeads.Listing listing, string imageID)
        {
            listing.RemoveImage(Helper.API, new Guid(imageID));            
        }
        #endregion

        #endregion
        #endregion
        #endregion

        #region Geocoding
        #region Internal Constructs
        /// <summary>
        /// A class which contains the results of a geocode inquery
        /// </summary>
        public class GeocodeResult
        {
            #region Variables
            /// <summary>
            /// The latitude
            /// </summary>
            public double Latitude;
            /// <summary>
            /// The longitude
            /// </summary>
            public double Longitude;
            /// <summary>
            /// The address
            /// </summary>
            public string Address;
            /// <summary>
            /// The City
            /// </summary>
            public string City;
            /// <summary>
            /// The State/Prov
            /// </summary>
            public string State;
            /// <summary>
            /// The Zip code
            /// </summary>
            public string Zip;
            /// <summary>
            /// The country
            /// </summary>
            public string Country;
            /// <summary>
            /// A message about the result
            /// </summary>
            public string Message;
            #endregion

            #region Methods
            /// <summary>
            /// Constructor
            /// </summary>
            public GeocodeResult()
            {
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// Gets a listing
        /// </summary>
        /// <param name="id">The listing ID</param>
        /// <param name="mode">The mode</param>
        /// <returns>The listing content</returns>
        [WebMethod]
        public GeocodeResult GetGeocode(string address)
        {
            string message = string.Empty;
            Address addr = GeocodeHelper.Geocode(address, out message);
            GeocodeResult result = new GeocodeResult();
            result.Address = addr.Street;
            result.City = addr.City;
            result.State = addr.StateProv;
            result.Country = addr.Country;
            result.Zip = addr.ZipPostal;
            result.Latitude = addr.Latitude;
            result.Longitude = addr.Longitude;
            result.Message = message;
            return result;
        }
        #endregion       

        /// <summary>
        /// Gets the pins for the listings in a certain area
        /// </summary>
        /// <param name="latitude">The longitude</param>
        /// <param name="longitude">The latitude</param>
        /// <returns>The array of pins in that area</returns>
        [WebMethod]
        public Pin[] GetPins(double latitude, double longitude)
        {
            PropertySearch search;
            if (Helper.Configuration.Mode == Helper.Configuration.WebSiteMode.RentalPropertyListings)
                search = new RentalPropertySearch();
            else
                search = new RealEstateSearch();

            search.Address.Longitude = longitude;
            search.Address.Latitude = latitude;
            search.Radius = 10;
            search.StartIndex = 1;
            search.EndIndex = 50;

            Inaugura.RealLeads.Listing[] listings = Helper.API.ListingManager.SearchListings(search);
            System.Collections.Generic.List<Pin> pins = new System.Collections.Generic.List<Pin>();
            foreach (Listing listing in listings)
            {
                if(listing is PropertyListing)
                {
                    PropertyListing pl = listing as PropertyListing;
                    Pin p = new Pin();
                    p.longitude = pl.Address.Longitude;
                    p.latitude = pl.Address.Latitude;
                    p.description = WebServices.ListingService.TransformListing(listing, "Pin");
                    p.title = pl.Title;
                    p.icon = GetIcon(listing);
                    pins.Add(p);
                }
            }
            return pins.ToArray();
        }

        private string GetIcon(Listing listing)
        {
            if (listing is RentalPropertyListing)
            {
                RentalPropertyListing pl = listing as RentalPropertyListing;
                if (pl.PropertyType == Inaugura.RealLeads.RentalPropertyType.House)
                {
                    if (pl.ParkingIncluded)
                        return "mapHome_A.png";
                    else
                        return "mapHome.png";
                }                
                else
                {
                    if (pl.ParkingIncluded)
                        return "mapCondo_A.png";
                    else
                        return "mapCondo.png";
                }
                
            }
            return string.Empty;
        }

        [WebMethod(true)]
        public Pin GetSearchPin()
        {
            CachedSearch search = Helper.Session.Search;
            if (search != null)
            {
                PropertySearch ps = search.Search as PropertySearch;
                if (ps != null && ps.Address.Latitude != 0)
                {
                    Pin pin = new Pin();
                    pin.title = ps.Address.Label;
                    pin.latitude = ps.Address.Latitude;
                    pin.longitude = ps.Address.Longitude;
                    pin.icon = "mapTarget.png";
                    return pin;
                }
            }
            return null;
        }

        [WebMethod(true)]
        public Pin GetListingPin(string listingID)
        {
            Guid id = new Guid(listingID);
            PropertyListing listing = Helper.API.ListingManager.GetListing(id) as PropertyListing;
            return this.GetPin(listing);
        }
                    
        /// <summary>
        /// Gets the list of pins for the users current search
        /// </summary>
        /// <returns>The list of pins</returns>
        [WebMethod(true)]
        public Pin[] GetSearchResultPins()
        {
            CachedSearch search = Helper.Session.Search;

            if (search == null)
                return new Pin[0];

            System.Collections.Generic.List<Pin> pins = new System.Collections.Generic.List<Pin>();
            foreach (Listing listing in search.CachedResults)
            {
                if(listing is PropertyListing)
                {
                    PropertyListing pl = listing as PropertyListing;
                    pins.Add(GetPin(pl));
                }
            }
            return pins.ToArray();
        }

        private Pin GetPin(PropertyListing listing)
        {
            Pin p = new Pin();
            p.id = listing.ID.ToString();
            p.longitude = listing.Address.Longitude;
            p.latitude = listing.Address.Latitude;
            p.description = WebServices.ListingService.TransformListing(listing, "Pin");
            p.title = listing.Title;
            p.icon = GetIcon(listing);
            return p;
        }
    }
}