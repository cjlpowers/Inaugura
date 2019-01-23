using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.Xml
{
    public interface IXmlStateReader
    {
        /// <summary>
        /// Reads the object state from an xml reader
        /// </summary>
        /// <param name="reader">The xml reader</param>
        void Read(XmlReader reader);        
    }
}
