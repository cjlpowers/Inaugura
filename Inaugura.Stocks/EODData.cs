using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Stocks
{
    /// <summary>
    /// A class representing end of data data
    /// </summary>
    public class EODData
    {
        #region Variables
        private int mStockID;
        private DateTime mDate;
        private float mOpen;
        private float mClose;
        private float mHigh;
        private float mLow;
        private int mVolume;
        #endregion

        #region Properties

        public int StockID
        {
            get
            {
                return this.mStockID;
            }
            internal set
            {
                this.mStockID = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.mDate;
            }            
        }

        public float Open
        {
            get
            {
                return this.mOpen;
            }
        }

        public float Close
        {
            get
            {
                return this.mClose;
            }
        }

        public float High
        {
            get
            {
                return this.mHigh;
            }
        }

        public float Low
        {
            get
            {
                return this.mLow;
            }
        }

        public int Volume
        {
            get
            {
                return mVolume;
            }
        }

        #endregion

        #region Methods
        public EODData(int stockID, DateTime date, float open, float close, float high, float low, int volume)
        {
            mStockID = stockID;
            mDate = date;
            mHigh = high;
            mLow = low;
            mOpen = open;
            mClose = close;
            mVolume = volume;
        }

        public override string ToString()
        {
            return string.Format("EOD Data [ID:{0}, Date:{1}, Open:{2}, Low:{3}, High:{4}, Close:{5}, Volume:{6}]", StockID, Date, Open, Low, High, Close, Volume);
        }
        #endregion
    }
}
