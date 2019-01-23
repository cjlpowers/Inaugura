using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Reports
{
    /// <summary>
    /// A class representing a report
    /// </summary>
    public class Report : ReportSectionContainer
    {
        #region Variables
        private string mID;
        #endregion

        #region Properties
        /// <summary>
        /// The report ID
        /// </summary>
        public string ID
        {
            get
            {
                return this.mID;
            }
            private set
            {
                this.mID = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public Report()
        {
            this.mID = Guid.NewGuid().ToString();
        }
        #endregion
    }
}
