using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Inaugura.Data;

namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Summary description for InvoiceStore.
	/// </summary>
    public class InvoiceStore : SqlDataStore, IInvoiceStore
    {
        #region Variables
        private IRealLeadsDataAdaptor mAdaptor;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataAdaptor">The data adaptor</param>
        /// <param name="connectionString">The database connection string</param>
        public InvoiceStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString) : base(connectionString)
        {
            this.mAdaptor = dataAdaptor;
        }

        internal static Invoice GetInvoiceFromReader(SqlDataReader reader)
        {
            string xml = Convert.ToString(reader["xml"]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return Invoice.FromXml(xmlDoc);
        }
         
        #region IInvoiceStore Members
        /// <summary>
        /// Gets an Invoice
        /// </summary>
        /// <param name="invoiceID">The ID of the Invoice</param>
        /// <returns>The Invoice matching the specified ID, otherwise null</returns>
        public Invoice GetInvoice(string invoiceID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Invoices_GetInvoice";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);

                    cmd.Parameters["@ID"].Value = invoiceID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return InvoiceStore.GetInvoiceFromReader(reader);
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

        /// <summary>
        /// Adds an Invoice 
        /// </summary>
        /// <param name="invoice">The Invoice to add</param>
        public void Add(Invoice invoice)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Invoices_AddInvoice";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);

                    cmd.Parameters["@ID"].Value = invoice.ID;
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(invoice.Xml.OuterXml, XmlNodeType.Document, null));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Updates an Invoice
        /// </summary>
        /// <param name="invoice">The Invoice to update</param>
        /// <returns>True if updated, false otherwise</returns>
        public bool Update(Invoice invoice)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Invoices_UpdateInvoice";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 50);
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);

                    cmd.Parameters["@ID"].Value = invoice.ID;
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(invoice.Xml.OuterXml, XmlNodeType.Document, null));
                    return (cmd.ExecuteNonQuery() > 0);
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        
        /// <summary>
        /// Removes an Invoice
        /// </summary>
        /// <param name="id">The ID of the Invoice to remove</param>
        /// <returns>True if removed, false otherwise</returns>
        public bool Remove(string id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Invoices_RemoveInvoice";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@invoiceID", SqlDbType.VarChar, 50);

                    cmd.Parameters["@invoiceID"].Value = id;

                    return (cmd.ExecuteNonQuery() > 0);
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
