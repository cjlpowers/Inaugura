using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Inaugura.Xml
{
    /// <summary>
    /// Supports reading and writing state to xml
    /// </summary>
    public interface IXmlObject : IXmlStateReader, IXmlStateWriter
    {        
    }
}
