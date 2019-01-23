using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Inaugura.Evolution;

namespace Ants
{
    public class Ant: Organism, IDrawer
    {
        #region Variables        
        AntGenome mGenome;
        Vector2 mPosition;
        Color mColor;
        int mGeneration;
        #endregion    

        #region Properties
        public override Genome Genome
        {
            get { return this.mGenome; }
        }

        public SandBox SandBox { get; set; }

        public float Energy { get; protected set; }

        public int GenomeLength
        {
            get
            {
                return this.mGenome.Size;
            }
        }

        public int Generation
        {
            get
            {
                return this.mGeneration;
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.mPosition;
            }
            set
            {
                this.mPosition = value;
            }
        }
        #endregion

        #region Methods
        public Ant()
        {
            this.mGenome = new AntGenome();
        }

        public Ant(Vector2 position, int genomeSize)
        {
            this.mColor = Color.White;//new Color(new Vector3((float)LinearGenome.Random.NextDouble(), (float)LinearGenome.Random.NextDouble(), (float)LinearGenome.Random.NextDouble()));
            this.mPosition = position;
            this.mGenome = new AntGenome(genomeSize);
            this.Energy = 500+LinearGenome.Random.Next(500);
        }

        public Ant(AntGenome genome)
        {
            this.mColor = Color.White;//new Color(new Vector3((float)LinearGenome.Random.NextDouble(), (float)LinearGenome.Random.NextDouble(), (float)LinearGenome.Random.NextDouble()));            
            this.mGenome = genome;
            this.Energy = 500 + LinearGenome.Random.Next(500);
        }
        #endregion

        #region Behaviour

        public void ProcessEpoch(SandBox sandbox)
        {
            float processTimeRemaining = 1.0f;
            while (processTimeRemaining > 0 && this.Energy > 0)
            {
                LinearGenomeInstruction instruction = this.mGenome.Instruction;               
                float processTime = this.ProcessInstruction(sandbox, instruction, processTimeRemaining);
                if (processTime > 0)
                {
                    processTimeRemaining -= processTime;
                    this.mGenome.MoveNext();
                }
                else
                    break;                
            }

            Energy -= 1+(float)this.mGenome.Size * 0.1f;

            this.Age++;
            if (this.Energy <= 0 || this.Age > 100)
            {
                sandbox.Remove(this);
                this.Expire();
            }            
        }

        private float ProcessInstruction(SandBox sandbox, LinearGenomeInstruction instruction, float processTimeRemaining)
        {
            float processTime = 0.01f;
            if (instruction.Code == 0)
            {
                this.mGenome.MoveNext();
                this.mGenome.MoveNext();
            }
            else if (instruction.Code == 1) // eat
            {
                this.Eat(sandbox.GetTile(this.mPosition));
                processTime = 0.1f;
            }
            else if (instruction.Code == 2) // move
            {
                Vector2 vect = new Vector2((float)((instruction.Data[0] % 200) - 100) / (this.SandBox.Size * 40000), (float)((instruction.Data[1] % 200) - 100) / (this.SandBox.Size * 40000));                
                this.Move(sandbox, vect);
            }
            else if (instruction.Code == 3) // reproduce
            {
                if (processTimeRemaining > 0.75f && Energy > 500)
                {
                    this.Reproduce(sandbox, (float)((instruction.Data[0] % 50)) / 100);
                    processTime = 1.0f;
                }
                else
                    processTime = 0;
            }
            else if (instruction.Code == 4) // conditional
            {
                if (!this.Condition(instruction))
                {
                    this.mGenome.MoveNext();
                    //this.mGenome.MoveNext();
                    while (mGenome.NextInstruction.Code == 5)
                    {
                        this.mGenome.MoveNext();
                        this.mGenome.MoveNext();
                    }
                    //    if (mGenome.Instruction == instruction)
                    //    {
                    //        this.mGenome.MoveNext();
                    //        break;
                    //    }
                    //    this.mGenome.MoveNext();
                    //    if (mGenome.Instruction == instruction)
                    //        break;
                    //}
                }
            }
            else if (instruction.Code == 5) // Link
            {
            }
            return processTime;            
        }

        public void Move(SandBox sandbox, Vector2 displacement)
        {
            // calcualte the energy required
            this.Energy -= displacement.Length()*100f;
            this.mPosition = sandbox.Move(this.mPosition, displacement);
        }

        protected Ant Reproduce(SandBox sandbox, float percentOfEnergy)
        {
            Ant ant = null;
            ant = new Ant(this.mGenome.Mutate());
            ant.mPosition = this.mPosition;            
            ant.Energy = this.Energy * percentOfEnergy;
            this.Energy -= ant.Energy*3f;
            ant.mGeneration = this.mGeneration + 1;
            sandbox.Add(ant);
            return ant;
        }

        protected void Eat(SandBox.Tile tile)
        {
            float energy = tile.Energy * 0.001f;
            this.Energy += energy;
            tile.Energy -= energy;
        }

        public void Consume(SandBox sandbox)
        {
        }

        protected bool Condition(LinearGenomeInstruction instruction)
        {
            int test = instruction.Data[0] % 4;
            if (test == 0) // Does food exceed threshold
            {
                int cell = instruction.Data[2] % 9;
                Vector2 vect = this.mPosition;
                if (cell == 1)
                {
                    vect.X -= 1;
                    vect.Y -= 1;
                }
                else if (cell == 2)
                {                    
                    vect.Y -= 1;
                }
                else if (cell == 3)
                {
                    vect.X += 1;
                    vect.Y -= 1;
                }
                else if (cell == 4)
                {
                    vect.X -= 1;                    
                }
                else if (cell == 5)
                {
                    vect.X += 1;                    
                }
                else if (cell == 6)
                {
                    vect.X -= 1;
                    vect.Y += 1;
                }
                else if (cell == 7)
                {
                    vect.Y += 1;
                }
                else if (cell == 8)
                {
                    vect.X += 1;
                    vect.Y += 1;
                }

                SandBox.Tile tile = this.SandBox.GetTile(vect);
                if (tile.Energy > instruction.Data[1] % SandBox.Tile.MaxEnergy)
                    return true;
            }
            else if (test == 1) // Is Energy Level above a certain value
            {               
                if (this.Energy > instruction.Data[1])
                    return true;
            }
            else if (test == 2) // Is age less then a certain threshold
            {
                if (this.Age < instruction.Data[1] % 100)
                    return true;
            }
            else if (test == 3) // Is age greater then a certain threshold
            {
                if (this.Age > instruction.Data[1] % 100)
                    return true;
            }    
            return false;
        }

        protected SandBox.Tile GetTile()
        {
            return this.SandBox.GetTile(this.mPosition);
        }
        #endregion        

        #region State Management
        protected override void WriteState(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Genome");
            writer.WriteAttributeString("x", this.mPosition.X.ToString());
            writer.WriteAttributeString("y", this.mPosition.Y.ToString());
            writer.WriteAttributeString("energy", this.Energy.ToString());            

            this.mGenome.Write(writer);
            writer.WriteEndElement();
        }

        protected override void ReadState(System.Xml.XmlReader reader)
        {
            this.mPosition = new Vector2(XmlConvert.ToSingle(Inaugura.Xml.Helper.ReadAttributeValue(reader, "x", true)),XmlConvert.ToSingle(Inaugura.Xml.Helper.ReadAttributeValue(reader, "y", true)));
            this.Energy = XmlConvert.ToSingle(Inaugura.Xml.Helper.ReadAttributeValue(reader, "energy", true));

            reader.Read();
            reader.MoveToContent();
            if (reader.LocalName.Equals("Genome"))
                this.mGenome.Read(reader);
        }
        #endregion

        

        #region IDrawer Members

        public void Draw(PrimitiveBatch primitiveBatch)
        {   
            // loop through all of the stars, and tell primitive batch to draw them.
            Vector2 positition = SandBox.ToScreen(this.mPosition);
            primitiveBatch.AddVertex(positition, Color.Fuchsia);
            //float energ = this.Energy / int.MaxValue;
            //Color energyColor = new Color(new Vector3(energ, energ, energ));
            //positition.X += 1;
            //positition.Y += 1;
            //primitiveBatch.AddVertex(positition, energyColor);
            //positition.X -= 2;
            //primitiveBatch.AddVertex(positition, energyColor);
            //positition.X += 1;
            //positition.Y += 1;
            //primitiveBatch.AddVertex(positition, energyColor);           
        }

        #endregion
    }
}
