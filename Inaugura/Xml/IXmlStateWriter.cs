using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.Xml
{
    public interface IXmlStateWriter
    {
        /// <summary>
        /// Writes the object state to an xml writer
        /// </summary>
        /// <param name="writer">The xml writer</param>
        void Write(XmlWriter writer);       
    }
}
