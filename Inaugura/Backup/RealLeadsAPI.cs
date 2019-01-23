using System;
using System.Collections.Generic;
using System.Text;

using Inaugura.RealLeads.Managers;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// The realleads programming interface
    /// </summary>
    public class RealLeadsAPI
    {
        #region Variables
        private Inaugura.Threading.ManagedThreadPool mThreadPool;
        private RealLeads.Data.IRealLeadsDataAdaptor mDataAdaptor;

        private AddressManager mAddressManager;
        private UserManager mUserManager;
        private ListingManager mListingManager;

        #endregion

        #region Properties
        /// <summary>
        /// The underlying data adaptor
        /// </summary>
        internal Data.IRealLeadsDataAdaptor Data
        {
            get
            {
                return this.mDataAdaptor;
            }
        }

        /// <summary>
        /// The address manager
        /// </summary>
        public AddressManager AddressManager
        {
            get
            {
                return this.mAddressManager;
            }
        }


        /// <summary>
        /// The user manager
        /// </summary>
        public UserManager UserManager
        {
            get
            {
                return this.mUserManager;
            }
        }

        /// <summary>
        /// The listing manager
        /// </summary>
        public ListingManager ListingManager
        {
            get
            {
                return this.mListingManager;
            }
        }

        /// <summary>
        /// A thread pool dedicated to the API
        /// </summary>
        internal Threading.ManagedThreadPool ThreadPool
        {
            get
            {
                return this.mThreadPool;
            }
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">The data adaptor</param>
        /// <param name="cache">The underlying cache implementation</param>
        public RealLeadsAPI(Data.IRealLeadsDataAdaptor data)
        {
            if(data == null)
                throw new ArgumentNullException("data");
            this.mDataAdaptor = data;

            this.mThreadPool = new Inaugura.Threading.ManagedThreadPool(1);

            this.mAddressManager = new AddressManager(this, data);
            this.mUserManager = new UserManager(this, data);
            this.mListingManager = new ListingManager(this, data);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="ex">The exception to log</param>
        public void LogError(Exception ex)
        {
            Inaugura.RealLeads.Administration.ErrorInformation errorInfo = new Inaugura.RealLeads.Administration.ErrorInformation(ex);
            this.LogError(errorInfo);
        }

        private delegate void LogErrorDelegate(Inaugura.RealLeads.Administration.ErrorInformation errorInfo);
        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="errorInfo">The error info</param>
        public void LogError(Inaugura.RealLeads.Administration.ErrorInformation errorInfo)
        {            
            LogErrorDelegate logErrorDelegate = delegate(Inaugura.RealLeads.Administration.ErrorInformation errorInformation)
            {
                this.Data.AdministrationStore.AddErrorInformation(errorInformation);
            };
            this.ThreadPool.QueueWorkItem(logErrorDelegate, errorInfo);
        }

        private delegate void LogUserLoginDelegate(User user);
        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        public User Login(string email, string password)
        {
            Inaugura.RealLeads.User user = this.UserManager.GetUserByEmail(email);
            if (user != null)
                if(user.Password.IsMatch(password))
                {
                    Login(user);
                    return user;
                }

            return null;
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="user">The user to login</param>
        public void Login(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            LogUserLoginDelegate loginDateDelegate = delegate(User u)
                   {
                       this.UserManager.UpdateUser(u);
                   };
            user.LastLoginDate = DateTime.Now;
            this.ThreadPool.QueueWorkItem(loginDateDelegate, user);
            Inaugura.RealLeads.Security.UserPrincipal.SetIdentity(user);
        }

        /// <summary>
        /// Locates a postal code
        /// </summary>
        /// <param name="postal">The postal code</param>
        /// <returns>The address located at the center of the postal code, or null if not found</returns>
        public Inaugura.Maps.Address LocatePostal(string postal)
        {
            postal = postal.ToUpper();
            postal = postal.Replace(" ", string.Empty);

            return this.Data.AddressStore.LocatePostal(postal);
        }
        #endregion
    }
}
