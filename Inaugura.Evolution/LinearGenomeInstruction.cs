using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Inaugura.Xml;


namespace Inaugura.Evolution
{
    /// <summary>
    /// A single linear genome instruction
    /// </summary>
    public class LinearGenomeInstruction: IXmlObject
    {
        #region Internal Constructs
        public struct Oprand
        {
            public int[] Values;            
        }

        #endregion

        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// The instruction opcode
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The instruction oprand
        /// </summary>
        public int[] Data { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public LinearGenomeInstruction()
        {            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="data">Data</param>
        public LinearGenomeInstruction(int code, int[] data)
        {
            this.Code = code;
            this.Data = data;
        }

        public LinearGenomeInstruction Clone()
        {
            List<int> list = new List<int>(this.Data);
            LinearGenomeInstruction lgi = new LinearGenomeInstruction(this.Code, list.ToArray());
            return lgi;
        }

        public void Mutate()
        {
            int rand = LinearGenome.Random.Next(1 + this.Data.Length);
            if (rand == 0)
                this.Code = LinearGenome.Random.Next();
            else
                this.Data[rand - 1] = LinearGenome.Random.Next();
        }

        public void MutateData()
        {
            int rand = LinearGenome.Random.Next(this.Data.Length);
            this.Data[rand] = LinearGenome.Random.Next();
        }

        public void MutateAll()
        {
            this.Code = LinearGenome.Random.Next();
            for(int i = 0; i < this.Data.Length; i++)
                this.Data[i] = LinearGenome.Random.Next();
        }

        #region IXmlStateReader Members
        public void Read(System.Xml.XmlReader reader)
        {
            this.Code = XmlConvert.ToInt32(Inaugura.Xml.Helper.ReadAttributeValue(reader, "code", true));
            reader.Read();
            reader.MoveToContent();            
            if (reader.LocalName.Equals("Data"))
            {
                List<int> values = new List<int>();
                bool more = true;
                while (more)
                {
                    values.Add(XmlConvert.ToInt32(reader.ReadElementString()));
                    if (!reader.LocalName.Equals("Value"))
                        more = false;
                }
                this.Data = values.ToArray();
            }            
        }

        #endregion

        #region IXmlStateWriter Members
        public void Write(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("code", this.Code.ToString());
            writer.WriteStartElement("Data");
            foreach (int value in this.Data)
                writer.WriteElementString("Value", value.ToString());
            writer.WriteEndElement();
        }
        #endregion
    }
}
