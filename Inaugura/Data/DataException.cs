using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.Data
{
    /// <summary>
    /// An exception that is thrown an error ocurrs during data opperations
    /// </summary>
    public class DataException : ApplicationException
    {
        public DataException()
        {
        }

        public DataException(string message)
            : base(message)
        {
        }

        public DataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
