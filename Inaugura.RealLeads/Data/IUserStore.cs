using System;

namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Interface definition for the User Store
	/// </summary>
    public interface IUserStore
    {
        #region Agent Specific
        /// <summary>
        /// Adds a user to the store
        /// </summary>
        /// <param name="user">The user to add</param>
        void Add(User user);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The user, otherwise null</returns>
        User GetUser(Guid id);

        /// <summary>
        /// Gets a user based on a email
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <returns>The user with the specified email address, otherwise null</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="user">The user to update</param>
        void Update(User user);

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <param name="user">The user to remove</param>
        void Remove(User user);

        /// <summary>
        /// Searches the store for users matching specific criteria
        /// </summary>
        /// <param name="search">The search criteria</param>
        /// <returns>The search results</returns>
        User[] SearchUsers(UserSearch search);       
        #endregion

        #region User Files
        /// <summary>
        /// Adds a file to an User profile
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <param name="file">The File to add</param>
        void AddFile(string userId, File file);

        /// <summary>
        /// Gets a File
        /// </summary>
        /// <param name="fileId">The ID of the File</param>
        /// <returns>The File with the specified ID</returns>
        File GetFile(string fileId);

        /// <summary>
        /// Removes a File
        /// </summary>
        /// <param name="fileId">The ID of the File</param>
        /// <returns>The File with the specified ID</returns>
        bool RemoveFile(string fileId);

        /// <summary>
        /// Updates a File
        /// </summary>
        /// <param name="file">The updated File</param>
        /// <returns></returns>
        bool UpdateFile(File file);
        #endregion

        #region Roles
        /// <summary>
        /// Gets a list of all the currently defined roles
        /// </summary>
        /// <returns></returns>
        Inaugura.Security.Role[] GetRoles();

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role">The role to add</param>
        void AddRole(Inaugura.Security.Role role);

        /// <summary>
        /// Removes a role
        /// </summary>
        /// <param name="role">The role to remove</param>
        void RemoveRole(Inaugura.Security.Role role);
        #endregion

        #region Messages
        /// <summary>
        /// Gets the list of messages for a given user
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <returns>The list of messages</returns>
        Message[] GetMessages(Guid userID);

        /// <summary>
        /// Gets a specific message
        /// </summary>
        /// <param name="id">The message ID</param>
        /// <returns>The message, otherwise null if not found</returns>
        Message GetMessage(Guid id);

        /// <summary>
        /// Adds a message
        /// </summary>
        /// <param name="message">The message to add</param>
        void AddMessage(Message message);

        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="message">The message to update</param>
        void UpdateMessage(Message message);

        /// <summary>
        /// Removes a message
        /// </summary>
        /// <param name="id">The id of the message to remove</param>
        void RemoveMessage(string id);
        #endregion

        #region Promotions
        /// <summary>
        /// Gets the list of promotions
        /// </summary>
        /// <returns>The list of promotions</returns>
        Promotion[] GetPromotions();

        /// <summary>
        /// Gets a promotion by ID
        /// </summary>
        /// <param name="id">The promotion ID</param>
        /// <returns>The promotion if found, otherwise null</returns>
        Promotion GetPromotion(Guid id);

        /// <summary>
        /// Gets a promotion by code
        /// </summary>
        /// <param name="code">The </param>
        /// <returns></returns>
        Promotion GetPromotionByCode(string code);

        /// <summary>
        /// Adds a promotion to the system
        /// </summary>
        /// <param name="promotion">The promotion to add</param>
        void AddPromotion(Promotion promotion);

        /// <summary>
        /// Removes a promotion
        /// </summary>
        /// <param name="id">The id of the promotion</param>
        void RemovePromotion(Guid id);

        /// <summary>
        /// Updates a promotion
        /// </summary>
        /// <param name="promotion">The promotion to update</param>
        void UpdatePromotion(Promotion promotion);
        #endregion
    }
}
