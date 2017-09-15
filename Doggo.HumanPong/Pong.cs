using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Doggo.HumanPong.Components.WindowObjects.Paddle;
using Doggo.HumanPong.Components.WindowObjects.FrameCounter;

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

        Paddle Player1;
        Paddle Player2;
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

            //graphics.IsFullScreen = true; 
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // create a solid color texture (without a png file), can be replaced by a premade image file using Content.Load<Texture2D>("contentFolderStructure\fileNameWithoutExtension")
            int paddleWidth = 5;
            int paddleHeight = 100;

            Texture2D paddleTexture = new Texture2D(GraphicsDevice, paddleWidth, paddleHeight);
            Color[] paddleTextureData = new Color[paddleTexture.Width * paddleTexture.Height];
            for (int i = 0; i < paddleTextureData.Length; ++i)
                paddleTextureData[i] = Color.Red;
            paddleTexture.SetData(paddleTextureData);
            
            // distance from the side of the screen
            float offset = TargetWidth * 0.01f;

            Vector2 positionP1 = new Vector2(offset - (paddleTexture.Width / 2), (TargetHeight - paddleTexture.Height) / 2);
            Player1 = new Paddle(this, paddleTexture, positionP1);

            Vector2 positionP2 = new Vector2((TargetWidth - offset) - (paddleTexture.Width / 2), (TargetHeight - paddleTexture.Height) / 2);
            Player2 = new Paddle(this, paddleTexture, positionP2);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // the scaling will have a bug when in 4:3 fullscreen it won't display the pads in the correct position
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scaleMatrix);
            Player1.Draw(spriteBatch);
            Player2.Draw(spriteBatch);
            spriteBatch.End();

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
