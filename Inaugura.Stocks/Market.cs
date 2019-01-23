using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Stocks
{
    public class Market
    {
        #region Variables
        private int mID;
        private string mName;
        private string mCode;
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public string Code
        {
            get
            {
                return this.mCode;
            }
            set
            {
                this.mCode = value;
            }
        }
        #endregion

        #region Methods
        internal Market(System.Data.DataRow row)
        {
            this.mID = Convert.ToInt32(row["ID"]);
            this.Name = Convert.ToString(row["Name"]);
            this.Code = Convert.ToString(row["Code"]);
        }

        public Market(string name, string code)
        {
            this.mName = name;
            this.mCode = code;
        }

        #region Logic
        public void AddStock(Data.IStockStore store, Stock stock)
        {
            stock.MarketID = this.ID;
            store.AddStock(stock);
        }

        public DateTime GetLastEODDate(Data.IStockStore store)
        {
            return store.GetLastEODDate(this.mID);
        }


        public Stock[] GetStocks(Data.IStockStore store)
        {
            return store.GetStocks(this.mID);
        }
        #endregion
        #endregion
    }
}
