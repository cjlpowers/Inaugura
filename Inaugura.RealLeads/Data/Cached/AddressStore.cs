#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Inaugura.Maps;

using Inaugura.Data;
#endregion

namespace Inaugura.RealLeads.Data.Cached
{
    /// <summary>
    /// A cached implementation of the address store
    /// </summary>
    internal class AddressStore : CachedStore, IAddressStore
    {
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The cached adaptor</param>
        public AddressStore(CachedAdaptor adaptor)
            : base(adaptor)
		{
        }
        #endregion


        #region IAddressStore Members

        public Country GetCountry(Guid id)
        {
            Key key = CreateCacheKey(id);
            Country item = Cache[key] as Country;
            if (item == null)
            {
                item = this.Data.AddressStore.GetCountry(id);
                Cache[key] = item;
            }
            return item;
        }

        public Country[] GetCountries()
        {
            Key key = CreateCacheKey("countries");
            Country[] item = Cache[key] as Country[];
            if (item == null)
            {
                item = this.Data.AddressStore.GetCountries();
                Cache[key] = item;
            }
            return item;
        }

        public Province GetProvince(Guid id)
        {
            Key key = CreateCacheKey(id);
            Province item = Cache[key] as Province;
            if (item == null)
            {
                item = this.Data.AddressStore.GetProvince(id);
                Cache[key] = item;
            }
            return item;
        }

        public Province[] GetProvinces(Guid countryID)
        {
            Key key = CreateCacheKey("provinces", countryID);
            Province[] item = Cache[key] as Province[];
            if (item == null)
            {
                item = this.Data.AddressStore.GetProvinces(countryID);
                Cache[key] = item;
            }
            return item;
        }

        public City GetCity(Guid id)
        {
            Key key = CreateCacheKey(id);
            City item = Cache[key] as City;
            if (item == null)
            {
                item = this.Data.AddressStore.GetCity(id);
                Cache[key] = item;
            }
            return item;
        }

        public City[] GetCities(Guid provinceID)
        {
            Key key = CreateCacheKey("cities", provinceID);
            City[] item = Cache[key] as City[];
            if (item == null)
            {
                item = this.Data.AddressStore.GetCities(provinceID);
                Cache[key] = item;
            }
            return item;
        }

        public City[] GetMajorCities(Guid provinceID)
        {
            Key key = CreateCacheKey("majorcities", provinceID);
            City[] item = Cache[key] as City[];
            if (item == null)
            {
                item = this.Data.AddressStore.GetMajorCities(provinceID);
                Cache[key] = item;
            }
            return item;
        }

        public Locale GetLocale(Guid id)
        {
            Key key = CreateCacheKey(id);
            Locale item = Cache[key] as Locale;
            if (item == null)
            {
                item = this.Data.AddressStore.GetLocale(id);
                Cache[key] = item;
            }
            return item;
        }

        public Locale[] GetLocales(Guid parentID, Locale.LocaleType type)
        {
            Key key = CreateCacheKey("locales", parentID, type);
            Locale[] item = Cache[key] as Locale[];
            if (item == null)
            {
                item = this.Data.AddressStore.GetLocales(parentID, type);
                Cache[key] = item;
            }
            return item;
        }

        public Locale[] GetLocales(Locale.LocaleType type)
        {
            Key key = CreateCacheKey("locales", type);
            Locale[] item = Cache[key] as Locale[];
            if (item == null)
            {
                item = this.Data.AddressStore.GetLocales(type);
                Cache[key] = item;
            }
            return item;
        }

        public void AddLocale(Locale locale)
        {
            this.Data.AddressStore.AddLocale(locale);
            Key key = CreateCacheKey("locales", locale.Address.StateProvID, locale.Type);
            Cache.Remove(key);
            key = CreateCacheKey("locales", locale.Type);
            Cache.Remove(key);
        }

        public bool RemoveLocale(Guid id)
        {
            bool result = this.Data.AddressStore.RemoveLocale(id);
            Key key = CreateCacheKey(id);
            Cache.Remove(key);
            return result;
        }

        public bool UpdateLocale(Locale locale)
        {
            bool result = this.Data.AddressStore.UpdateLocale(locale);
            Key key = CreateCacheKey(locale.ID);
            Cache.Remove(key);
            key = CreateCacheKey("locales", locale.Address.StateProvID, locale.Type);
            Cache.Remove(key);
            key = CreateCacheKey("locales", locale.Type);
            Cache.Remove(key);
            return result;
        }

        public Address LocatePostal(string postal)
        {
            Key key = CreateCacheKey("postal",postal);
            Address item = Cache[key] as Address;
            if (item == null)
            {
                item = this.Data.AddressStore.LocatePostal(postal);
                Cache[key] = item;
            }
            return item;
        }
        #endregion
    }
}
