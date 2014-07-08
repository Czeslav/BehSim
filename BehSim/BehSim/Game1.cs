using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using BehSimLib;
using BehSimLib.Blobs;
using BehSimLib.Controlers;

namespace BehSim
{
    /*
     * TODO LIST
     * 
     * -stop blobs moving away from screen
     * -prevent blobs from hitting each other
     * -add game item list or something
     * 
     * for now all, will be updatet A LOT later
     */


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BlobController testController;

        public Game1()
        {
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

       


        protected override void Initialize()
        {

            base.Initialize();
        }

        

        protected override void LoadContent()
        {
            Blob.SetTexture(Content.Load<Texture2D>("blob"));
            Blob.SetViewFieldTexture(Content.Load<Texture2D>("shadow"));
            BlobController.ScreenDim.X = GraphicsDevice.Viewport.Width;
            BlobController.ScreenDim.Y = GraphicsDevice.Viewport.Height;

            InfoBox.Load(Content.Load<SpriteFont>("InfoBoxFont"), Content.Load<Texture2D>("InfoBoxBackground"));
            spriteBatch = new SpriteBatch(GraphicsDevice);

            testController = new BlobController();
            Vector2 screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            for (int i = 0; i < 20; i++)
            {
                testController.AddBlob(screenCenter);
            }
            

        }

       

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            testController.Update();

            base.Update(gameTime);
        }

        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
                testController.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
