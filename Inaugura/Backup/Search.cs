using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
	/// <summary>
	/// A class which holds the search criteria
	/// </summary>
	public abstract class Search : Inaugura.Xml.IXmlable
	{
		#region Variables
        private int mStartIndex;
        private int mEndIndex;
        private int mResultCount;
        
        private bool mCalculateResultCount;
		#endregion

		#region Properties
        /// <summary>
        /// The starting index of the result set
        /// </summary>
        public int StartIndex
        {
            get
            {
                return this.mStartIndex;
            }
            set
            {
                this.mStartIndex = value;
            }
        }

        /// <summary>
        /// The end index of the result set
        /// </summary>
        public int EndIndex
        {
            get
            {
                return this.mEndIndex;
            }
            set
            {
                this.mEndIndex = value;
            }
        }       

        /// <summary>
        /// The number of results
        /// </summary>
        public int ResultCount
        {
            get
            {
                return this.mResultCount;
            }
            internal set
            {
                this.mResultCount = value;
            }
        }

        /// <summary>
        /// A flag which determins if the result count should be calculated
        /// </summary>
        public bool CalculateResultCount
        {
            get
            {
                return this.mCalculateResultCount;
            }
            set
            {
                this.mCalculateResultCount = value;
            }
        }
		#endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of this instance
        /// </summary>
        public System.Xml.XmlNode Xml
        {
            get 
            {
                XmlNode searchNode = Inaugura.Xml.Helper.NewNodeDocument("Search");
                this.PopulateNode(searchNode);
                return searchNode;
            }
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public virtual void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetTypeAttribute(node, this.GetType());
        }

        /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node">The xml node</param>
        public virtual void PopulateInstance(XmlNode node)
        {
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Constructor
        /// </summary>
        protected Search()
        {
            this.mStartIndex = 1;
            this.mEndIndex = 20;
            this.CalculateResultCount = true;
        }

        /// <summary>
        /// Gets the number of pages needed to display the result set
        /// </summary>
        /// <param name="pageSize">The number of results per page</param>
        /// <param name="resultCount">The number of results</param>
        /// <returns>The number of pages reqired</returns>
        static public int CalculatePageCount(int pageSize, int resultCount)
        {
            double val = ((double)resultCount) / ((double)pageSize);
            return (int)Math.Ceiling(val);
        }

        /// <summary>
        /// Determins the page index for the current index
        /// </summary>
        /// <param name="pageSize">The page size</param>
        /// <param name="index">The result index</param>
        /// <returns>The page index</returns>
        static public int CalculatePageIndex(int pageSize, int index)
        {
            double val = ((double)index) / ((double)pageSize);
            return (int)Math.Floor(val);
        }

        /// <summary>
        /// Returns the hashcode for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {          
            return this.Xml.OuterXml.GetHashCode();
        }
        #endregion
    }
}
