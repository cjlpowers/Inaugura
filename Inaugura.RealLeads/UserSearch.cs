using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// A class representing a user search
    /// </summary>
    public class UserSearch : Search
    {
        #region Internal Constructs
        /// <summary>
        /// A email verificatin status enumeration
        /// </summary>
        public enum EmailVerificationStatus
        {
            Any = -1,
            Unverified = 0,
            Verified = 1,                        
        }
        #endregion

        #region Variables
        private SearchCriteria mEmail;
        private SearchCriteria mFirstName;
        private SearchCriteria mLastName;
        private SearchCriteria mPhoneNumber;
        private DateTime mCreationDateStart;
        private DateTime mCreationDateEnd;
        private DateTime mLastLoginStart;
        private DateTime mLastLoginEnd;
        private EmailVerificationStatus mEmailVerificationStatus;      
        #endregion

        #region Properties
        /// <summary>
        /// The first name criteria
        /// </summary>
        public SearchCriteria FirstName
        {
            get
            {
                return this.mFirstName;
            }
            set
            {
                this.mFirstName = value;
            }
        }

        /// <summary>
        /// The last name criteria
        /// </summary>
        public SearchCriteria LastName
        {
            get
            {
                return this.mLastName;
            }
            set
            {
                this.mLastName = value;
            }
        }

        /// <summary>
        /// The email criteria
        /// </summary>
        public SearchCriteria Email
        {
            get
            {
                return this.mEmail;
            }
            set
            {
                this.mEmail = value;
            }
        }     

        /// <summary>
        /// The phone number criteria
        /// </summary>
        public SearchCriteria PhoneNumber
        {
            get
            {
                return this.mPhoneNumber;
            }
            set
            {
                this.mPhoneNumber = value;
            }
        }

        /// <summary>
        /// The minimum creation date
        /// </summary>
        public DateTime CreationDateStart
        {
            get
            {
                return this.mCreationDateStart;
            }
            set
            {
                this.mCreationDateStart = value;
            }
        }

        /// <summary>
        /// The maximum creation date
        /// </summary>
        public DateTime CreationDateEnd
        {
            get
            {
                return this.mCreationDateEnd;
            }
            set
            {
                this.mCreationDateEnd = value;
            }
        }

        /// <summary>
        /// The minimum last login date
        /// </summary>
        public DateTime LastLoginStart
        {
            get
            {
                return this.mLastLoginStart;
            }
            set
            {
                this.mLastLoginStart = value;
            }
        }

        /// <summary>
        /// The maximum last login date
        /// </summary>
        public DateTime LastLoginEnd
        {
            get
            {
                return this.mLastLoginEnd;
            }
            set
            {
                this.mLastLoginEnd = value;
            }
        }

        /// <summary>
        /// The email verification status
        /// </summary>
        public EmailVerificationStatus EmailVerification
        {
            get
            {
                return this.mEmailVerificationStatus;
            }
            set
            {
                this.mEmailVerificationStatus = value;
            }
        }
        #endregion

        #region Methods
        public UserSearch()
        {
            this.mCreationDateEnd = DateTime.MaxValue;
            this.mCreationDateStart = DateTime.MinValue;
            this.mLastLoginEnd = DateTime.MaxValue;
            this.mLastLoginStart = DateTime.MinValue;
            this.mEmailVerificationStatus = EmailVerificationStatus.Any;
        }
        #endregion
    }
}
