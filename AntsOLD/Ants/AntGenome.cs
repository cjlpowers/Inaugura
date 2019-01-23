using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Inaugura.Evolution;

namespace Ants
{
    /// Instructions
    /// 0: Skip Next Instruction
    /// 1: Eat
    /// 2: Move
    /// 3: Reproduce
    /// 4: Conditional
    /// 5: Link (previous instruction to the next)    
    
    /// 6: Wait

    /// <summary>
    /// A class representing a ant genome
    /// </summary>
    public class AntGenome : LinearGenome
    {
        #region Variables
        public const int ActionCount = 6;
        int mIndex;
        #endregion

        /// <summary>
        /// The current instruction
        /// </summary>
        public LinearGenomeInstruction Instruction
        {
            get
            {
                  return mInstructions[GetIndex(mIndex)];
            }
        }

        /// <summary>
        /// The current instruction
        /// </summary>
        public LinearGenomeInstruction NextInstruction
        {
            get
            {
                return mInstructions[GetIndex(mIndex+1)];
            }
        }

        public int Size
        {
            get
            {
                return this.mInstructions.Count;
            }
        }

        #region Methods
        public AntGenome()
        {
            this.mInstructions = new List<LinearGenomeInstruction>();            
        }

        public AntGenome(int initialGenomeSize) : this()
        {
            for (int i = 0; i < initialGenomeSize; i++)
                this.mInstructions.Add(this.NewInstruction());
        }

        public AntGenome(IEnumerable<LinearGenomeInstruction> instructions) : base(instructions)
        {
        }

        public AntGenome Mutate()
        {
            int count = Random.Next(3);
            AntGenome genome = new AntGenome(this.mInstructions);
            for (int i = 0; i < count; i++)
            {
                int action = Random.Next(100);
                if (action > 0 && action < 20) // add a new item
                    genome.mInstructions.Insert(Random.Next(genome.mInstructions.Count + 1), NewInstruction());
                else if (action >= 20 && action < 40)
                {
                    int index = Random.Next(genome.mInstructions.Count);
                    LinearGenomeInstruction instruction = genome.mInstructions[index];
                    instruction.Mutate();
                    instruction.Code = instruction.Code % (ActionCount+1);                    
                }
                else if (action >= 40 && action < 50 && genome.mInstructions.Count > 1)
                    genome.mInstructions.RemoveAt(Random.Next(genome.mInstructions.Count));
            }
            return genome;
        }

        protected virtual LinearGenomeInstruction NewInstruction()
        {
            return new LinearGenomeInstruction(Random.Next(ActionCount), RandomData(3));
        }
        private int[] RandomData(int size)
        {
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
                data[i] = Random.Next();
            return data;
        }

        public LinearGenomeInstruction MoveNext()
        {            
            mIndex = this.GetIndex(mIndex+1);
            return mInstructions[mIndex];
        }


        private int GetIndex(int index)
        {
            if (index <= 0)
                index = 0;
            else if (index >= this.mInstructions.Count)
            {
                index -= this.mInstructions.Count;
                return GetIndex(index);
            }
            return index;
        }
        

        #region State Management
        protected override void WriteState(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("index", this.mIndex.ToString());
            base.WriteState(writer);
        }

        protected override void ReadState(System.Xml.XmlReader reader)
        {
            this.mIndex = XmlConvert.ToInt32(Inaugura.Xml.Helper.ReadAttributeValue(reader, "index", true));
            base.ReadState(reader);
        }
        #endregion
        #endregion

    }
}
