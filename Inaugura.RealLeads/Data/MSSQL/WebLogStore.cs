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
    public class WebLogStore : SqlDataStore, IWebLogStore
    {
        #region Variables
        private IRealLeadsDataAdaptor mAdaptor;
        #endregion

        public WebLogStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString)
            : base(connectionString)
        {
            this.mAdaptor = dataAdaptor;
        }

        #region IWebLogStore Members
        /// <summary>
        /// Gets the web volume for a specific agent
        /// </summary>
        /// <param name="userID">The id of the User</param>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="resolution">The resolution</param>
        /// <returns>The volume</returns>
        public Volume[] GetAgentWebVolume(string userID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
        {
            return this.GetWebVolume(userID, null, startDate, endDate, resolution);
        }

        /// <summary>
        /// Gets the web volume for a specific listing
        /// </summary>
        /// <param name="listingID">The listing ID</param>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="resolution">The resolution</param>
        /// <returns>The volume</returns>
        public Volume[] GetListingWebVolume(string listingID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
        {
            return this.GetWebVolume(null, listingID, startDate, endDate, resolution);
        }

        private Volume[] GetWebVolume(string userID, string listingID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.RealLeadsWebLog_GetVolume";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (userID != null)
                    {
                        cmd.Parameters.Add("@userID", SqlDbType.VarChar, 50);
                        cmd.Parameters["@userID"].Value = userID;
                    }

                    if (listingID != null)
                    {
                        cmd.Parameters.Add("@listingID", SqlDbType.VarChar, 50);
                        cmd.Parameters["@listingID"].Value = listingID;
                    }

                    cmd.Parameters.Add("@startDate", SqlDbType.DateTime);
                    cmd.Parameters["@startDate"].Value = startDate;

                    cmd.Parameters.Add("@endDate", SqlDbType.DateTime);
                    cmd.Parameters["@endDate"].Value = endDate;

                    cmd.Parameters.Add("@resolution", SqlDbType.SmallInt);
                    cmd.Parameters["@resolution"].Value = resolution;

                    System.Collections.Generic.List<Volume> list = new System.Collections.Generic.List<Volume>(128);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime dateTime = Convert.ToDateTime(reader["date"]);
                            int volume = Convert.ToInt32(reader["volume"]);
                            string agent = Convert.ToString(reader["agentID"]);
                            string listing = Convert.ToString(reader["listingID"]);

                            Volume webVolume = new Volume(dateTime, volume, agent, listing);
                            list.Add(webVolume);
                        }
                    }
                    return list.ToArray();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Adds call details to the log
        /// </summary>
        /// <param name="webLog">The details of a web hit</param>
        public void Add(Log webLog)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.RealLeadsWebLog_AddLog";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ID", webLog.ID));
                    cmd.Parameters.Add(new SqlParameter("@userID", webLog.UserID));
                    cmd.Parameters.Add(new SqlParameter("@listingID", webLog.ListingID));

                    DateTime time = webLog.Time;
                    cmd.Parameters.Add("@date", SqlDbType.DateTime);
                    cmd.Parameters["@date"].Value = time;

                    cmd.Parameters.Add("@year", SqlDbType.DateTime);
                    DateTime year = new DateTime(time.Year, 1, 1, 1, 1, 1);
                    cmd.Parameters["@year"].Value = year;

                    cmd.Parameters.Add("@month", SqlDbType.DateTime);
                    DateTime month = new DateTime(time.Year, time.Month, 1, 1, 1, 1);
                    cmd.Parameters["@month"].Value = month;

                    cmd.Parameters.Add("@day", SqlDbType.DateTime);
                    DateTime day = new DateTime(time.Year, time.Month, time.Day, 12, 1, 1);
                    cmd.Parameters["@day"].Value = day;

                    cmd.Parameters.Add("@hour", SqlDbType.DateTime);
                    DateTime hour = new DateTime(time.Year, time.Month, time.Day, time.Hour, 30, 1);
                    cmd.Parameters["@hour"].Value = hour;

                    cmd.Parameters.Add("@minute", SqlDbType.DateTime);
                    DateTime minute = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 30);
                    cmd.Parameters["@minute"].Value = minute;

                    cmd.Parameters.Add("@isOwner", SqlDbType.Bit);
                    cmd.Parameters["@isOwner"].Value = webLog.IsOwner;

                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(webLog.Xml.OuterXml, XmlNodeType.Document, null));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Gets the web log for a specific hit
        /// </summary>
        /// <param name="id">The id of the log</param>
        /// <returns>The call details with the specified ID, otherwise null</returns>
        public Log GetLog(string id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.RealLeadsWebLog_GetLog";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
                    cmd.Parameters["@ID"].Value = id;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string xml = Convert.ToString(reader["xml"]);
                            XmlDocument xmlDoc = new XmlDataDocument();
                            xmlDoc.LoadXml(xml);
                            Log log = new Log(xmlDoc.DocumentElement);
                            return log;
                        }
                    }
                }
                return null;
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Removes all logs which occurd before a specific date
        /// </summary>
        /// <param name="dateTime">The date and time before which to remove all logs</param>
        public void RemoveAll(DateTime dateTime)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.RealLeadsWebLog_RemoveAll";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date", SqlDbType.DateTime);
                    cmd.Parameters["@date"].Value = dateTime;

                    cmd.ExecuteNonQuery();
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
