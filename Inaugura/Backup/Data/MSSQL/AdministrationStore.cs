#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Inaugura.RealLeads.Administration;

using Inaugura.Data;
#endregion

namespace Inaugura.RealLeads.Data
{
    public class AdministrationStore : SqlDataStore, IAdministrationStore
	{
		#region Variables
        private IRealLeadsDataAdaptor mAdaptor;
		#endregion

        public AdministrationStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString) : base(connectionString)
		{
			this.mAdaptor = dataAdaptor;
		}

        #region IAdministrationStore Members
        /// <summary>
        /// Adds error information to the store
        /// </summary>
        /// <param name="errorInformation">The error information</param>
        public void AddErrorInformation(ErrorInformation errorInformation)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Errors_Add";
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.Add(new SqlParameter("@xml",new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(errorInformation.Xml.OuterXml, XmlNodeType.Document, null))));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Removes error information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveErrorInformation(string id)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Updates error information
        /// </summary>
        /// <param name="errorInformation">The updated error information</param>
        /// <returns>True if the update was successful</returns>
        public bool UpdateErrorInformation(ErrorInformation errorInformation)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }       
        #endregion
    }
}
