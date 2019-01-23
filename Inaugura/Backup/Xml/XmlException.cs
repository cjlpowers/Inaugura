using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Xml
{
    /// <summary>
    /// A base class for all xml related exceptions
    /// </summary>
    public class XmlException : System.ApplicationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The exception message</param>
        public XmlException(string message) : base(message)
        {
        }
    }
}
