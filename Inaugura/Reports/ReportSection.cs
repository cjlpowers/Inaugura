using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Reports
{
    /// <summary>
    /// The base class for all report sections
    /// </summary>
    public abstract class ReportSection
    {
        #region Internal Constructs
        /// <summary>
        /// A class representing a attribute collection
        /// </summary>
        public class AttributeCollection : List<KeyValuePair<string, string>>
        {
            public void Add(string key, string value)
            {
                this.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        #endregion


        #region Variables
        private AttributeCollection mAttributes;
        private string mID;
        #endregion

        #region Properties
        /// <summary>
        /// The ID of the report section
        /// </summary>
        public string ID
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

        /// <summary>
        /// The attributes
        /// </summary>
        public AttributeCollection Attributes
        {
            get
            {
                return this.mAttributes;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public ReportSection()
        {
            this.mAttributes = new AttributeCollection();
        }
        #endregion
    }
}
