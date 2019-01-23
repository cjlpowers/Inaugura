using System;
using Inaugura.Maps;


namespace Inaugura.RealLeads.Data
{

	/// <summary>
	/// Summary description for IListingStore.
	/// </summary>
    public interface IAddressStore
	{
        #region Countries
        /// <summary>
        /// Gets a Country
        /// </summary>
        /// <param name="id">The ID of the country</param>
        /// <returns>The country matching the specified ID, otherwise null</returns>
        Country GetCountry(Guid id);

        /// <summary>
        /// Gets a list of all Countries
        /// </summary>
        /// <returns>A list of Countries</returns>
        Country[] GetCountries();
        #endregion

        #region Provinces
        /// <summary>
        /// Gets a Province
        /// </summary>
        /// <param name="id">The ID of the province</param>
        /// <returns>The province matching the specified ID, otherwise null</returns>
        Province GetProvince(Guid id);

        /// <summary>
        /// Gets a list of all Provinces
        /// </summary>
        /// <param name="countryID">The id of the country</param>
        /// <returns>A list of all provinces from a specific country</returns>
        Province[] GetProvinces(Guid countryID);
        #endregion

        #region Cities
        /// <summary>
        /// Gets a City
        /// </summary>
        /// <param name="id">The ID of the city</param>
        /// <returns>The city matching the specified ID, otherwise null</returns>
        City GetCity(Guid id);

        /// <summary>
        /// Gets a list of all Cities
        /// </summary>
        /// <param name="provinceID">The id of the province</param>
        /// <returns>A list of all cities from a specific region</returns>
        City[] GetCities(Guid provinceID);

        /// <summary>
        /// Gets a list of all Major Cities from a specific province
        /// </summary>
        /// <param name="provinceID">The id of the province</param>
        /// <returns>A list of all major cities from a specific province</returns>
        City[] GetMajorCities(Guid provinceID);
        #endregion

        #region Locales
        /// <summary>
        /// Gets a Locale
        /// </summary>
        /// <param name="id">The ID of the Locale</param>
        /// <returns>The locale matching the specified ID, otherwise null</returns>
        Locale GetLocale(Guid id);

        /// <summary>
        /// Gets a list of Locales for a specific area
        /// </summary>
        /// <param name="parentID">The id of the country, province, or city which contains the locale</param>
        /// <param name="type">The locale types</param>
        /// <returns>A list of all districts from a specific city</returns>
        Locale[] GetLocales(Guid parentID, Locale.LocaleType type);

        /// <summary>
        /// Gets a list of Locales
        /// </summary>
        /// <param name="type">The locale types</param>
        /// <returns>A list of all districts from a specific city</returns>
        Locale[] GetLocales(Locale.LocaleType type);

        /// <summary>
        /// Adds a Locale
        /// </summary>
        /// <param name="locale">The district to add</param>
        void AddLocale(Locale locale);

        /// <summary>
        /// Removes a Locale
        /// </summary>
        /// <param name="id">The ID of the locale to remove</param>
        bool RemoveLocale(Guid id);

        /// <summary>
        /// Updates a Locale
        /// </summary>
        /// <param name="district">The district to update</param>
        bool UpdateLocale(Locale locale);
        #endregion

        #region Postal Codes
        /// <summary>
        /// Locates a postal code
        /// </summary>
        /// <param name="postal">The postal code</param>
        /// <returns>The address located at the center of the specified postal code, otherwise null</returns>
        Address LocatePostal(string postal);
        #endregion        
	}
}
