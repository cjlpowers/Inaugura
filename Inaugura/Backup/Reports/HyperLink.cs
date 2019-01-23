using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Reports
{
    /// <summary>
    /// A class representing a hyper link
    /// </summary>
    public class HyperLink : ReportSectionContainer
    {
        #region Variables
        private Uri mUri;
        private string mText;
        #endregion

        #region Properties
        /// <summary>
        /// The Uri of the hyper link
        /// </summary>
        public Uri Uri
        {
            get
            {
                return this.mUri;
            }
            set
            {
                this.mUri = value;
            }
        }

        /// <summary>
        /// The text
        /// </summary>
        public string Text
        {
            get
            {
                return this.mText;
            }
            set
            {
                this.mText = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri"></param>
        public HyperLink(string uri)
        {
            this.Uri = new Uri(uri);
        }
        #endregion
    }
}
