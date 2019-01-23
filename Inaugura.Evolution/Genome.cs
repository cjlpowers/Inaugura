using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inaugura.Xml;

namespace Inaugura.Evolution
{
    /// <summary>
    /// A class which represents all the inheritable traits of an organism
    /// </summary>
    public abstract class Genome: IXmlObject
    {
        #region Methods
        public Genome()
        {
        }

        #region Loading and Saving
        /// <summary>
        /// Writes genome specific state to xml
        /// </summary>
        /// <param name="writer">The xml writer</param>
        protected abstract void WriteState(System.Xml.XmlWriter writer);


        /// <summary>
        /// Reads genome specific state from xml
        /// </summary>
        /// <param name="writer">The xml writer</param>
        protected abstract void ReadState(System.Xml.XmlReader reader);
        #endregion
        #endregion

        #region IXmlStateReader Members
        public void Read(System.Xml.XmlReader reader)
        {            
            this.ReadState(reader);
        }
        #endregion
        #region IXmlStateWriter Members
        public void Write(System.Xml.XmlWriter writer)
        {
            this.WriteState(writer);
        }
        #endregion
    }
}
