using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Security
{
   /// <summary>
    /// The base class for all security exceptions
    /// </summary>
    public class SecurityException : ApplicationException
    {
        #region Internal Constructs
        /// <summary>
        /// An enumeration of security messages
        /// </summary>
        public enum SecurityMessages
        {
            /// <summary>
            /// Permission Required Exception
            /// </summary>
            PermissionRequired
        }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The exception message</param>
        public SecurityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The standard security message</param>
        public SecurityException(SecurityMessages message)
            : base(SecurityException.GetSecurityMessage(message))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The inner exception</param>
        public SecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private static string GetSecurityMessage(SecurityMessages message)
        {
            if (message == SecurityMessages.PermissionRequired)
                return "You do not have the required permission to perform the requested action.";

            throw new NotSupportedException("The security message was not supported");
        }
    }
}
