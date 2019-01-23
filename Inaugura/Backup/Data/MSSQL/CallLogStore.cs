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
	public class CallLogStore :SqlDataStore, ICallLogStore
	{
		#region Variables
        private IRealLeadsDataAdaptor mAdaptor;
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataAdaptor">The realleads data adaptor</param>
        /// <param name="connectionString">The connection string</param>
        public CallLogStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString) : base(connectionString)
		{
			this.mAdaptor = dataAdaptor;
		}
			
		#region ICallLogStore Members
		public CallVolume[] GetAdminCallVolume(string agentID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
		{
			return this.GetCallVolume( null, agentID, null, startDate, endDate, resolution, true);
		}

        public CallVolume[] GetSwitchCallVolume(string switchID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
		{
			return this.GetCallVolume(switchID, null, null, startDate, endDate, resolution, false);
		}

        public CallVolume[] GetAgentCallVolume(string agentID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
		{
			return this.GetCallVolume(null, agentID, null, startDate, endDate, resolution, false);
		}

        public CallVolume[] GetListingCallVolume(string listingID, DateTime startDate, DateTime endDate, Volume.Resolution resolution)
		{
			return GetCallVolume(null, null, listingID, startDate, endDate, resolution, false);
		}

        private CallVolume[] GetCallVolume(string switchID, string agentID, string listingID, DateTime startDate, DateTime endDate, Volume.Resolution resolution, bool onlyAgentCalls)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.RealLeadsCallLog_GetCallVolume";
					cmd.CommandType = CommandType.StoredProcedure;

					if (switchID != null)
					{
						cmd.Parameters.Add("@switchID", SqlDbType.VarChar, 50);
						cmd.Parameters["@switchID"].Value = switchID;
					}

					if (agentID != null)
					{
						cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);
						cmd.Parameters["@agentID"].Value = agentID;
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

					cmd.Parameters.Add("@onlyAgentCalls", SqlDbType.Bit);
					cmd.Parameters["@onlyAgentCalls"].Value = onlyAgentCalls;

					cmd.Parameters.Add("@resolution", SqlDbType.SmallInt);
					cmd.Parameters["@resolution"].Value = resolution;

					System.Collections.Generic.List<CallVolume> list = new System.Collections.Generic.List<CallVolume>(128);					
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime dateTime = Convert.ToDateTime(reader["date"]);
							int volume = Convert.ToInt32(reader["volume"]);							
							string sw = Convert.ToString(reader["switchID"]);
							string agent = Convert.ToString(reader["agentID"]);
							string listing = Convert.ToString(reader["listingID"]);

							CallVolume callVolume = new CallVolume(dateTime, volume, sw, agent, listing);
							list.Add(callVolume);
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

		public void Add(CallLog callDetails)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.RealLeadsCallLog_AddCall";
					cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
                    cmd.Parameters["@ID"].Value = callDetails.ID;

					cmd.Parameters.Add("@switchID", SqlDbType.VarChar, 50);
					cmd.Parameters["@switchID"].Value = callDetails.SwitchID;

					if (callDetails.UserID != null)
					{
						cmd.Parameters.Add("@userID", SqlDbType.VarChar, 50);
                        cmd.Parameters["@userID"].Value = callDetails.UserID;
					}

					if (callDetails.ListingID != null)
					{
						cmd.Parameters.Add("@listingID", SqlDbType.VarChar, 50);
						cmd.Parameters["@listingID"].Value = callDetails.ListingID;
					}

					DateTime callTime = callDetails.Time;
					cmd.Parameters.Add("@date", SqlDbType.DateTime);
					cmd.Parameters["@date"].Value = callTime;

					cmd.Parameters.Add("@year", SqlDbType.DateTime);
					DateTime year = new DateTime(callTime.Year, 1, 1, 1, 1, 1);
					cmd.Parameters["@year"].Value = year;

					cmd.Parameters.Add("@month", SqlDbType.DateTime);
					DateTime month = new DateTime(callTime.Year, callTime.Month, 1, 1, 1, 1);
					cmd.Parameters["@month"].Value = month;

					cmd.Parameters.Add("@day", SqlDbType.DateTime);
					DateTime day = new DateTime(callTime.Year, callTime.Month, callTime.Day, 12, 1, 1);
					cmd.Parameters["@day"].Value = day;

					cmd.Parameters.Add("@hour", SqlDbType.DateTime);
					DateTime hour = new DateTime(callTime.Year, callTime.Month, callTime.Day, callTime.Hour, 30, 1);
					cmd.Parameters["@hour"].Value = hour;

					cmd.Parameters.Add("@minute", SqlDbType.DateTime);
					DateTime minute = new DateTime(callTime.Year, callTime.Month, callTime.Day, callTime.Hour, callTime.Minute, 30);
					cmd.Parameters["@minute"].Value = minute;

                    cmd.Parameters.Add("@isOwner", SqlDbType.Bit);
					cmd.Parameters["@isOwner"].Value = callDetails.IsOwner;

					cmd.Parameters.Add("@xml", SqlDbType.Xml);
					cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(callDetails.Xml.OuterXml, XmlNodeType.Document, null));
					
					cmd.ExecuteNonQuery();										
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

        /// <summary>
        /// Gets the call details for a specific call
        /// </summary>
        /// <param name="id">The id of the call details</param>
        /// <returns>The call details with the specified ID, otherwise null</returns>
        public CallLog GetCall(string id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.RealLeadsCallLog_GetCall";
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
                            CallLog call = new CallLog(xmlDoc.DocumentElement);
                            return call;
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

		public void RemoveAll(DateTime dateTime)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.RealLeadsCallLog_RemoveAll";
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

		public CallLog[] GetRecentCalls( string switchID, string agentID, string listingID, bool includeAgentCalls, int maxItems)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.RealLeadsCallLog_GetCalls";
					cmd.CommandType = CommandType.StoredProcedure;
                    				

					if (switchID != null)
					{
						cmd.Parameters.Add("@switchID", SqlDbType.VarChar, 50);
						cmd.Parameters["@switchID"].Value = switchID;
					}

					if (agentID != null)
					{
						cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);
						cmd.Parameters["@agentID"].Value = agentID;
					}

					if (listingID != null)
					{
						cmd.Parameters.Add("@listingID", SqlDbType.VarChar, 50);
						cmd.Parameters["@listingID"].Value = listingID;
					}

					cmd.Parameters.Add("@includeAgentCalls", SqlDbType.Bit);
					cmd.Parameters["@includeAgentCalls"].Value = includeAgentCalls;

					cmd.Parameters.Add("@maxItems", SqlDbType.SmallInt);
					cmd.Parameters["@maxItems"].Value = maxItems;

					System.Collections.Generic.List<CallLog> list = new System.Collections.Generic.List<CallLog>(maxItems);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							string xml = Convert.ToString(reader["xml"]);
							XmlDocument xmlDoc = new XmlDataDocument();
							xmlDoc.LoadXml(xml);
							CallLog call = new CallLog(xmlDoc.DocumentElement);

							list.Add(call);
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
		/// Retreives call details
		/// </summary>		
		/// <param name="switchID">The switch ID</param>
		/// <param name="agentID">The agent ID</param>
		/// <param name="listingID">The listing ID</param>
		/// <param name="includeAgentCalls">Determines if calls from the agent should be included</param>
		/// <param name="startDate">The start date</param>
		/// <param name="endDate">The end date</param>
		/// <returns>The list of call details found</returns>
		public CallLog[] GetCalls( string switchID, string agentID, string listingID, bool includeAgentCalls, DateTime startDate, DateTime endDate)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.RealLeadsCallLog_GetCalls";
					cmd.CommandType = CommandType.StoredProcedure;
                    
					if (switchID != null)
					{
						cmd.Parameters.Add("@switchID", SqlDbType.VarChar, 50);
						cmd.Parameters["@switchID"].Value = switchID;
					}

					if (agentID != null)
					{
						cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);
						cmd.Parameters["@agentID"].Value = agentID;
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

					cmd.Parameters.Add("@includeAgentCalls", SqlDbType.Bit);
					cmd.Parameters["@includeAgentCalls"].Value = includeAgentCalls;
					
					System.Collections.Generic.List<CallLog> list = new System.Collections.Generic.List<CallLog>();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							string xml = Convert.ToString(reader["xml"]);
							XmlDocument xmlDoc = new XmlDataDocument();
							xmlDoc.LoadXml(xml);
							CallLog call = new CallLog(xmlDoc.DocumentElement);

							list.Add(call);
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
		#endregion
	}
}
