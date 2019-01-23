using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using Inaugura.Xml;

namespace Inaugura.Evolution
{
    /// <summary>
    /// The class which represents an environment
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Environment<T> : Collection<T>, IXmlObject where T : Organism, new()
    {
        #region Events
        /// <summary>
        /// An event which is fired when an organism is added
        /// </summary>
        public event EventHandler<OrganismEventArgs> OrganismAdded;                
        private void OnOrganismAdded(Organism organism)
        {
            if (OrganismAdded != null)
                OrganismAdded(this, new OrganismEventArgs(organism));
        }

        /// <summary>
        /// An event which is fired when an organism is removed
        /// </summary>
        public event EventHandler<OrganismEventArgs> OrganismRemoved;
        private void OnOrganismRemoved(Organism organism)
        {
            if (OrganismRemoved != null)
                OrganismRemoved(this, new OrganismEventArgs(organism));
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets an organism with the specified ID
        /// </summary>
        /// <param name="id">The ID of the organism</param>
        /// <returns>The organsm witht he specified ID, otherwise null</returns>
        public T this[Guid id]
        {
            get
            {  
                foreach (T organism in this)
                    if (organism.ID == id)
                        return organism;
                return null;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The age of the environment in epochs
        /// </summary>
        public int Age { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Advances the environmet and the organisms it contains by one epoch
        /// </summary>
        public void Epoch()
        {
            this.RunEpoch();
            this.Age += 1;
        }

        protected abstract void RunEpoch();
        

        #region Loading and Saving

        /// <summary>
        /// Saves the entire environment to a file
        /// </summary>
        /// <param name="file"></param>
        public void Write(string file)
        {
            using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(file, null))
            {
                writer.WriteStartDocument();
                writer.WriteComment("Date: " + DateTime.Now.ToString());
                writer.WriteStartElement("Environment");
                this.Write(writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// Saves environment specific state to xml
        /// </summary>
        /// <param name="writer">THe xml writer</param>
        protected abstract void WriteState(System.Xml.XmlWriter writer);

        /// <summary>
        /// Loads organism specific state from xml
        /// </summary>
        /// <param name="writer">The xml writer</param>
        protected abstract void ReadState(System.Xml.XmlReader reader);
        #endregion

        /// <summary>
        /// Inerts an organism into the collection
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="item">The organism</param>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            this.OnOrganismAdded(item);
        }

        /// <summary>
        /// Removes an organism from the collection
        /// </summary>
        /// <param name="index">The index</param>
        protected override void RemoveItem(int index)
        {            
            T organism = this[index];
            base.RemoveItem(index);
            this.OnOrganismRemoved(organism);            
        }


        #endregion

        #region IXmlStateReader Members

        public void Read(XmlReader reader)
        {
            this.Age = XmlConvert.ToInt32(Inaugura.Xml.Helper.ReadAttributeValue(reader, "age", true));
            reader.Read();
            reader.MoveToContent();
            this.ReadState(reader);            
            if (reader.LocalName.Equals("Organisms"))
            {
                bool more = true;
                while (more)
                {
                    reader.Read();
                    reader.MoveToContent();
                    if (reader.LocalName.Equals("Organism"))
                        this.Add(Inaugura.Xml.Helper.Load<T>(reader));
                    else
                        more = false;
                    reader.Read();
                }                
            }
        }
        #endregion

        #region IXmlStateWriter Members
        public void Write(XmlWriter writer)
        {            
            Inaugura.Xml.Helper.WriteType(writer, this.GetType());
            writer.WriteAttributeString("age", this.Age.ToString());
            this.WriteState(writer);
            writer.WriteStartElement("Organisms");
            foreach (T organism in this)
            {
                writer.WriteStartElement("Organism");
                organism.Write(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}
