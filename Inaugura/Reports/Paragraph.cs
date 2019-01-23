using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Reports
{
    /// <summary>
    /// A class which represents a 
    /// </summary>
    public class Paragraph : ReportSectionContainer
    {
        #region Variables
        private string mText;
        #endregion

        #region Properties
        /// <summary>
        /// The paragraph text
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
        /// <param name="text">The paragraph text</param>
        public Paragraph(string text)
        {
            this.mText = text;
        }
        #endregion
    }
}
