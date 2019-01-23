#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Inaugura.Data;
#endregion

namespace Inaugura.RealLeads.Data
{
	public class ListingStore : SqlDataStore, IListingStore
	{
		#region Variables
        private IRealLeadsDataAdaptor mAdaptor;
        private string mFileDirectory;
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataAdaptor">The realleads data adaptor</param>
        /// <param name="connectionString">The database connection string</param>
        /// <param name="fileDirectory">The directory where all the files are stored</param>
        public ListingStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString, string fileDirectory) : base(connectionString)
		{
			this.mAdaptor = dataAdaptor;
            this.mFileDirectory = fileDirectory;
		}

		internal static Listing GetListingFromReader(SqlDataReader reader, string field)
		{
			string xml = Convert.ToString(reader[field]);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
            return Inaugura.Xml.Helper.GetIXmlableFromXml(xmlDoc) as Listing;			
		}

		#region IListingStore Members
        public virtual Listing GetListing(Guid id)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.Listings_GetListing";
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							return ListingStore.GetListingFromReader(reader, "xml");
						else
							return null;
					}
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

        public Listing[] GetListings(Guid userId)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.Listings_GetListings";
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", userId));

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						List<Listing> list = new List<Listing>();
						while (reader.Read())
							list.Add(ListingStore.GetListingFromReader(reader, "xml"));
						return list.ToArray();
					}
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

         /// <summary>
        /// Searches RealEstateListings
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The list of realestate listings meeting the search criteria</returns>
        public Listing[] SearchListings(ListingSearch search)
        {
            return SearchListings(search, null);
        }

        /// <summary>
        /// Searches RealEstateListings
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <param name="cache">The cache object</param>
        /// <returns>The list of realestate listings meeting the search criteria</returns>
        public Listing[] SearchListings(ListingSearch search, Caching.Cache cache)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand command = null;
                    if (search is RealEstateSearch)
                        command =  RealEstateListingSearch(search as RealEstateSearch);
                    else if (search is RentalPropertySearch)
                        command = RentalPropertyListingSearch(search as RentalPropertySearch);
                    else 
                        command = BasicListingSearch(search);

                    command.Connection = connection;

                    List<Listing> list = new List<Listing>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            Listing l = null;
                            if(cache != null)
                                l = cache[id] as Listing;
                            if (l == null)
                            {
                                l = ListingStore.GetListingFromReader(reader, "xml");
                                if (cache != null)
                                    cache[id] = l;
                            }
                            list.Add(l);
                        }
                    }

                    //get the total record count
                    int result = Convert.ToInt32(command.Parameters["@retcode"].Value);
                    if (result != 0)
                    {
                        search.ResultCount = result;
                        search.CalculateResultCount = false;
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("Searching  returned {0} listings", list.Count));
                    return list.ToArray();

                }               
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        private SqlCommand RealEstateListingSearch(RealEstateSearch search)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "dbo.RealEstateListings_Search";
            cmd.CommandType = CommandType.StoredProcedure;

            if (search.Status != Listing.ListingStatus.All)
                cmd.Parameters.Add(new SqlParameter("@status", search.Status));

            if (search.UserID != null && search.UserID != Guid.Empty)
                cmd.Parameters.Add(new SqlParameter("@userId", search.UserID));

            if (search.PriceLower != 0)
                cmd.Parameters.Add(new SqlParameter("@priceLower", search.PriceLower));

            if (search.PriceUpper != 0)
                cmd.Parameters.Add(new SqlParameter("@priceUpper", search.PriceUpper));

            if (search.PropertyType != null)
                cmd.Parameters.Add(new SqlParameter("@propertyType", search.PropertyType.Value));

            if (search.MinBedrooms != 0)
                cmd.Parameters.Add(new SqlParameter("@minBedrooms", search.MinBedrooms));

            if (!string.IsNullOrEmpty(search.Address.CityID))
                cmd.Parameters.Add(new SqlParameter("@cityID", search.Address.CityID));

            if (search.Address.Longitude != 0)
                cmd.Parameters.Add(new SqlParameter("@longitude", search.Address.Longitude));

            if (search.Address.Latitude != 0)
                cmd.Parameters.Add(new SqlParameter("@latitude", search.Address.Latitude));

            if (search.Radius != 0)
                cmd.Parameters.Add(new SqlParameter("@radius", search.Radius));

            if (search.MinBathrooms != 0)
                cmd.Parameters.Add(new SqlParameter("@minBathrooms", search.MinBathrooms));

            cmd.Parameters.Add(new SqlParameter("@startIndex", search.StartIndex));
            cmd.Parameters.Add(new SqlParameter("@endIndex", search.EndIndex));

            if (search.CalculateResultCount)
                cmd.Parameters.Add(new SqlParameter("@calculateRecordCount", search.CalculateResultCount));

            cmd.Parameters.Add("@retcode", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;
        }

        private SqlCommand RentalPropertyListingSearch(RentalPropertySearch search)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "dbo.RentalPropertyListings_Search";
            cmd.CommandType = CommandType.StoredProcedure;

            if (search.Status != Listing.ListingStatus.All)
                cmd.Parameters.Add(new SqlParameter("@status", search.Status));

            if (search.UserID != null && search.UserID != Guid.Empty)
                cmd.Parameters.Add(new SqlParameter("@userID", search.UserID));

            if (search.RentLower != 0)
                cmd.Parameters.Add(new SqlParameter("@rentLower", search.RentLower));

            if (search.RentUpper != 0)
                cmd.Parameters.Add(new SqlParameter("@rentUpper", search.RentUpper));

            if (search.PropertyType != null)
                cmd.Parameters.Add(new SqlParameter("@propertyType", search.PropertyType.Value));

            if (search.AvailabilityStart != DateTime.MinValue)
                cmd.Parameters.Add(new SqlParameter("@availabilityStart",search.AvailabilityStart));

            if (search.AvailabilityEnd != DateTime.MaxValue)
                cmd.Parameters.Add(new SqlParameter("@availabilityEnd",search.AvailabilityEnd));
           
            if (search.MinBedrooms != 0)
                cmd.Parameters.Add(new SqlParameter("@minBedrooms", search.MinBedrooms));

            if (search.MinParkingSpaces != 0)
                cmd.Parameters.Add(new SqlParameter("@minParkingSpaces", search.MinParkingSpaces));

            if (search.IncludesElectricity)
                cmd.Parameters.Add(new SqlParameter("@includesElectricity", search.IncludesElectricity));

            if (search.IncludesHeating)
                cmd.Parameters.Add(new SqlParameter("@includesHeating", search.IncludesHeating));

            if (search.LaundryService)
                cmd.Parameters.Add(new SqlParameter("@laundryService", search.LaundryService));

            if (search.InternetService)
                cmd.Parameters.Add(new SqlParameter("@internetService", search.InternetService));

            if (search.TelevisionService)
                cmd.Parameters.Add(new SqlParameter("@televisionService", search.TelevisionService));
            
            if (search.Pets)
                cmd.Parameters.Add(new SqlParameter("@pets", search.Pets));

            if (search.Pool)
                cmd.Parameters.Add(new SqlParameter("@pool", search.Pool));
           
            if (!string.IsNullOrEmpty(search.Address.CityID))
                cmd.Parameters.Add(new SqlParameter("@cityID", search.Address.CityID));

            if (search.Address.Longitude != 0)
                cmd.Parameters.Add(new SqlParameter("@longitude", search.Address.Longitude));

            if (search.Address.Latitude != 0)
                cmd.Parameters.Add(new SqlParameter("@latitude", search.Address.Latitude));

            if (search.Radius != 0)
                cmd.Parameters.Add(new SqlParameter("@radius", search.Radius));

            cmd.Parameters.Add(new SqlParameter("@startIndex", search.StartIndex));
            cmd.Parameters.Add(new SqlParameter("@endIndex", search.EndIndex));

            if (search.CalculateResultCount)
                cmd.Parameters.Add(new SqlParameter("@calculateRecordCount", search.CalculateResultCount));

            cmd.Parameters.Add("@retcode", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;
        }

        private SqlCommand BasicListingSearch(ListingSearch search)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "dbo.Listings_Search";
            cmd.CommandType = CommandType.StoredProcedure;

            if (search.Status != Listing.ListingStatus.All)
                cmd.Parameters.Add(new SqlParameter("@status", search.Status));

            if (search.UserID != null && search.UserID != Guid.Empty)
                cmd.Parameters.Add(new SqlParameter("@userID", search.UserID));

            if (search.ExpirationStart != DateTime.MinValue)
                cmd.Parameters.Add(new SqlParameter("@expirationStart", search.ExpirationStart));

            if (search.ExpirationEnd != DateTime.MaxValue)
                cmd.Parameters.Add(new SqlParameter("@expirationEnd", search.ExpirationEnd));

            cmd.Parameters.Add(new SqlParameter("@startIndex", search.StartIndex));
            cmd.Parameters.Add(new SqlParameter("@endIndex", search.EndIndex));

            if (search.CalculateResultCount)
                cmd.Parameters.Add(new SqlParameter("@calculateRecordCount", search.CalculateResultCount));

            cmd.Parameters.Add("@retcode", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            return cmd;
        }

        /// <summary>
        /// Gets a featured listing
        /// </summary>
       /// <param name="listingType">The type of listing</param>
        /// <returns></returns>
        public virtual Listing GetFeaturedListing(Type listingType)
        {
            if (listingType != typeof(Inaugura.RealLeads.RentalPropertyListing))
                throw new NotSupportedException("The type of listing is not supported");
            
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.RentalPropertyListings_GetFeature";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ListingStore.GetListingFromReader(reader, "xml");
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

		public virtual void Add(Listing listing)
		{
			this.AddGenericListing(listing);

			// if the listing is a real estate listing add it the the details table
			//if (listing is RealEstateListing)
				//this.AddRealEstateListing(listing as RealEstateListing);							
		}

		private void AddGenericListing(Listing listing)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.Listings_AddListing";
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", listing.ID));
					cmd.Parameters.Add("@expirationDate", SqlDbType.DateTime);
					cmd.Parameters.Add("@xml", SqlDbType.Xml);


					cmd.Parameters["@expirationDate"].Value = listing.ExpirationDate;
					cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(listing.Xml.OuterXml, XmlNodeType.Document, null));
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		private void AddRealEstateListing(RealEstateListing listing)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.RealEstateListingDetails_AddListing";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@lotArea", SqlDbType.Int);
					cmd.Parameters.Add("@price", SqlDbType.Int);
					cmd.Parameters.Add("@livingArea", SqlDbType.Int);
					cmd.Parameters.Add("@countryID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@provinceID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@regionID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@cityID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@districtID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@street", SqlDbType.VarChar,255);
					cmd.Parameters.Add("@numberOfBedrooms", SqlDbType.Int);
					cmd.Parameters.Add("@numberOfBathrooms", SqlDbType.Int);
					cmd.Parameters.Add("@propertyTax", SqlDbType.Int);
					cmd.Parameters.Add("@hasGararge", SqlDbType.Bit);
					cmd.Parameters.Add("@garargeParkingSpaces", SqlDbType.Int);
					cmd.Parameters.Add("@garargeFloorArea", SqlDbType.Int);
					cmd.Parameters.Add("@garargeAttached", SqlDbType.Int);
					cmd.Parameters.Add("@hasFireplace", SqlDbType.Bit);
					cmd.Parameters.Add("@hasPool", SqlDbType.Bit);
					cmd.Parameters.Add("@hasWaterfront", SqlDbType.Bit);
					cmd.Parameters.Add("@hasBackyardNeighbors", SqlDbType.Bit);
					cmd.Parameters.Add("@propertyType", SqlDbType.Int);
					cmd.Parameters.Add("@roofType", SqlDbType.Int);
					cmd.Parameters.Add("@foundationType", SqlDbType.Int);
					cmd.Parameters.Add("@heatingPrimary", SqlDbType.Int);
					cmd.Parameters.Add("@heatingSecondary", SqlDbType.Int);
					cmd.Parameters.Add("@exteriorPrimary", SqlDbType.Int);
					cmd.Parameters.Add("@exteriorSecondary", SqlDbType.Int);
					cmd.Parameters.Add("@electricalService", SqlDbType.Int);
					cmd.Parameters.Add("@basementType", SqlDbType.Int);

					cmd.Parameters["@ID"].Value = listing.ID;
					cmd.Parameters["@lotArea"].Value = listing.Lot.Size.ToStandardUnit().Value;
					cmd.Parameters["@price"].Value = listing.Price;
					cmd.Parameters["@livingArea"].Value = listing.Size.ToStandardUnit().Value;
					cmd.Parameters["@countryID"].Value = listing.Address.CountryID;
					cmd.Parameters["@provinceID"].Value = listing.Address.StateProvID;
					cmd.Parameters["@regionID"].Value = listing.Address.RegionID;
					cmd.Parameters["@cityID"].Value = listing.Address.CityID;
					cmd.Parameters["@districtID"].Value = listing.Address.DistrictID;
					cmd.Parameters["@street"].Value = listing.Address.Street;
					cmd.Parameters["@numberOfBedrooms"].Value = listing.NumberOfBedrooms;
					cmd.Parameters["@numberOfBathrooms"].Value = listing.NumberOfBathrooms;
					cmd.Parameters["@propertyTax"].Value = listing.PropertyTax.Value;
					if (listing.Gararge != null)
					{
						cmd.Parameters["@hasGararge"].Value = true;
						cmd.Parameters["@garargeParkingSpaces"].Value = listing.Gararge.ParkingSpaces;
						cmd.Parameters["@garargeFloorArea"].Value = listing.Gararge.Dimensions.Area.ToStandardUnit().Value;
						cmd.Parameters["@garargeAttached"].Value = listing.Gararge.ParkingSpaces;
					}
					else
					{
						cmd.Parameters["@hasGararge"].Value = false;
						cmd.Parameters["@garargeParkingSpaces"].Value = DBNull.Value;
						cmd.Parameters["@garargeFloorArea"].Value = DBNull.Value;
						cmd.Parameters["@garargeAttached"].Value = DBNull.Value;
					}

					cmd.Parameters["@hasFireplace"].Value = listing.Fireplace;
					cmd.Parameters["@hasPool"].Value = listing.Pool;
					cmd.Parameters["@hasWaterfront"].Value = listing.Waterfront;
					cmd.Parameters["@hasBackyardNeighbors"].Value = listing.BackyardNeighbors;

					cmd.Parameters["@propertyType"].Value = listing.PropertyType.Value;
					cmd.Parameters["@roofType"].Value = listing.RoofType.Value;
					cmd.Parameters["@foundationType"].Value = listing.FoundationType.Value;
					cmd.Parameters["@heatingPrimary"].Value = listing.HeatingPrimary.Value;
					cmd.Parameters["@heatingSecondary"].Value = listing.HeatingSecondary.Value;
					cmd.Parameters["@exteriorPrimary"].Value = listing.ExteriorPrimary.Value;
					cmd.Parameters["@exteriorSecondary"].Value = listing.ExteriorSecondary.Value;
					cmd.Parameters["@electricalService"].Value = listing.ElectricalService.Value;
					cmd.Parameters["@basementType"].Value = listing.BasementType.Value;
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

        public virtual bool Remove(Guid id)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.Listings_RemoveListing";
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@listingID", id));

					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public virtual bool Update(Listing listing)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.Listings_UpdateListing";
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", listing.ID));
					cmd.Parameters.Add("@expirationDate", SqlDbType.DateTime);
					cmd.Parameters.Add("@xml", SqlDbType.Xml);

					cmd.Parameters["@expirationDate"].Value = listing.ExpirationDate;
					cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(listing.Xml.OuterXml, XmlNodeType.Document, null));

					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

        /// <summary>
        /// Gets a Listing
        /// </summary>
        /// <param name="zoneID">The ID of the Zone to which the Listing belongs</param>
        /// <param name="code">The code of the Listing</param>
        /// <returns></returns>
        public virtual Listing GetListingByCode(string code)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Listings_GetListingByCode";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@code", SqlDbType.VarChar, 5);

                    cmd.Parameters["@code"].Value = code;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ListingStore.GetListingFromReader(reader, "xml");
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Gets a list of unused listing codes
        /// </summary>
        /// <param name="maxCodes">The maximum number of codes to return</param>
        /// <returns>A list of unused listing codes</returns>
        public string[] GetUnusedCodes(int maxCodes)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Listings_GetUnusedCodes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maxCodes", SqlDbType.Int);
                    cmd.Parameters["@maxCodes"].Value = maxCodes;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<string> list = new List<string>();
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                        return list.ToArray();
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
            

		#region Listing Files

        private string FilePath(string fileName)
        {
            return System.IO.Path.Combine(this.mFileDirectory, fileName);
        }

		/// <summary>
		/// Adds a file to an Listing's profile
		/// </summary>
		/// <param name="listingID">The Listing ID</param>
		/// <param name="file">The File to add</param>
        public void AddFile(Guid listingID, File file)
        {
            // try adding the file to the directory
            System.IO.File.WriteAllBytes(FilePath(file.FileName), file.Data);
            FileStoreHelper.AddFile(file, this.ConnectionString, "dbo.ListingFiles_AddFile", "@listingID", listingID);
        }

		/// <summary>
		/// Gets a File
		/// </summary>
		/// <param name="fileID">The ID of the File</param>
		/// <returns>The File with the specified ID</returns>
        public File GetFile(Guid fileId)
		{
            // see if the file exists            
			File file = FileStoreHelper.GetFile(this.ConnectionString, "dbo.ListingFiles_GetFile", fileId);
            if (file.Data.Length == 0 && System.IO.File.Exists(FilePath(file.FileName)))
            {
                file.Data = System.IO.File.ReadAllBytes(FilePath(file.FileName));
            }
            return file;
		}

		/// <summary>
		/// Removes a File
		/// </summary>
		/// <param name="fileID">The ID of the File</param>
		/// <returns>The File with the specified ID</returns>
        public bool RemoveFile(Guid fileId)
		{
            File file = GetFile(fileId);
            if(file != null && System.IO.File.Exists(FilePath(file.FileName)))
                System.IO.File.Delete(FilePath(file.FileName));                
			return FileStoreHelper.RemoveFile(this.ConnectionString, "dbo.ListingFiles_RemoveFile", fileId);
		}

		/// <summary>
		/// Updates a File
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public bool UpdateFile(File file)
		{
            if (FileStoreHelper.UpdateFile(file, this.ConnectionString, "dbo.ListingFiles_UpdateFile"))
            {
                System.IO.File.WriteAllBytes(FilePath(file.FileName), file.Data);
                return true;
            }
            return false;
		}
		#endregion

		#endregion

		#region Valid Codes
		public void AddValidCode(string code)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.ValidCodes_AddCode";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@code", SqlDbType.VarChar, 10);

					cmd.Parameters["@code"].Value = code;

					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}
		#endregion

        #region Listing Archive
        /// <summary>
        /// Adds a listing no longer in use to the listing archive
        /// </summary>
        /// <param name="listing">The listing</param>
        public void AddArchivedListing(Listing listing)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.ListingsArchive_AddListing";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(listing.Xml.OuterXml, XmlNodeType.Document, null));

                    // execute the command
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException("The listing could not be added to the database");    
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        #endregion

        #region Listing Abuse
        public AbuseNotification[] SearchAbuseNotifications(AbuseNotificationSearch search)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.AbuseNotifications_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(search.Xml.OuterXml, XmlNodeType.Document, null));
                    cmd.Parameters.Add(new SqlParameter("@startIndex", search.StartIndex));
                    cmd.Parameters.Add(new SqlParameter("@endIndex", search.EndIndex));
                    cmd.Parameters.Add(new SqlParameter("@calculateRecordCount", search.CalculateResultCount));
                    cmd.Parameters.Add("@retcode", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    List<AbuseNotification> list = new List<AbuseNotification>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string xml = Convert.ToString(reader["xml"]);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(xml);

                            AbuseNotification item = new AbuseNotification(xmlDoc.DocumentElement);                          
                            list.Add(item);
                        }
                    }

                    //get the total record count
                    int result = Convert.ToInt32(cmd.Parameters["@retcode"].Value);
                    if (result != 0)
                    {
                        search.ResultCount = result;
                        search.CalculateResultCount = false;
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("Searching  returned {0} abuse notifications", list.Count));
                    return list.ToArray();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        internal static AbuseNotification GetAbuseNotificationFromReader(SqlDataReader reader, string field)
        {
            string xml = Convert.ToString(reader[field]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return Inaugura.Xml.Helper.GetIXmlableFromXml(xmlDoc) as AbuseNotification;
        }

        public void AddAbuseNotification(AbuseNotification notification)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.AbuseNotifications_AddNotification";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(notification.Xml.OuterXml, XmlNodeType.Document, null));

                    // execute the command
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException("The abuse notification could not be added to the database");
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        
        public void UpdateAbuseNotification(AbuseNotification notification)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.AbuseNotifications_UpdateNotification";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(notification.Xml.OuterXml, XmlNodeType.Document, null));

                    // execute the command
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException("The abuse notification could not be updated in the database");
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public void RemoveAbuseNotification(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.AbuseNotifications_RemoveNotification";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    // execute the command
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException("The abuse notification could not be removed from the database");
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        #endregion
	}
}
