using System;
using System.Text;

namespace Inaugura.Stocks.Data
{
    /// <summary>
    /// An interface which defines the methods supported by the Meter Store
    /// </summary>
    public interface IStockStore
    {

        #region Market Functionality
        Market[] GetMarkets();
        void AddMarket(Market market);
        #endregion

        #region Stock Functionality
        Stock[] GetStocks(int marketID);

        void AddStock(Stock stock);

        void UpdateStock(Stock stock);
        #endregion

        #region End of Data Data Functionality
        void AddEODData(EODData data);

        DateTime GetLastEODDate(int marketID);
        #endregion
    }
}
