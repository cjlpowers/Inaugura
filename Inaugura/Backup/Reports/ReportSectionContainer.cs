using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Reports
{
    /// <summary>
    /// A report section which contains sub report sections
    /// </summary>
    public abstract class ReportSectionContainer : ReportSection
    {
        #region Variables
        private List<ReportSection> mSections;
        #endregion 

        #region Properties
        /// <summary>
        /// The child report sections
        /// </summary>
        public List<ReportSection> Sections
        {
            get
            {
                return this.mSections;
            }
            set
            {
                this.mSections = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public ReportSectionContainer()
        {
            this.mSections = new List<ReportSection>();
        }
        #endregion
    }
}
