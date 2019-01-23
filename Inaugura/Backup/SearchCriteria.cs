using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class which represents a search criteria
    /// </summary>
    public class SearchCriteria
    {
        #region Internal Constructs
        /// <summary>
        /// How the search is to be performed
        /// </summary>
        public enum SearchMethod
        {
            /// <summary>
            /// String Constains
            /// </summary>
            Contains = 1,
            /// <summary>
            /// String Starts With
            /// </summary>
            StartsWith = 2,
            /// <summary>
            /// String Ends With
            /// </summary>
            EndsWith = 3,
            /// <summary>
            /// String Is (Exact match)
            /// </summary>
            Is = 4
        }
        #endregion

        #region Variables
        private SearchMethod mSearchMethod;
        private string mSearchValue;
        #endregion

        #region Properties
        /// <summary>
        /// The method used to search
        /// </summary>
        public SearchMethod Method
        {
            get
            {
                return this.mSearchMethod;
            }
            set
            {
                this.mSearchMethod = value;
            }
        }

        /// <summary>
        /// The search value
        /// </summary>
        public string SearchValue
        {
            get
            {
                return this.mSearchValue;
            }
            set
            {
                this.mSearchValue = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public SearchCriteria()
            : this(string.Empty, SearchMethod.Contains)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchValue">The search value</param>
        public SearchCriteria(string searchValue)
            : this(searchValue, SearchMethod.Is)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchValue">The search value</param>
        /// <param name="searchMethod">The search method</param>
        public SearchCriteria(string searchValue, SearchMethod searchMethod)
        {
            this.mSearchMethod = searchMethod;
            this.mSearchValue = searchValue;
        }

        /// <summary>
        /// Determins if there is a match with the current search criteria.
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="comparisonType">The comparison type</param>
        /// <returns>True if there is a match, false otherwise</returns>
        public bool IsMatch(string value, StringComparison comparisonType)
        {
            if (value == null)
                return false;

            if (this.mSearchMethod == SearchMethod.Is)
                return (string.Compare(this.mSearchValue, value, comparisonType) == 0);
            else if (this.mSearchMethod == SearchMethod.EndsWith)
                return value.EndsWith(mSearchValue, comparisonType);
            else if (this.mSearchMethod == SearchMethod.StartsWith)
                return value.StartsWith(mSearchValue, comparisonType);
            else if (this.mSearchMethod == SearchMethod.Contains)
                return (value.IndexOf(mSearchValue, comparisonType) != -1);
            else
                throw new NotImplementedException("The search method was not implemented");
        }

        /// <summary>
        /// Determins if there is a match with the current search criteria (OrdinalIgnoreCase).
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True if there is a match, false otherwise</returns>
        public bool IsMatch(string value)
        {
            return this.IsMatch(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.GetSqlString();
        }

        /// <summary>
        /// Gets the Sql representation of the search criteria
        /// </summary>
        /// <returns>The sql string</returns>
        internal string GetSqlString()
        {
            // TODO get a safe sql string

            if (this.mSearchMethod == SearchMethod.StartsWith)
                return this.mSearchValue + "%";
            else if (this.mSearchMethod == SearchMethod.EndsWith)
                return "%" + this.mSearchValue;
            else if (this.mSearchMethod == SearchMethod.Contains)
                return "%" + this.mSearchValue + "%";
            else if (this.mSearchMethod == SearchMethod.Is)
                return this.mSearchValue;
            else
                throw new NotImplementedException("The search method was not implemented");
        }

        /// <summary>
        /// Parses a value to the actual search criteria
        /// </summary>
        /// <param name="value">The search value</param>
        /// <returns>The search criteria</returns>
        public static SearchCriteria Parse(string value)
        {
            if (value.StartsWith("%"))
            {
                if (value.EndsWith("%"))
                    return new SearchCriteria(value.Trim('%'), SearchMethod.Contains);
                else
                    return new SearchCriteria(value.Trim('%'), SearchMethod.StartsWith);
            }
            else if (value.EndsWith("%"))
            {
                return new SearchCriteria(value.Trim('%'), SearchMethod.EndsWith);
            }
            else
                return new SearchCriteria(value, SearchMethod.Is);
        }
        #endregion
    }
}
