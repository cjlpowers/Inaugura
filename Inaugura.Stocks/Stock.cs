using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Stocks
{
    public class Stock
    {
        #region Variables
        private int mID;
        private int mMarketID;
        private string mName;
        private string mSymbol;
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return this.mID;
            }
            internal set
            {
                this.mID = value;
            }
        }

        public int MarketID
        {
            get
            {
                return this.mMarketID;
            }
            internal set
            {
                this.mMarketID = value;
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

        public string Symbol
        {
            get
            {
                return this.mSymbol;
            }
            set
            {
                this.mSymbol = value;
            }
        }        
        #endregion

        #region Methods
        internal Stock(System.Data.DataRow row)
        {
            this.mID = Convert.ToInt32(row["ID"]);
            this.Name = Convert.ToString(row["Name"]);
            this.Symbol = Convert.ToString(row["Symbol"]);
            this.MarketID = Convert.ToInt32(row["MarketID"]);
        }

        public Stock(int marketID, string name, string symbol)
        {            
            this.Name = name;
            this.Symbol = symbol;
            this.mMarketID = marketID;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Symbol);
        }

        #region Logic
        public void AddEODData(Data.IStockStore store, EODData data)
        {
            data.StockID = ID;
            store.AddEODData(data);
        }

        public void Update(Data.IStockStore store)
        {
            store.UpdateStock(this);
        }
        #endregion

        #endregion

    }
}
