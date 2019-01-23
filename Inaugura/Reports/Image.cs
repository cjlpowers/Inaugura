using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Reports
{
    /// <summary>
    /// A class representing a image
    /// </summary>
    public class Image : ReportSection
    {
        #region Variables
        private Uri mUri;
        #endregion

        #region Properties
        /// <summary>
        /// The Uri of the image
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
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public Image(string uri)
        {
            this.Uri = new Uri(uri);
        }
        #endregion
    }
}
