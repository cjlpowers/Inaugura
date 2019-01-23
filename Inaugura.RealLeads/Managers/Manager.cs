using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads.Managers
{
    /// <summary>
    /// A abstract class for all managers
    /// </summary>
    public abstract class Manager
    {
        #region Variables
        private RealLeadsAPI mApi;
        private Caching.Cache mCache;
        private Data.IRealLeadsDataAdaptor mDataAdaptor;
        #endregion

        #region Properties
        /// <summary>
        /// The RealLeads API
        /// </summary>
        protected RealLeadsAPI API
        {
            get
            {
                return this.mApi;
            }
        }

        /// <summary>
        /// The underlying data adaptor
        /// </summary>
        protected Data.IRealLeadsDataAdaptor Data
        {
            get
            {
                return this.mDataAdaptor;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="api">The RealLeads API object</param>
        /// <param name="dataAdaptor">The data adaptor</param>
        protected Manager(RealLeadsAPI api, Data.IRealLeadsDataAdaptor dataAdaptor)
        {
            if (api == null)
                throw new ArgumentNullException("api");
            if (dataAdaptor == null)
                throw new ArgumentNullException("dataAdaptor");

            this.mApi = api;
            this.mDataAdaptor = dataAdaptor;
        }
        #endregion
    }
}
