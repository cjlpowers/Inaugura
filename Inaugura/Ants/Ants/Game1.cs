using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Ants
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PrimitiveBatch primitiveBatch;
        SandBox mSandBox;
        TimeSpan mLastTitleUpdate;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);
            Content.RootDirectory = "Content";            
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            this.mSandBox.ScreenWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            this.mSandBox.ScreenHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            this.mSandBox = new SandBox(20);

            List<Inaugura.Evolution.LinearGenomeInstruction> list = new List<Inaugura.Evolution.LinearGenomeInstruction>();
            list.Add(new Inaugura.Evolution.LinearGenomeInstruction(2, new int[] {190,190,0}));
            list.Add(new Inaugura.Evolution.LinearGenomeInstruction(1, new int[] {0,0,0}));
            list.Add(new Inaugura.Evolution.LinearGenomeInstruction(4, new int[] {1,15000,0}));
            list.Add(new Inaugura.Evolution.LinearGenomeInstruction(3, new int[] { 20, 0, 0 }));
            AntGenome genome = new AntGenome(list);            
            Ant ant = new Ant(genome);
            ant.Position = new Vector2(4.0f,4.0f);
            this.mSandBox.Add(ant);


            Window_ClientSizeChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            this.mSandBox.Epoch();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            this.mSandBox.ScreenWidth = graphics.GraphicsDevice.Viewport.Width;
            this.mSandBox.ScreenHeight = graphics.GraphicsDevice.Viewport.Height;
            
            // stars are drawn as a list of points, so begin the primitiveBatch.
            this.mSandBox.Draw(primitiveBatch);

            base.Draw(gameTime);

            if (this.mLastTitleUpdate == TimeSpan.Zero || gameTime.TotalRealTime.TotalSeconds - this.mLastTitleUpdate.TotalSeconds > 1)
            {
                this.mLastTitleUpdate = gameTime.TotalRealTime;
                this.Window.Title = string.Format("Age: {0}, Ants: {1}, Generation (min/max): {2}/{3}, Genome (shortest/longest): {4}/{5}, Energy (max/min): {5}/{6}", this.mSandBox.Age, this.mSandBox.Count, this.mSandBox.MinGeneration, this.mSandBox.MaxGeneration, this.mSandBox.ShortestGenome, this.mSandBox.LongestGenome, this.mSandBox.MaxEnergy, this.mSandBox.MinEnergy);
            }
        }
    }
}

