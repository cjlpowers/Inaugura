#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
#endregion

namespace Inaugura.RealLeads.Data
{
	internal class PhoneStoreHelper
	{
		public static void AddPhone(string phone, string dbConnectionString, string procedure, string keyParamName, string keyValue)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(keyParamName, SqlDbType.VarChar, 50);
					cmd.Parameters.Add("@phone", SqlDbType.VarChar, 15);

					cmd.Parameters[keyParamName].Value = keyValue;
					cmd.Parameters["@phone"].Value = phone;
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public static bool RemovePhone(string phoneNumber, string dbConnectionString, string procedure)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@phone", SqlDbType.VarChar, 15);

					cmd.Parameters["@phone"].Value = phoneNumber;
					return (cmd.ExecuteNonQuery() > 0);
				}
			}
			catch (SqlException sqlException)
			{
				throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
			}
		}

		public static string[] GetPhoneNumbers(string dbConnectionString, string procedure, string keyParamName, string keyValue)
		{
			try
			{
				using (SqlConnection connection = SQLAdaptor.EstablishConnection(dbConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = connection;
					cmd.CommandText = procedure;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(keyParamName, SqlDbType.VarChar, 50);

					cmd.Parameters[keyParamName].Value = keyValue;
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						List<string> list = new List<string>();
						while (reader.Read())
						{
							list.Add(reader.GetString(0));
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
	}
}
