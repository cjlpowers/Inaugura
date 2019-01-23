using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Inaugura.Evolution
{
    /// <summary>
    /// A class representing a linear gemome
    /// </summary>
    public class LinearGenome : Genome
    {
        #region Variables
        protected List<LinearGenomeInstruction> mInstructions;
        public static System.Random Random = new Random((int)DateTime.Now.Ticks);
        #endregion

        protected LinearGenome(IEnumerable<LinearGenomeInstruction> instructions) : this()
        {
            foreach (LinearGenomeInstruction instruction in instructions)
                this.mInstructions.Add(instruction.Clone());
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LinearGenome()
        {
            this.mInstructions = new List<LinearGenomeInstruction>();
        }

        protected override void WriteState(System.Xml.XmlWriter writer)
        {            
            foreach (LinearGenomeInstruction i in this.mInstructions)
            {
                writer.WriteStartElement("I");
                i.Write(writer);
                writer.WriteEndElement();                
            }
        }

        protected override void ReadState(System.Xml.XmlReader reader)
        {            
            this.mInstructions.Clear();
            List<int> values = new List<int>();
            bool more = true;
            while (more)
            {
                reader.Read();
                reader.MoveToContent();
                if (reader.LocalName.Equals("I"))
                {
                    LinearGenomeInstruction lgi = new LinearGenomeInstruction();
                    lgi.Read(reader);
                    this.mInstructions.Add(lgi);
                    reader.Read();
                }
                else
                    more = false;
                
            }
        }

        #region Execution
        #endregion

    }
}
