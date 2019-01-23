#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
#endregion

namespace Inaugura.RealLeads.Data
{
	internal class FileStoreHelper
	{
		public static void AddFile(File file, string dbConnectionString, string procedure, string keyParamName, Guid keyValue)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", file.ID));
                    cmd.Parameters.Add(new SqlParameter(keyParamName,keyValue));
					cmd.Parameters.Add("@fileName", SqlDbType.VarChar, 255);
					cmd.Parameters.Add("@size", SqlDbType.Int);
					//cmd.Parameters.Add("@data", SqlDbType.Image);

					cmd.Parameters["@fileName"].Value = file.FileName;
					cmd.Parameters["@size"].Value = file.Data.Length;
					//cmd.Parameters["@data"].Value = file.Data;
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public static File GetFile(string dbConnectionString, string procedure, Guid fileId)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", fileId));

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
                            if (!reader.IsDBNull(0))
                            {
                                int index = 0;
                                int bytesRead = 0;
                                byte[] buffer = new byte[1024 * 4];
                                using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
                                {
                                    do
                                    {
                                        bytesRead = (int)reader.GetBytes(0, index, buffer, 0, buffer.Length);
                                        index += bytesRead;
                                        memStream.Write(buffer, 0, bytesRead);
                                    } while (bytesRead > 0);
                                    return new File(fileId, Convert.ToString(reader["fileName"]), memStream.ToArray());                            
                                }
                            }
                            else
                                return new File(fileId, Convert.ToString(reader["fileName"]), new byte[0]); 
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


		public static bool RemoveFile(string dbConnectionString, string procedure, Guid fileId)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);

					cmd.Parameters["@ID"].Value = fileId;
					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public static bool UpdateFile(File file, string dbConnectionString, string procedure)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@fileName", SqlDbType.VarChar, 255);
					cmd.Parameters.Add("@size", SqlDbType.Int);
					//cmd.Parameters.Add("@data", SqlDbType.Image);

					cmd.Parameters["@ID"].Value = file.ID;
					cmd.Parameters["@fileName"].Value = file.FileName;
					cmd.Parameters["@size"].Value = file.Data.Length;
					//cmd.Parameters["@data"].Value = file.Data;
					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}
	}
}
