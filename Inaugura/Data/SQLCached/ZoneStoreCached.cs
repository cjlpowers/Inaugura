using System;
using System.Collections.Generic;
using System.Text;
using Inaugura.Maps;

namespace Inaugura.Data
{
    /// <summary>
    ///  A hybrid Zone Store which includes caching functionality
    /// </summary>
    public class ZoneStoreCached : ZoneStore, IDisposable
    {
        #region Varaibles
        private Inaugura.Data.Caching.Cache<Zone> mZoneCache;
        private Inaugura.Data.Caching.Cache<District> mDistrictCache;
        private bool mDisposed = false;
        #endregion

        #region Properties
        /// <summary>
        /// The zone cache
        /// </summary>
        protected Inaugura.Data.Caching.Cache<Zone> ZoneCache
        {
            get
            {
                return this.mZoneCache;
            }
            private set
            {
                this.mZoneCache = value;
            }
        }

        /// <summary>
        /// The district cache
        /// </summary>
        protected Inaugura.Data.Caching.Cache<District> DistrictCache
        {
            get
            {
                return this.mDistrictCache;
            }
            private set
            {
                this.mDistrictCache = value;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adaptor">The data adaptor</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="cacheDirectory">The cache directory</param>
        public ZoneStoreCached(IDataAdaptor adaptor, string connectionString, string cacheDirectory)
            : base(adaptor, connectionString)
        {
            this.ZoneCache = new Inaugura.Data.Caching.Cache<Zone>(cacheDirectory);
            this.DistrictCache = new Inaugura.Data.Caching.Cache<District>(System.IO.Path.Combine(cacheDirectory, "Districts"));
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ZoneStoreCached()
        {
            this.Dispose(false);
        }
        
        #region Districts
        /// <summary>
        /// Gets a list of Districts
        /// </summary>
        /// <param name="cityID">The id of the city</param>
        /// <returns>A list of all districts from a specific city</returns>
        public override District[] GetDistricts(string cityID)
        {
            try
            {
                District[] districts = base.GetDistricts(cityID);
                if (districts.Length > 0)
                    this.DistrictCache.Store(districts);
                return districts;
            }
            catch (Inaugura.Data.DataException exception) // some sort of data exception
            {
                Inaugura.Log.AddException(exception);

                // try to retrieve the districts from the cache
                District[] allDistricts = this.DistrictCache.ToArray();
                List<District> districts = new List<District>();
                foreach (District district in allDistricts)
                {
                    if (district.CityID == cityID)
                        districts.Add(district);
                }
                return districts.ToArray();
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.mDisposed)
            {
                if (disposing)
                {
                    // dispose any managed resources
                    this.mZoneCache.Dispose();
                    this.DistrictCache.Dispose();
                }
                // release any unmanaged resources
                this.mDisposed = true;
            }
        }

        #endregion
    }
}
