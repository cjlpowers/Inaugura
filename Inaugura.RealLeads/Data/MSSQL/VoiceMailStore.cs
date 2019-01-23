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
	public class VoiceMailStore : SqlDataStore, IVoiceMailStore
	{
		#region Variables
        private IRealLeadsDataAdaptor mAdaptor;
		#endregion

        public VoiceMailStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString) : base(connectionString)
		{
			this.mAdaptor = dataAdaptor;
		}

		internal static VoiceMail GetVoiceMailFromReader(SqlDataReader reader, string field)
		{
			string xml = Convert.ToString(reader[field]);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			return VoiceMail.FromXml(xmlDoc);
		}

		#region IVoiceMailStore Members

		public VoiceMail GetVoiceMail(string id)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.VoiceMail_GetVoiceMail";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);

					cmd.Parameters["@ID"].Value = id;
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							return VoiceMailStore.GetVoiceMailFromReader(reader, "xml");
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

		public VoiceMail[] GetVoiceMails(string agentID)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.VoiceMail_GetVoiceMails";
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);

					cmd.Parameters["@agentID"].Value = agentID;
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						List<VoiceMail> list = new List<VoiceMail>();
						while (reader.Read())
						{
							list.Add(VoiceMailStore.GetVoiceMailFromReader(reader, "xml"));
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

		public VoiceMail[] GetVoiceMails(string agentID, VoiceMail.VoiceMailStatus status)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.VoiceMail_GetVoiceMailByStatus";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@status", SqlDbType.VarChar, 100);


					cmd.Parameters["@agentID"].Value = agentID;
					cmd.Parameters["@status"].Value = status.ToString();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						List<VoiceMail> list = new List<VoiceMail>();
						while (reader.Read())
						{
							list.Add(VoiceMailStore.GetVoiceMailFromReader(reader, "xml"));
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

		public void Add(VoiceMail voiceMail)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.VoiceMail_AddVoiceMail";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@date", SqlDbType.DateTime);
					cmd.Parameters.Add("@status", SqlDbType.VarChar, 100);
					cmd.Parameters.Add("@xml", SqlDbType.Xml);


					cmd.Parameters["@ID"].Value = voiceMail.ID;
					cmd.Parameters["@agentID"].Value = voiceMail.AgentID;
					cmd.Parameters["@date"].Value = voiceMail.Date;
					cmd.Parameters["@status"].Value = voiceMail.Status.ToString();
					cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(voiceMail.Xml.OuterXml, XmlNodeType.Document, null));
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public bool Remove(string id)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.VoiceMail_RemoveVoiceMail";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);

					cmd.Parameters["@ID"].Value = id;
					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public bool Update(VoiceMail voiceMail)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = "dbo.VoiceMail_UpdateVoiceMail";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@agentID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@date", SqlDbType.DateTime);
					cmd.Parameters.Add("@status", SqlDbType.VarChar, 100);
					cmd.Parameters.Add("@xml", SqlDbType.Xml);


					cmd.Parameters["@ID"].Value = voiceMail.ID;
					cmd.Parameters["@agentID"].Value = voiceMail.AgentID;
					cmd.Parameters["@date"].Value = voiceMail.Date;
					cmd.Parameters["@status"].Value = voiceMail.Status.ToString();
					cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(voiceMail.Xml.OuterXml, XmlNodeType.Document, null));
					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		#region Voice Mail Files
		public void AddFile(Guid voiceMailID, File file)
		{
			FileStoreHelper.AddFile(file, this.ConnectionString, "dbo.VoiceMailFiles_AddFile", "@voiceMailID", voiceMailID);
		}

		public File GetFile(Guid fileID)
		{
			return FileStoreHelper.GetFile(this.ConnectionString, "dbo.VoiceMailFiles_GetFile", fileID);
		}

		public bool RemoveFile(Guid fileId)
		{
			return FileStoreHelper.RemoveFile(this.ConnectionString, "dbo.VoiceMailFiles_RemoveFile", fileId);
		}
		/// <summary>
		/// Updates a File
		/// </summary>
		/// <param name="file">The updated File</param>
		/// <returns></returns>
		public bool UpdateFile(File file)
		{
			return FileStoreHelper.UpdateFile(file, this.ConnectionString, "dbo.VoiceMailFiles_UpdateFile");
		}
		#endregion
		#endregion
	}
}
