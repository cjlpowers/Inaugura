using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Inaugura.Stocks.Data.MSSQL
{
    public class StockStore : Inaugura.Data.SqlDataStore, IStockStore
    {
        #region Methods
        public StockStore(string connectionString)
            : base(connectionString)
        {
        }
        #endregion
        
        #region IStockStore Members

        #region Market Functionality
        public Market[] GetMarkets()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Getting Markets"), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Markets_GetMarkets";
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the commnad
                DataTable results = GetResult(cmd);
                List<Market> markets = new List<Market>();
                foreach (DataRow row in results.Rows)
                    markets.Add(new Market(row));
                return markets.ToArray();
            }    
        }

        public void AddMarket(Market market)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Adding Market {0}", market), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Markets_AddMarket";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Code", market.Code));
                cmd.Parameters.Add(new SqlParameter("@Name", market.Name));

                SqlParameter returnParam = new SqlParameter("@return", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParam);

                // execute the command
                if (cmd.ExecuteNonQuery() == 0)
                    throw new DataException("The market could not be added to the database");

                market.ID = Convert.ToInt32(cmd.Parameters["@return"].Value);
            }
        }
        #endregion


        public Stock[] GetStocks(int marketID)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Getting Stocks for Market {0}",marketID), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Stocks_GetStocks";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MarketID", marketID);

                // execute the commnad
                DataTable results = GetResult(cmd);
                List<Stock> stocks = new List<Stock>();
                foreach (DataRow row in results.Rows)
                    stocks.Add(new Stock(row));
                return stocks.ToArray();
            }            
        }

        public void AddStock(Stock stock)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Adding Stock {0}",stock), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Stocks_AddStock";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MarketID", stock.MarketID));
                cmd.Parameters.Add(new SqlParameter("@Name", stock.Name));
                cmd.Parameters.Add(new SqlParameter("@Symbol", stock.Symbol));

                SqlParameter returnParam = new SqlParameter("@return", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParam);

                // execute the command
                if (cmd.ExecuteNonQuery() == 0)
                    throw new DataException("The stock could not be added to the database");

                stock.ID = Convert.ToInt32(cmd.Parameters["@return"].Value);
            }
        }

        public void UpdateStock(Stock stock)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Updating Stock {0}", stock), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Stocks_UpdateStock";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StockID", stock.ID));
                cmd.Parameters.Add(new SqlParameter("@MarketID", stock.MarketID));
                cmd.Parameters.Add(new SqlParameter("@Name", stock.Name));
                cmd.Parameters.Add(new SqlParameter("@Symbol", stock.Symbol));

                // execute the command
                if (cmd.ExecuteNonQuery() == 0)
                    throw new DataException("The stock could not be updated in the database");
            }
        }


        public void AddEODData(EODData data)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Adding Stock EOD Data {0}", data), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "EODData_AddData";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StockID", data.StockID));
                cmd.Parameters.Add(new SqlParameter("@Date", data.Date));
                cmd.Parameters.Add(new SqlParameter("@Open", data.Open));
                cmd.Parameters.Add(new SqlParameter("@High", data.High));
                cmd.Parameters.Add(new SqlParameter("@Low", data.Low));
                cmd.Parameters.Add(new SqlParameter("@Close", data.Close));
                cmd.Parameters.Add(new SqlParameter("@Volume", data.Volume));

                // execute the command
                if (cmd.ExecuteNonQuery() == 0)
                    throw new DataException("The form could not be added to the database");
            }
        }

        public DateTime GetLastEODDate(int marketID)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Getting Last EOD Data Date for market ID {0}", marketID), this.GetType().ToString());

            using (SqlConnection connection = EstablishConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "EODData_GetLastDate";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MarketID", marketID));


                DataTable results = GetResult(cmd);

                if (results.Rows.Count == 0)
                    return DateTime.MinValue;
                else
                    return Convert.ToDateTime(results.Rows[0]["Date"]);
            }
        }


        #endregion
    }
}
