using Doggo.HumanPong.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Doggo.HumanPong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        #region Field Region
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int TargetWidth = 1280; //1920
        public const int TargetHeight = 720; //1080
        Matrix scaleMatrix;
        #endregion

        #region Property Region
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public Matrix ScaleMatrix
        {
            get { return scaleMatrix; }
        }
        #endregion

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false; //vsync
            //graphics.PreferredBackBufferFormat = SurfaceFormat.Alpha8;
            IsFixedTimeStep = false;
            if (IsFixedTimeStep)
                TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000.0f / 60);

            SetWindowResolution();
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new FrameRateCounter(this));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void SetWindowResolution(int windowWidth = TargetWidth, int windowHeight = TargetHeight)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();

            float scaleWidth = graphics.GraphicsDevice.Viewport.Width / (float)TargetWidth;
            float scaleHeight = graphics.GraphicsDevice.Viewport.Height / (float)TargetHeight;

            scaleMatrix = Matrix.CreateScale(scaleWidth, scaleHeight, 1);
        }
    }
}
