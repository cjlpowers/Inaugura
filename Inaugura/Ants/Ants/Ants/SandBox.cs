using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Inaugura.Evolution;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ants
{
    public class SandBox : Environment<Ant>, IDrawer
    {
        #region Internal Constructs
        public class Tile
        {
            public const int MaxEnergy = 50000;
            public const float Size = 1.0f; // size in screen units
            #region Properties
            public float Energy { get; set; }
            #endregion

            #region Methods
            public Tile()
            {
                this.Energy = LinearGenome.Random.Next(MaxEnergy/2);
            }

            public void ProcessEpoch()
            {
                Energy += LinearGenome.Random.Next(MaxEnergy/1000);
                Energy *= 1.015f;
                if (Energy > MaxEnergy)
                    Energy = MaxEnergy;
            }
            #endregion
        }
        #endregion

        #region Variables
        float mScreenWidth;
        float mScreenHeight;        
        float mWidth;
        float mHeight;
        float mToScreenScaleX;
        float mToScreenScaleY;
        
        List<Ant> mTempList;
        private int mSize;
        Tile[,] mTiles;
        #endregion

        #region Properties
        public int Size
        {
            get
            {
                return mSize;
            }
        }

        public float ScreenWidth
        {
            get
            {
                return this.mScreenWidth;
            }
            set
            {
                this.mScreenWidth = value;
                this.mToScreenScaleX = value / this.mWidth;
            }
        }

        public float ScreenHeight
        {
            get
            {
               return this.mScreenHeight;
            }
            set
            {
                this.mScreenHeight = value;
                this.mToScreenScaleY = value / this.mHeight;
            }
        }

        public int LongestGenome
        {
            get
            {
                int longest = 0;
                foreach (Ant ant in this)
                {
                    if (ant.GenomeLength > longest)
                        longest = ant.GenomeLength;
                    
                }
                return longest;
            }
        }

        public int ShortestGenome
        {
            get
            {
                int value = int.MaxValue;
                foreach (Ant ant in this)
                {
                    if (ant.GenomeLength < value)
                        value = ant.GenomeLength;

                }
                return value;
            }
        }

        public float MaxEnergy
        {
            get
            {
                float value = 0;
                foreach (Ant ant in this)
                {
                    if (ant.Energy > value)
                        value = ant.Energy;

                }
                return value;
            }
        }

        public float MinEnergy
        {
            get
            {
                float value = float.MaxValue;
                foreach (Ant ant in this)
                {
                    if (ant.Energy < value)
                        value = ant.Energy;

                }
                return value;
            }
        }

        public int MaxGeneration
        {
            get
            {
                int largest = 0;
                foreach (Ant ant in this)
                {
                    if (ant.Generation > largest)
                        largest = ant.Generation;

                }
                return largest;
            }
        }

        public int MinGeneration
        {
            get
            {
                int value = int.MaxValue;
                foreach (Ant ant in this)
                {
                    if (ant.Generation < value)
                        value = ant.Generation;

                }
                return value;
            }
        }
        #endregion

        #region Methods
        public SandBox(int size)
        {
            this.mTempList = new List<Ant>();
            this.mTiles = new Tile[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    mTiles[x, y] = new Tile();

            this.mSize = size;
            this.mWidth = size * Tile.Size;
            this.mHeight = size * Tile.Size;
        }

        public Vector2 Move(Vector2 position, Vector2 displacement)
        {
            position += displacement;
                        
            if (position.X < 0)
                position.X += this.mWidth;
            if (position.X >= this.mWidth)
                position.X -= this.mWidth;
            if (position.Y < 0)
                position.Y += this.mHeight;
            if (position.Y >= this.mHeight)
                position.Y -= this.mHeight;
            return position;
        }

        public Vector2 ToScreen(Vector2 vector)
        {
            return this.ToScreen(vector.X, vector.Y);
        }

        public Vector2 ToScreen(float x, float y)
        {
            return new Vector2(x*mToScreenScaleX, y*mToScreenScaleY);
        }

        public Tile GetTile(Vector2 position)
        {
            return this.GetTile(position.X, position.Y);
        }

        public Tile GetTile(float x, float y)
        {
            int iX = (int)x;
            int iY = (int)y;
            if (iX < 0)
                iX += Size;
            if (iX >= Size)
                iX -= Size;

            if (iY < 0)
                iY += Size;
            if (iY >= Size)
                iY -= Size;
            return this.mTiles[iX, iY];
        }
        #endregion

        protected override void RunEpoch()
        {
            if (this.Count < 100 || this.Count > 5000)
            {
                string s = string.Empty;
            }

            for(int y=0; y < mSize; y++)
                for (int x = 0; x < mSize; x++)
                {
                    Tile t = this.mTiles[x, y];
                    t.ProcessEpoch();
                }

            // if there are less then 50 ants insert
            for (int i = this.Count; i < 10; i++)
                this.Add(new Ant(new Vector2(LinearGenome.Random.Next(this.Size) + (float)LinearGenome.Random.NextDouble(), LinearGenome.Random.Next(this.Size) + (float)LinearGenome.Random.NextDouble()), 2 + LinearGenome.Random.Next(10)));

            mTempList.Clear();
            mTempList.AddRange(this);

            // process the organisms
            foreach (Ant ant in mTempList)
                ant.ProcessEpoch(this);
        }


        protected override void InsertItem(int index, Ant item)
        {
            base.InsertItem(index, item);
            item.SandBox = this;
        }

        protected override void WriteState(System.Xml.XmlWriter writer)
        {            
        }

        protected override void ReadState(System.Xml.XmlReader reader)
        {            
        }

        #region IDrawer Members
        public void Draw(PrimitiveBatch primitiveBatch)
        {
            Vector2 vect;
            primitiveBatch.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList);
            // draw the tiles            
            for (int y = 0; y < mSize; y++)
                for (int x = 0; x < mSize; x++)
                {
                    Tile t = this.mTiles[x, y];
                    float energ = t.Energy / Tile.MaxEnergy;
                    if (energ > 1)
                        energ = 1;
                    Color color = new Color(new Vector3(energ, energ, energ));

                    vect = ToScreen(Tile.Size * x, Tile.Size * y);
                    vect.X += 1;
                    vect.Y += 1;
                    primitiveBatch.AddVertex(vect, color);
                    vect = ToScreen(Tile.Size * x + Tile.Size, Tile.Size * y);
                    vect.X -= 1;
                    vect.Y += 1;
                    primitiveBatch.AddVertex(vect, color);

                    vect = ToScreen(Tile.Size * x + Tile.Size, Tile.Size * y);
                    vect.X -= 1;
                    vect.Y += 1;
                    primitiveBatch.AddVertex(vect, color);
                    vect = ToScreen(Tile.Size * x + Tile.Size, Tile.Size * y + Tile.Size);
                    vect.X -= 1;
                    vect.Y -= 1;
                    primitiveBatch.AddVertex(vect, color);

                    vect = ToScreen(Tile.Size * x + Tile.Size, Tile.Size * y + Tile.Size);
                    vect.X -= 1;
                    vect.Y -= 1;
                    primitiveBatch.AddVertex(vect, color);
                    vect = ToScreen(Tile.Size * x, Tile.Size * y + Tile.Size);
                    vect.X += 1;
                    vect.Y -= 1;
                    primitiveBatch.AddVertex(vect, color);

                    vect = ToScreen(Tile.Size * x, Tile.Size * y + Tile.Size);
                    vect.X += 1;
                    vect.Y -= 1;
                    primitiveBatch.AddVertex(vect, color);
                    vect = ToScreen(Tile.Size * x, Tile.Size * y);
                    vect.X += 1;
                    vect.Y += 1;
                    primitiveBatch.AddVertex(vect, color);
                }
            primitiveBatch.End();

            primitiveBatch.Begin(PrimitiveType.PointList);
            foreach (Ant ant in this)
                ant.Draw(primitiveBatch);
            primitiveBatch.End();
        }
        #endregion
    }
}
