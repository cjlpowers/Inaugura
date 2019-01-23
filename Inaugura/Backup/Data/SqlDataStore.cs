using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Data
{
    /// <summary>
    /// A base class for SQL Data stores
    /// </summary>
    public abstract class SqlDataStore
    {
        #region Variables
        private string mConnectionString;
        #endregion

        #region Properties
        /// <summary>
        /// The database connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.mConnectionString;
            }
            set
            {
                this.mConnectionString = value;
            }
        }
        #endregion

        protected SqlDataStore(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
