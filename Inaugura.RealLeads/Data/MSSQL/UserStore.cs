using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using Inaugura.Data;


namespace Inaugura.RealLeads.Data
{
    class UserStore : SqlDataStore, IUserStore
    {
        #region Variables
        private IRealLeadsDataAdaptor mAdaptor;
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataAdaptor">The realleads data adaptor</param>
        /// <param name="connectionString">The database connection string</param>
        public UserStore(IRealLeadsDataAdaptor dataAdaptor, string connectionString)
            : base(connectionString)
		{
			this.mAdaptor = dataAdaptor;
        }

        /// <summary>
        /// Creates a user object from a reader
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>The user object</returns>
		internal static User GetUserFromReader(SqlDataReader reader)
		{
			string xml = Convert.ToString(reader["xml"]);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
            return new User(xmlDoc.DocumentElement);
		}

        /// <summary>
        /// Creates a role object from a reader
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>The role object</returns>
        internal static Inaugura.Security.Role GetRoleFromReader(SqlDataReader reader)
        {
            string xml = Convert.ToString(reader["xml"]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return new Inaugura.Security.Role(xmlDoc.DocumentElement);
        }

        #region IUserStore Members
        /// <summary>
        /// Adds a user to the store
        /// </summary>
        /// <param name="user">The user to add</param>
        public void Add(User user)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Users_AddUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);

                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(user.Xml.OuterXml, XmlNodeType.Document, null));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The user, otherwise null</returns>
        public User GetUser(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Users_GetUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return UserStore.GetUserFromReader(reader);
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Gets a user based on a email
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <returns>The user with the specified email address, otherwise null</returns>
        public User GetUserByEmail(string email)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Users_GetUserByEmail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@email", email));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return UserStore.GetUserFromReader(reader);
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="user">The user to update</param>
        public void Update(User user)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Users_UpdateUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(user.Xml.OuterXml, XmlNodeType.Document, null));
                    
                    if(cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException(string.Format("An attempt to update user {0} resulted in zero modified records",user.ID));
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <param name="user">The user to remove</param>        
        public void Remove(User user)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Users_RemoveUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", user.ID));

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException(string.Format("An attempt to remove user {0} resulted in zero modified records", user.ID));
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Searches the store for users matching specific criteria
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The search results</returns>
        public User[] SearchUsers(UserSearch search)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Users_Search";
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    if(search.FirstName != null)
                        cmd.Parameters.Add(new SqlParameter("@firstName", search.FirstName.GetSqlString()));
                    if (search.LastName != null)
                        cmd.Parameters.Add(new SqlParameter("@lastName", search.LastName.GetSqlString()));
                    if (search.Email != null)
                        cmd.Parameters.Add(new SqlParameter("@email", search.Email.GetSqlString()));
                    if (search.PhoneNumber != null)
                        cmd.Parameters.Add(new SqlParameter("@phoneNumber", search.PhoneNumber.GetSqlString()));
                    if (search.CreationDateStart != DateTime.MinValue)
                        cmd.Parameters.Add(new SqlParameter("@creationDateStart", search.CreationDateStart));
                    if (search.CreationDateEnd != DateTime.MaxValue)
                        cmd.Parameters.Add(new SqlParameter("@creationDateEnd", search.CreationDateEnd));
                    if (search.LastLoginStart != DateTime.MinValue)
                        cmd.Parameters.Add(new SqlParameter("@lastLoginStart", search.LastLoginStart));
                    if (search.LastLoginEnd != DateTime.MaxValue)
                        cmd.Parameters.Add(new SqlParameter("@lastLoginEnd", search.LastLoginEnd));
                    if (search.EmailVerification != UserSearch.EmailVerificationStatus.Any)
                    {
                        if (search.EmailVerification == UserSearch.EmailVerificationStatus.Unverified)
                            cmd.Parameters.Add(new SqlParameter("@emailVerified", false));
                        else if (search.EmailVerification == UserSearch.EmailVerificationStatus.Verified)
                            cmd.Parameters.Add(new SqlParameter("@emailVerified", true));
                        else
                            throw new NotSupportedException("The email verification status was not supported");
                    }

                    cmd.Parameters.Add(new SqlParameter("@startIndex", search.StartIndex));
                    cmd.Parameters.Add(new SqlParameter("@endIndex", search.EndIndex));

                    if (search.CalculateResultCount)
                        cmd.Parameters.Add(new SqlParameter("@calculateRecordCount", search.CalculateResultCount));

                    cmd.Parameters.Add("@retcode", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    List<User> list = new List<User>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = GetUserFromReader(reader);
                            list.Add(user);
                        }
                    }

                    //get the total record count
                    int result = Convert.ToInt32(cmd.Parameters["@retcode"].Value);
                    if (result != 0)
                    {
                        search.ResultCount = result;
                        search.CalculateResultCount = false;
                    }

                    System.Diagnostics.Debug.WriteLine(string.Format("Searching  returned {0} users", list.Count));
                    return list.ToArray();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public void AddFile(string userId, File file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public File GetFile(string fileId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool RemoveFile(string fileId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool UpdateFile(File file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #region Roles
        /// <summary>
        /// Gets a list of all the currently defined roles
        /// </summary>
        /// <returns></returns>
        public Inaugura.Security.Role[] GetRoles()
        {
            List<Inaugura.Security.Role> list = new List<Inaugura.Security.Role>();
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Roles_GetRoles";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(GetRoleFromReader(reader));
                        }
                    }
                    return list.ToArray();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role">The role to add</param>
        public void AddRole(Inaugura.Security.Role role)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Roles_AddRole";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);

                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(role.Xml.OuterXml, XmlNodeType.Document, null));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Removes a role
        /// </summary>
        /// <param name="role">The role to remove</param>
        public void RemoveRole(Inaugura.Security.Role role)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Roles_RemoveRole";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", role.Name));

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Inaugura.Data.DataException(string.Format("An attempt to remove role {0} resulted in zero modified records", role.Name));
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        #endregion

        #region Messages

        /// <summary>
        /// Gets a message from a reader
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>The message</returns>
        private static Message GetMessageFromReader(SqlDataReader reader)
        {
            string xml = Convert.ToString(reader["xml"]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return new Inaugura.RealLeads.Message(xmlDoc.DocumentElement);
        }

        /// <summary>
        /// Gets the list of messages for a given user
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <returns>The list of messages</returns>
        public Message[] GetMessages(Guid userID)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    List<Message> list = new List<Message>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Messages_GetMessages";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userID", userID));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(GetMessageFromReader(reader));

                        return list.ToArray();
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Gets a specific message
        /// </summary>
        /// <param name="id">The message ID</param>
        /// <returns>The message, otherwise null if not found</returns>
        public Message GetMessage(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Messages_GetMessage";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetMessageFromReader(reader);
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Adds a message
        /// </summary>
        /// <param name="message">The message to add</param>
        public void AddMessage(Message message)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Messages_AddMessage";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(message.Xml.OuterXml, XmlNodeType.Document, null));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="message">The message to update</param>
        public void UpdateMessage(Message message)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Messages_UpdateMessage";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(message.Xml.OuterXml, XmlNodeType.Document, null));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        /// <summary>
        /// Removes a message
        /// </summary>
        /// <param name="id">The id of the message to remove</param>
        public void RemoveMessage(string id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Messages_RemoveMessage";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    cmd.ExecuteNonQuery();                    
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        
        #endregion

        #region Promotions
        /// <summary>
        /// Gets a message from a reader
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>The message</returns>
        private static Promotion GetPromotionFromReader(SqlDataReader reader)
        {
            string xml = Convert.ToString(reader["xml"]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return new Inaugura.RealLeads.Promotion(xmlDoc.DocumentElement);
        }

        public Promotion[] GetPromotions()
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    List<Promotion> list = new List<Promotion>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Promotions_GetPromotions";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(GetPromotionFromReader(reader));

                        return list.ToArray();
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public Promotion GetPromotion(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Promotions_GetPromotion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetPromotionFromReader(reader);
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public Promotion GetPromotionByCode(string code)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Promotions_GetPromotionByCode";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@code", code));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return GetPromotionFromReader(reader);
                        else
                            return null;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public void AddPromotion(Promotion promotion)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Promotions_AddPromotion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(promotion.Xml.OuterXml, XmlNodeType.Document, null));

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public void RemovePromotion(Guid id)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Promotions_RemovePromotion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@promotionID", id));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }

        public void UpdatePromotion(Promotion promotion)
        {
            try
            {
                using (SqlConnection connection = SQLAdaptor.EstablishConnection(this.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "dbo.Promotions_UpdatePromotion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@xml", SqlDbType.Xml);
                    cmd.Parameters["@xml"].Value = new System.Data.SqlTypes.SqlXml(new System.Xml.XmlTextReader(promotion.Xml.OuterXml, XmlNodeType.Document, null));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlException)
            {
                throw new Inaugura.Data.DataException("Error during Sql opperation", sqlException);
            }
        }
        #endregion
        #endregion
    }
}
