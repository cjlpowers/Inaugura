using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Xml
{
    /// <summary>
    /// A exception class which is thrown when xml data is missing
    /// </summary>
    public class XmlMissingException : XmlException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The exception message</param>
        public XmlMissingException(string message)
            : base(message)
        {
        }
    }
}
