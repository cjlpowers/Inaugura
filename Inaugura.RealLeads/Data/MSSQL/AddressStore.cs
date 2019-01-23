#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Inaugura.Maps;

using Inaugura.Data;
#endregion

namespace Inaugura.RealLeads.Data
{
    public class AddressStore : SqlDataStore, IAddressStore
	{
		#region Variables
        private IRealLeadsDataAdaptor mAdaptor;
		#endregion

        public AddressStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString)
            : base(connectionString)
		{
			this.mAdaptor = dataAdaptor;
		}

        internal static Country GetCountryFromReader(SqlDataReader reader)
        {
            return new Country(reader["Name"].ToString(), reader["ID"].ToString());
        }

        internal static Province GetProvinceFromReader(SqlDataReader reader)
        {
            return new Province(reader["Name"].ToString(), reader["CountryID"].ToString(), reader["ID"].ToString());
        }

        internal static City GetCityFromReader(SqlDataReader reader)
        {
            City city = new City(reader["Name"].ToString(), reader["ProvinceID"].ToString(), reader["ID"].ToString());
            city.Latitude = Convert.ToDouble(reader["Latitude"]);
            city.Longitude = Convert.ToDouble(reader["Longitude"]);
            return city;
        }

        internal static Locale GetLocaleFromReader(SqlDataReader reader, string field)
        {
            string xml = Convert.ToString(reader[field]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return new Locale(xmlDoc["Locale"]);
        }
   
        #region IAddressStore Members
        #region Countries
        /// <summary>
        /// Gets a Zone
        /// </summary>
        /// <param name="id">The ID of the country</param>
        /// <returns>The country matching the specified ID, otherwise null</returns>
        public Country GetCountry(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetCountry";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetCountryFromReader(reader);
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
        /// Gets a list of all Countries
        /// </summary>
        /// <returns>A list of Countries</returns>
        public Country[] GetCountries()
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetCountries";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Country> list = new List<Country>();
                        while (reader.Read())
                        {
                            list.Add(GetCountryFromReader(reader));
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
        #endregion

        #region Provinces
        /// <summary>
        /// Gets a Province
        /// </summary>
        /// <param name="id">The ID of the province</param>
        /// <returns>The province matching the specified ID, otherwise null</returns>
        public Province GetProvince(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetProvince";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetProvinceFromReader(reader);
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
        /// Gets a list of all Provinces
        /// </summary>
        /// <param name="countryID">The id of the country</param>
        /// <returns>A list of all provinces from a specific country</returns>
        public Province[] GetProvinces(Guid countryID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetProvinces";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@countryID", countryID)); 
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Province> list = new List<Province>();
                        while (reader.Read())
                        {
                            list.Add(GetProvinceFromReader(reader));
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
        #endregion

        #region Cities
        /// <summary>
        /// Gets a City
        /// </summary>
        /// <param name="id">The ID of the city</param>
        /// <returns>The city matching the specified ID, otherwise null</returns>
        public City GetCity(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetCity";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id)); 

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetCityFromReader(reader);
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
        /// Gets a list of all Cities from a province
        /// </summary>
        /// <param name="provinceID">The id of the region</param>
        /// <returns>A list of all cities from a specific region</returns>
        public City[] GetCities(Guid provinceID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetCities";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@provinceID", provinceID)); 

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<City> list = new List<City>();
                        while (reader.Read())
                            list.Add(GetCityFromReader(reader));
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
        /// Gets a list of all major Cities from a specific province
        /// </summary>
        /// <param name="regionID">The id of the region</param>
        /// <returns>A list of all major cities from a specific province</returns>
        public City[] GetMajorCities(Guid provinceID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_GetMajorCities";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@provinceID", provinceID)); 

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<City> list = new List<City>();
                        while (reader.Read())
                        {
                            list.Add(GetCityFromReader(reader));
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
        #endregion

        #region Locales
        /// <summary>
        /// Gets a Locale
        /// </summary>
        /// <param name="id">The ID of the Locale</param>
        /// <returns>The locale matching the specified ID, otherwise null</returns>
        public Locale GetLocale(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Locales_GetLocale";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id)); 

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetLocaleFromReader(reader, "xml");
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
        /// Gets a list of Locales for a specific area
        /// </summary>
        /// <param name="parentID">The id of the country, province, or city which contains the locale</param>
        /// <param name="type">The locale types</param>
        /// <returns>A list of all districts from a specific city</returns>
        public Locale[] GetLocales(Guid parentID, Locale.LocaleType type)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Locales_GetLocales";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(parentID != Guid.Empty)
                        cmd.Parameters.Add(new SqlParameter("@parentID", parentID));
                    cmd.Parameters.Add(new SqlParameter("@localeType", (int)type));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Locale> list = new List<Locale>();
                        while (reader.Read())
                        {
                            list.Add(GetLocaleFromReader(reader, "xml"));
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

        /// <summary>
        /// Gets a list of Locales
        /// </summary>
        /// <param name="type">The locale types</param>
        /// <returns>A list of all districts from a specific city</returns>
        public Locale[] GetLocales(Locale.LocaleType type)
        {
            return this.GetLocales(Guid.Empty, type);
        }


        /// <summary>
        /// Adds a Locale
        /// </summary>
        /// <param name="locale">The district to add</param>
        public void AddLocale(Locale locale)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Locales_AddLocale";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@xml", new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(locale.Xml.OuterXml, XmlNodeType.Document, null))));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Removes a Locale
        /// </summary>
        /// <param name="id">The ID of the locale to remove</param>
        public bool RemoveLocale(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Locales_RemoveLocale";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    return (cmd.ExecuteNonQuery() > 0);
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Updates a Locale
        /// </summary>
        /// <param name="district">The district to update</param>
        public bool UpdateLocale(Locale locale)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Locales_UpdateLocale";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@xml", new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(locale.Xml.OuterXml, XmlNodeType.Document, null))));

                    return (cmd.ExecuteNonQuery() > 0);
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        #endregion

        /// <summary>
        /// Locates a postal code
        /// </summary>
        /// <param name="postal">The postal code</param>
        /// <returns>The address located at the center of the specified postal code, otherwise null</returns>
        public Inaugura.Maps.Address LocatePostal(string postal)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Address_Geocode";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@postalCode",postal));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetAddressFromReader(reader);
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

        private static Inaugura.Maps.Address GetAddressFromReader(SqlDataReader reader)
        {
            Inaugura.Maps.Address address = new Inaugura.Maps.Address();
            address.ZipPostal = Convert.ToString(reader["postalcode"]);
            address.City = Convert.ToString(reader["city"]);
            address.StateProv = Convert.ToString(reader["province"]);
            address.Country = Convert.ToString(reader["country"]);
            address.Latitude = Convert.ToDouble(reader["latitude"]);
            address.Longitude = Convert.ToDouble(reader["longitude"]);
            address.CityID = Convert.ToString(reader["CityID"]);
            address.StateProvID = Convert.ToString(reader["ProvinceID"]);
            address.CountryID = Convert.ToString(reader["CountryID"]);
            return address;
        }
        #endregion
    }
}
