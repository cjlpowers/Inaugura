#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
#endregion

namespace Inaugura.Data
{
    internal class FileStoreHelper
    {
        public static void AddFile(Inaugura.File file, string dbConnectionString, string procedure, string keyParamName, string keyValue)
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
                    cmd.Parameters.Add(keyParamName, SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@fileName", SqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@size", SqlDbType.Int);
                    cmd.Parameters.Add("@data", SqlDbType.Image);

                    cmd.Parameters["@ID"].Value = file.ID;
                    cmd.Parameters[keyParamName].Value = keyValue;
                    cmd.Parameters["@fileName"].Value = file.FileName;
                    cmd.Parameters["@size"].Value = file.Data.Length;
                    cmd.Parameters["@data"].Value = file.Data;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public static Inaugura.File GetFile(string dbConnectionString, string procedure, Guid fileId)
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
                    cmd.Parameters["@ID"].Value = fileId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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
                                return new File(fileId, (string)reader["fileName"], memStream.ToArray());
                            }
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


        public static bool RemoveFile(string dbConnectionString, string procedure, string fileId)
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

                    cmd.Parameters["@ID"].Value = fileId;
                    return (cmd.ExecuteNonQuery() > 0);
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public static bool UpdateFile(Inaugura.File file, string dbConnectionString, string procedure)
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
                    cmd.Parameters.Add("@data", SqlDbType.Image);

                    cmd.Parameters["@ID"].Value = file.ID;
                    cmd.Parameters["@fileName"].Value = file.FileName;
                    cmd.Parameters["@size"].Value = file.Data.Length;
                    cmd.Parameters["@data"].Value = file.Data;
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
