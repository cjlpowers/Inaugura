using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.RealLeads
{
    /// <summary>
    /// The user class
    /// </summary>
    public class User : Inaugura.Person
    {
        #region Internal Constructs
        /// <summary>
        /// A class which manages role information
        /// </summary>
        public class UserRoleCollection : List<Inaugura.Security.Role>, Xml.IXmlable
        {
            #region Indexers
            /// <summary>
            /// Indexer
            /// </summary>
            /// <param name="name">The role name</param>
            /// <returns>The role matching the name, otherwise null</returns>
            public Inaugura.Security.Role this[string name]
            {
                get
                {
                    lock (this)
                    {
                        foreach (Inaugura.Security.Role role in this)
                        {
                            if (role.Name == name)
                                return role;
                        }
                    }
                    return null;
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Constructor
            /// </summary>
            public UserRoleCollection()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="node">The xml node</param>
            public UserRoleCollection(XmlNode node)
            {
                this.PopulateInstance(node);
            }          

            /// <summary>
            /// Determins if the collection contains a role
            /// </summary>
            /// <param name="roleName">The role name</param>
            /// <returns></returns>
            public bool Contains(string roleName)
            {
                lock (this)
                {
                    foreach (Inaugura.Security.Role role in this)
                    {
                        if (role.Name == roleName)
                            return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Populates the instance with the specifed xml data
            /// </summary>
            /// <param name="node"></param>
            public virtual void PopulateInstance(XmlNode node)
            {
                base.Clear();
                XmlNodeList nodes = node.SelectNodes("Role");
                foreach (XmlNode roleNode in nodes)
                {
                    Inaugura.Security.Role role = new Inaugura.Security.Role(roleNode);
                    base.Add(role);
                }
            }

            /// <summary>
            /// Populates a xml node with the objects data
            /// </summary>
            /// <param name="node">The  node to populate</param>
            public virtual void PopulateNode(XmlNode node)
            {
                if (this.Count > 0)
                {
                    foreach (Inaugura.Security.Role role in this)
                    {
                        XmlNode roleNode = node.OwnerDocument.CreateElement("Role");
                        role.PopulateNode(roleNode);
                        node.AppendChild(roleNode);
                    }
                }
            }
            #endregion

            #region IXmlable Members

            public System.Xml.XmlNode Xml
            {
                get
                {
                    XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("Roles");
                    this.PopulateNode(node);
                    return node;
                }
            }
            #endregion
        }
        #endregion

        #region Variables
        private Guid mID;        
        private Inaugura.Security.SaltedHash mPassword;
        private string mPasswordQuestion;
        private string mPasswordAnswer;
        private ContactScheduleCollection mContactSchedules;
        private Details mDetails;
        private int mMaxListings;
        private int mMaxImages;
        private string mPinCode;
        private DateTime mCreationDate;
        private DateTime mLastLoginDate;
        private UserRoleCollection mRoles;
        private bool mPhoneNumberPrivate;
        private bool mEmailVerified;
        private TimeSpan mListingExpiration;
        #endregion

        #region IXmlable Members
        /// <summary>
        /// Gets the xml representation of the Address
        /// </summary>
        /// <value></value>
        new public XmlNode Xml
        {
            get
            {
                XmlNode node = Inaugura.Xml.Helper.NewNodeDocument("User");
                this.PopulateNode(node);
                return node;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The ID of the user
        /// </summary>
        public Guid ID
        {
            get
            {
                return this.mID;
            }
            protected set
            {
                this.mID = value;
            }
        }

        /// <summary>
        /// The first name
        /// </summary>
        public new string FirstName
        {
            get
            {
                return base.FirstName;
            }
            set
            {
                this.EnforceEditPolicy();
                base.FirstName = value;
            }
        }

        /// <summary>
        /// The password
        /// </summary>
        public Inaugura.Security.SaltedHash Password
        {
            get
            {
                return this.mPassword;
            }
            private set
            {
                this.mPassword = value;
            }
        }

        /// <summary>
        /// The password question
        /// </summary>
        public string PasswordQuestion
        {
            get
            {
                return this.mPasswordQuestion;
            }
            set
            {
                this.mPasswordQuestion = value;
            }
        }

        /// <summary>
        /// The password answer
        /// </summary>
        public string PasswordAnswer
        {
            get
            {
                return this.mPasswordAnswer;
            }
            set
            {
                this.mPasswordAnswer = value;
            }
        }

        /// <summary>
        /// Addidtional Details
        /// </summary>
        public Inaugura.Details Details
        {
            get
            {
                return this.mDetails;
            }
            set
            {
                this.mDetails = value;
            }
        }

        /// <summary>
        /// Contact schedule
        /// </summary>
        /// <value></value>		
        public ContactScheduleCollection ContactSchedules
        {
            get
            {
                return this.mContactSchedules;
            }
            private set
            {
                this.mContactSchedules = value;
            }
        }

        /// <summary>
        /// The maximum number of images
        /// </summary>
        public int MaxImages
        {
            get
            {
                return this.mMaxImages;
            }
            set
            {
                this.mMaxImages = value;
            }
        }

        /// <summary>
        /// The maximum number of listings
        /// </summary>
        public int MaxListings
        {
            get
            {
                return this.mMaxListings;
            }
            set
            {
                this.mMaxListings = value;
            }
        }

        /// <summary>
        /// The listing expiration
        /// </summary>
        public TimeSpan ListingExpiration
        {
            get
            {
                return this.mListingExpiration;
            }
            set
            {
                this.mListingExpiration = value;
            }
        }


        /// <summary>
        /// The users pincode
        /// </summary>
        public string PinCode
        {
            get
            {
                return this.mPinCode;
            }
            set
            {
                this.EnforceEditPolicy();
                this.mPinCode = value;
            }
        }
        
        /// <summary>
        /// The creation date for this user
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return this.mCreationDate;
            }
            set
            {
                this.mCreationDate = value;
            }
        }

        /// <summary>
        /// The creation date for this user
        /// </summary>
        public DateTime LastLoginDate
        {
            get
            {
                return this.mLastLoginDate;
            }
            set
            {
                this.mLastLoginDate = value;
            }
        }

        /// <summary>
        /// The roles that this user is a member of
        /// </summary>
        public Inaugura.Security.Role[] Roles
        {
            get
            {
                return this.mRoles.ToArray() ;
            }        
        }

        /// <summary>
        /// A flag which indicates the users phone number is hidden from other users
        /// </summary>
        public bool PhoneNumberPrivate
        {
            get
            {
                return this.mPhoneNumberPrivate;
            }
            set
            {
                this.mPhoneNumberPrivate = value;
            }
        }

        /// <summary>
        /// A flag which indicates the users email address has been verified.
        /// </summary>
        public bool EmailVerified
        {
            get
            {
                return this.mEmailVerified;
            }
            set
            {
                this.mEmailVerified = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node"></param>
        public User(XmlNode node) : base(node)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
            this.mMaxImages = 5;
            this.mMaxListings = 1;
            this.mListingExpiration = TimeSpan.FromDays(60);
            this.mID = Guid.NewGuid();
            this.mCreationDate = DateTime.Now;
            this.mLastLoginDate = DateTime.MinValue;
            this.Details = new Details();
            this.ContactSchedules = new ContactScheduleCollection();
            this.mRoles = new UserRoleCollection();
            this.mListingExpiration = TimeSpan.FromDays(60);
        }

         /// <summary>
        /// Populates the instance with the specifed xml data
        /// </summary>
        /// <param name="node"></param>
        public override void PopulateInstance(XmlNode node)
        {
            base.PopulateInstance(node);

            Inaugura.Xml.Helper.EnsureAttributeExists(node, "id");
            Inaugura.Xml.Helper.EnsureAttributeExists(node, "creationDate");
            Inaugura.Xml.Helper.EnsureNodeExists(node, "Password");

            this.mID = new Guid(node.Attributes["id"].Value);
            this.mCreationDate = DateTime.ParseExact(node.Attributes["creationDate"].Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal);                

            this.Password = new Inaugura.Security.SaltedHash(node["Password"]);

            if (node.Attributes["lastLogin"] != null)
                this.mLastLoginDate = DateTime.Parse(node.Attributes["lastLogin"].Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal);                

            if (node.Attributes["maxListings"] != null)
                this.MaxListings = int.Parse(node.Attributes["maxListings"].Value);

            if (node.Attributes["maxImages"] != null)
                this.MaxImages = int.Parse(node.Attributes["maxImages"].Value);

            if (node.Attributes["listingExpiration"] != null)
                this.mListingExpiration = TimeSpan.FromDays(double.Parse(Inaugura.Xml.Helper.GetAttribute(node, "listingExpiration")));

            if (node.Attributes["pinCode"] != null)
                this.PinCode = node.Attributes["pinCode"].Value;

            if (node.Attributes["emailVerified"] != null)
                this.EmailVerified = bool.Parse(node.Attributes["emailVerified"].Value);

            if (node["PasswordQuestion"] != null)
                this.PasswordQuestion = node["PasswordQuestion"].InnerText;

            if (node.Attributes["phoneNumberPrivate"] != null)
                this.PhoneNumberPrivate = bool.Parse(node.Attributes["phoneNumberPrivate"].Value);

            if (node["PasswordAnswer"] != null)
                this.PasswordAnswer = node["PasswordAnswer"].InnerText;
            
            if (node["Details"] != null)
                this.Details = new Details(node["Details"]);
            else
                this.Details = new Details();

            if (node["ContactSchedules"] != null)
                this.ContactSchedules = new ContactScheduleCollection(node["ContactSchedules"]);
            else
                this.ContactSchedules = new ContactScheduleCollection();
            
            if(node["Roles"] != null)
                this.mRoles = new UserRoleCollection(node["Roles"]);     
            else
                this.mRoles = new UserRoleCollection();
        }

        /// <summary>
        /// Populates a xml node with the objects data
        /// </summary>
        /// <param name="node">The  node to populate</param>
        public override void PopulateNode(XmlNode node)
        {
            Inaugura.Xml.Helper.SetAttribute(node, "id", this.ID.ToString());
            Inaugura.Xml.Helper.SetAttribute(node, "creationDate", this.CreationDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

            base.PopulateNode(node);            

            if(this.mLastLoginDate != DateTime.MinValue)
                Inaugura.Xml.Helper.SetAttribute(node, "lastLogin", this.mLastLoginDate.ToString(System.Globalization.CultureInfo.InvariantCulture));
            
            if (this.MaxListings > 0)
                Inaugura.Xml.Helper.SetAttribute(node, "maxListings", this.MaxListings.ToString());

            if (this.MaxImages > 0)
                Inaugura.Xml.Helper.SetAttribute(node, "maxImages", this.MaxImages.ToString());

            Inaugura.Xml.Helper.SetAttribute(node, "listingExpiration", this.mListingExpiration.TotalDays.ToString());

            if (this.PinCode != null && this.PinCode != string.Empty)
                Inaugura.Xml.Helper.SetAttribute(node, "pinCode", this.PinCode);

            if (this.PhoneNumberPrivate)
                Inaugura.Xml.Helper.SetAttribute(node, "phoneNumberPrivate", this.PhoneNumberPrivate.ToString());

            if (this.EmailVerified)
                Inaugura.Xml.Helper.SetAttribute(node, "emailVerified", this.EmailVerified.ToString());

            XmlNode passwordNode = node.OwnerDocument.CreateElement("Password");
            this.mPassword.PopulateNode(passwordNode);
            node.AppendChild(passwordNode);

            XmlNode pqNode = node.OwnerDocument.CreateElement("PasswordQuestion");
            pqNode.InnerText = this.PasswordQuestion;
            node.AppendChild(pqNode);

            pqNode = node.OwnerDocument.CreateElement("PasswordAnswer");
            pqNode.InnerText = this.PasswordAnswer;
            node.AppendChild(pqNode);

            if (this.ContactSchedules.Count > 0)
            {
                XmlNode contactSchedulesNode = node.OwnerDocument.CreateElement("ContactSchedules");
                this.ContactSchedules.PopulateNode(contactSchedulesNode);
                node.AppendChild(contactSchedulesNode);
            }

            if (this.Details.Count > 0)
            {
                XmlNode detailsNode = node.OwnerDocument.CreateElement("Details");
                this.mDetails.PopulateNode(detailsNode);
                node.AppendChild(detailsNode);
            }

            if (this.mRoles.Count > 0)
            {
                XmlNode rolesNode = node.OwnerDocument.CreateElement("Roles");
                this.mRoles.PopulateNode(rolesNode);
                node.AppendChild(rolesNode);
            }
        }

        /// <summary>
        /// Adds a role to the users profile
        /// </summary>
        /// <param name="role">The role to add</param>
        public void AddRole(Inaugura.Security.Role role)
        {
            Inaugura.Security.Role.EnforceRole(UserRoles.Administrator);
            this.mRoles.Add(role);
        }

        /// <summary>
        /// Removes a role from the users profile
        /// </summary>
        /// <param name="role">The role to remove</param>
        public void RemoveRole(Inaugura.Security.Role role)
        {
            Inaugura.Security.Role.EnforceRole(UserRoles.Administrator);
            this.mRoles.Remove(role);
        }
        
        /// <summary>
        /// Determins if the user is in a specific role
        /// </summary>
        /// <param name="role">The role</param>
        /// <returns>True if the user is in the role, otherwise false</returns>
        public bool IsInRole(Inaugura.Security.Role role)
        {
            return this.IsInRole(role.Name);
        }

        /// <summary>
        /// Determins if a user is in a specific role
        /// </summary>
        /// <param name="roleName">The role name</param>
        /// <returns></returns>
        public bool IsInRole(string roleName)
        {
            return this.mRoles.Contains(roleName);
        }

        /// <summary>
        /// Determins if a user can edit this listing
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>True if the listing can be editied by the user, otherwise false</returns>
        public bool CheckEditPolicy(Inaugura.RealLeads.User user)
        {
            if (user != null && (user.IsInRole(UserRoles.Administrator) || user.ID == this.ID))
                return true;
            return false;
        }

        /// <summary>
        /// Determins if the principal of the active thread context can edit this listing
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>True if the listing can be editied by the user, otherwise false</returns>
        public bool CheckEditPolicy()
        {
            if (System.Threading.Thread.CurrentPrincipal is Inaugura.RealLeads.UserPrincipal)
            {
                if (Inaugura.Security.Role.IsInRole(UserRoles.Administrator) || System.Threading.Thread.CurrentPrincipal.Identity.Name == this.Email)
                    return true;
                return false;
            }
            else
                return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void EnforceEditPolicy()
        {
            if (!this.CheckEditPolicy())
                throw new Inaugura.Security.SecurityException("Permission is required to edit this user");
        }

        /// <summary>
        /// Sets the users password
        /// </summary>
        /// <param name="password">The new password</param>
        public void SetPassword(string password)
        {
            // perform security check
            //if (!Inaugura.Security.Role.IsInRole(UserRoles.Administrator) && !User.IsIdentity(this))
                //throw new Inaugura.Security.SecurityException("Cannot set the password");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Cannot use an empty password");

            this.Password = new Inaugura.Security.SaltedHash(password);
        }                  

        /// <summary>
        /// Checks if the current thread context identity is a specific user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>True if the idendity of the current thread context is the specified user, otherwise false</returns>
        public static bool IsIdentity(User user)
        {
            System.Security.Principal.IIdentity identity = System.Threading.Thread.CurrentPrincipal.Identity;
            if (identity.Name == user.Email)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Tostring method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }


        #region Business Logic
        /// <summary>
        /// Calculates a listing expiration date, given the starting date.
        /// </summary>
        /// <param name="startDate">The starting date</param>
        /// <returns>The expiration date</returns>
        public DateTime CalculateListingExpiration(DateTime startDate)
        {
                return startDate+this.ListingExpiration;          
        }

        /// <summary>
        /// Calculates a listing expiration date.
        /// </summary>
        /// <returns>The expiration date</returns>
        public DateTime CalculateListingExpiration()
        {
            return CalculateListingExpiration(DateTime.Now);
        }

        #region Promotions
        /// <summary>
        /// Applies a promotion to the users account
        /// </summary>
        /// <param name="promotion">The promotion to apply</param>
        public void ApplyPromotion(Inaugura.RealLeads.RealLeadsAPI api, Promotion promotion)
        {
            if (promotion != null)
            {
                XmlNode node = this.Xml;
                promotion.Apply(node);
                this.PopulateInstance(node);

                api.UserManager.UpdateUser(this);

                promotion.Count = promotion.Count - 1;
                if (promotion.Count <= 0)
                    api.UserManager.RemovePromotion(promotion);
                else
                    api.UserManager.UpdatePromotion(promotion);
            }
        }
        #endregion
        #endregion

        #endregion
    }
}
