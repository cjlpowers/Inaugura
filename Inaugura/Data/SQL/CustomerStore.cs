using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Inaugura.Data
{
    /// <summary>
    /// Summary description for CompanyDBStore.
    /// </summary>
    public class CustomerStore : SqlDataStore, ICustomerStore
    {
        #region Variables
        private Inaugura.Data.IDataAdaptor mAdaptor;
        #endregion

        #region Properties
        #endregion

        public CustomerStore(Inaugura.Data.IDataAdaptor adaptor, string connectionString) : base(connectionString)
        {
            this.mAdaptor = adaptor;
        }

        internal static Customer GetCustomerFromReader(SqlDataReader reader)
        {
            string xml = Convert.ToString(reader["xml"]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return Customer.FromXml(xmlDoc);
        }

        #region ICustomerStore Members

        public virtual Customer GetCustomer(string customerID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Customers_GetCustomer";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);

                    cmd.Parameters["@ID"].Value = customerID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return CustomerStore.GetCustomerFromReader(reader);
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

        public virtual Customer GetCustomerByEmail(string email)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Customers_GetCustomerByEmail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100);

                    cmd.Parameters["@email"].Value = email;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return CustomerStore.GetCustomerFromReader(reader);
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

        public virtual void Add(Customer customer)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Customers_AddCustomer";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@companyID", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@customerNumber", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100);
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);


                    cmd.Parameters["@ID"].Value = customer.ID;
                    cmd.Parameters["@companyID"].Value = customer.CompanyID;
                    cmd.Parameters["@customerNumber"].Value = customer.CustomerNumber;
                    cmd.Parameters["@email"].Value = customer.Email;
                    cmd.Parameters["@password"].Value = customer.Password;
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(customer.Xml.OuterXml, XmlNodeType.Document, null));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public virtual bool Remove(string customerID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Customers_Remove";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@customerID", SqlDbType.VarChar, 50);

                    cmd.Parameters["@customerID"].Value = customerID;

                    return (cmd.ExecuteNonQuery() > 0);
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public virtual bool Update(Customer customer)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Customers_UpdateCustomer";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@companyID", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@customerNumber", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100);
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);


                    cmd.Parameters["@ID"].Value = customer.ID;
                    cmd.Parameters["@companyID"].Value = customer.CompanyID;
                    cmd.Parameters["@customerNumber"].Value = customer.CustomerNumber;
                    cmd.Parameters["@email"].Value = customer.Email;
                    cmd.Parameters["@password"].Value = customer.Password;
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(customer.Xml.OuterXml, XmlNodeType.Document, null));
                    return (cmd.ExecuteNonQuery() > 0);
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public Customer[] Search()
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    List<Customer> list = new List<Customer>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Customers_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(CustomerStore.GetCustomerFromReader(reader));
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
