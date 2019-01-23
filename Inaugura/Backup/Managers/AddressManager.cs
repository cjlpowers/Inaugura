using System;
using System.Collections.Generic;
using System.Text;

using Inaugura.Maps;

namespace Inaugura.RealLeads.Managers
{
    public class AddressManager : Manager
    {
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The RealLeads API object</param>
        /// <param name="dataAdaptor">The data adaptor</param>        
        internal AddressManager(RealLeadsAPI api, Data.IRealLeadsDataAdaptor dataAdaptor)
            : base(api, dataAdaptor)
        {
        }

        #region Countries
        /// <summary>
        /// Gets a list of countries
        /// </summary>
        /// <returns>The list of countries</returns>
        public Country[] GetCountries()
        {
            return this.Data.AddressStore.GetCountries();
        }
        #endregion

        #region Provinces
        /// <summary>
        /// Gets a list of provinces for a specific country
        /// </summary>
        /// <param name="countryID">The country ID</param>
        /// <returns>The list of provinces</returns>
        public Province[] GetProvinces(Guid countryID)
        {
            return this.Data.AddressStore.GetProvinces(countryID);
        }
        #endregion

        #region Cities
        /// <summary>
        /// Gets a list of major cities contained in a specific province
        /// </summary>
        /// <param name="provinceID">The province ID</param>
        /// <returns>The list of cities</returns>
        public City[] GetMajorCities(Guid provinceID)
        {
            return this.Data.AddressStore.GetMajorCities(provinceID);
        }

        /// <summary>
        /// Gets a City
        /// </summary>
        /// <param name="id">The ID of the city</param>
        /// <returns>The city if found, otherwise null</returns>
        public City GetCity(Guid id)
        {
            return this.Data.AddressStore.GetCity(id);
        }
        #endregion

        #region Locales
        /// <summary>
        /// Gets a list of locales
        /// </summary>
        /// <param name="parentID">The ID of the parent country, province or city</param>
        /// <param name="type">The locale type</param>
        /// <returns>The list of locales</returns>
        public Locale[] GetLocales(Guid parentID, Locale.LocaleType type)
        {
            return this.Data.AddressStore.GetLocales(parentID, type);
        }

        /// <summary>
        /// Gets a list of locales
        /// </summary>
        /// <param name="type">The locale type</param>
        /// <returns>The list of locales</returns>
        public Locale[] GetLocales(Locale.LocaleType type)
        {
            return GetLocales(Guid.Empty, type);
        }

        /// <summary>
        /// Gets a Locale
        /// </summary>
        /// <param name="id">The ID of the locale</param>
        /// <returns>The locale if found, otherwise null</returns>
        public Locale GetLocale(Guid id)
        {
            return this.Data.AddressStore.GetLocale(id);
        }


        /// <summary>
        /// Adds a locale
        /// </summary>
        /// <param name="locale">The locale to add</param>        
        public void AddLocale(Locale locale)
        {
            this.Data.AddressStore.AddLocale(locale);
        }
        #endregion

        #region Postal
        /// <summary>
        /// Locates a postal code
        /// </summary>
        /// <param name="postal">The postal code</param>
        /// <returns>The address of the postal code if found, otherwise null</returns>
        public Address LocatePostal(string postal)
        {
            return this.Data.AddressStore.LocatePostal(postal);
        }
        #endregion

        #endregion
    }
}
