using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Inaugura.Xml;

namespace Inaugura.Evolution
{
    /// <summary>
    /// A class which represents an organism
    /// </summary>
    public abstract class Organism: IXmlObject
    {
        #region Events
        /// <summary>
        /// An event which is fired when the organism expires
        /// </summary>
        public event EventHandler<OrganismEventArgs> Expired;
        protected void OnExpired()
        {
            if(Expired != null)
                this.Expired(this,new OrganismEventArgs(this));
        }
        #endregion

        #region Properties
        /// <summary>
        /// The ID of this organism
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// The organism's genetic material
        /// </summary>
        public abstract Genome Genome { get; }

        /// <summary>
        /// The age of the organism in number of epochs
        /// </summary>
        public int Age { get; protected set; }

        /// <summary>
        /// The fitness
        /// </summary>
        public double Fitness { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        protected Organism()
        {
            this.ID = Guid.NewGuid();
        }        

        /// <summary>
        /// Causes the organism to expire
        /// </summary>
        public void Expire()
        {
            this.OnExpired();
        }

        #region Loading and Saving       
        /// <summary>
        /// Saves organism specific state to xml
        /// </summary>
        /// <param name="writer">The xml writer</param>
        protected abstract void WriteState(System.Xml.XmlWriter writer);

               
        /// <summary>
        /// Loads organism specific state from xml
        /// </summary>
        /// <param name="writer">The xml writer</param>
        protected abstract void ReadState(System.Xml.XmlReader reader);        
        #endregion

        #endregion

        #region IXmlStateReader Members

        public void Read(XmlReader reader)
        {
            this.ID = new Guid(Inaugura.Xml.Helper.ReadAttributeValue(reader, "id", true));
            this.Age = int.Parse(Inaugura.Xml.Helper.ReadAttributeValue(reader, "age", true));
            this.Fitness = double.Parse(Inaugura.Xml.Helper.ReadAttributeValue(reader, "fitness", true));            
            this.ReadState(reader);
        }

        #endregion

        #region IXmlStateWriter Members

        public void Write(XmlWriter writer)
        {
            Inaugura.Xml.Helper.WriteType(writer, this.GetType());            
            writer.WriteAttributeString("id", this.ID.ToString());
            writer.WriteAttributeString("age", this.Age.ToString());
            writer.WriteAttributeString("fitness", this.Fitness.ToString());
            this.WriteState(writer);
        }
        #endregion
    }
}
